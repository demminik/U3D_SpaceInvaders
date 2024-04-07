using SpaceInvaders.Gameplay.Combat;
using SpaceInvaders.Gameplay.Common;
using SpaceInvaders.Gameplay.Spawners;
using SpaceInvaders.Gameplay.Utils;
using UnityEngine;
using Zenject;

namespace SpaceInvaders.Gameplay.Weapons {

    // TODO: very similar to Booster, need to extract more elements (lazy)
    public class Projectile : BaseGameplayBehaviour {

        [SerializeField]
        private EProjectileType _type;
        public EProjectileType Type => _type;

        [SerializeField]
        private LimitedLifetimeBehaviour _lifetimeBehaviour;

        [SerializeField]
        private ConstantSpeedMovementBehaviour _movementBehaviour;

        private ProjectileSpawner _projectileSpawner;

        // TODO: need to create methods ProcessPoolRetrieve and ProcessPoolRelease (IPoolable) (not important for demo)
        // thos flag is bad and can easily break logic
        private bool _isLaunched = false;

        [Inject]
        private void HandleInjection(ProjectileSpawner projectileSpawner) {
            _projectileSpawner = projectileSpawner;
        }

        public void Launch() {
            _isLaunched = true;

            _lifetimeBehaviour.OnLifetimeEnded -= Despawn;
            _lifetimeBehaviour.OnLifetimeEnded += Despawn;

            _lifetimeBehaviour.Launch();
            _movementBehaviour.Launch();
        }

        private void Stop() {
            _isLaunched = false;

            _lifetimeBehaviour.OnLifetimeEnded -= Despawn;

            _lifetimeBehaviour.Stop();
            _movementBehaviour.Stop();
        }

        protected override void ProcessGameplayCommandInternal(EGameplayCommand command) {
            switch (command) {
                case EGameplayCommand.Restart:
                case EGameplayCommand.TotalLoss:
                    Despawn();
                    break;
            }
        }

        private void OnTriggerEnter(Collider other) {
            var goOther = other.gameObject;

            var hitObjectTeamRelations = TeamUtils.GetTeamRelationType(gameObject, goOther);
            if (hitObjectTeamRelations == ETeamRelationType.Enemy || hitObjectTeamRelations == ETeamRelationType.None) {
                var hitReceiver = goOther.GetComponent<IProjectileHitReceiver>();
                if (hitReceiver != null && hitReceiver.CanReceiveHitFrom(this)) {
                    hitReceiver.ReceiveHit(this);
                    Despawn();
                }
            }
        }

        private void Despawn() {
            if (_isLaunched) {
                Stop();
                _projectileSpawner.Despawn(this);
            }
        }
    }
}