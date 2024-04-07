using SpaceInvaders.Gameplay.Common;
using SpaceInvaders.Gameplay.Entities;
using SpaceInvaders.Gameplay.Spawners;
using SpaceInvaders.Gameplay.Utils;
using UnityEngine;
using Zenject;

namespace SpaceInvaders.Gameplay.Boosters {

    // TODO: very similar to Projectile, need to extract more elements (lazy)
    public abstract class Booster : BaseGameplayBehaviour {

        [SerializeField]
        private EBoosterType _type;
        public EBoosterType Type => _type;

        [SerializeField]
        private float _duration;
        public float Duration => _duration;

        [SerializeField]
        private LimitedLifetimeBehaviour _lifetimeBehaviour;

        [SerializeField]
        private ConstantSpeedMovementBehaviour _movementBehaviour;
        
        private BoosterSpawner _boosterSpawner;

        // TODO: need to create methods ProcessPoolRetrieve and ProcessPoolRelease (IPoolable) (not important for demo)
        // thos flag is bad and can easily break logic
        private bool _isLaunched = false;

        [Inject]
        private void HandleInjection(BoosterSpawner boosterSpawner) {
            _boosterSpawner = boosterSpawner;
        }

        protected override void ProcessGameplayCommandInternal(EGameplayCommand command) {
            switch (command) {
                case EGameplayCommand.Restart:
                case EGameplayCommand.TotalLoss:
                    Despawn();
                    break;
            }
        }

        protected abstract void ApplyTo(Ship target);

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

        private void OnTriggerEnter(Collider other) {
            var goOther = other.gameObject;

            var ship = goOther.GetComponent<Ship>();
            if(ship == null) {
                return;
            }

            var hitObjectTeamRelations = TeamUtils.GetTeamRelationType(ship.gameObject, gameObject);
            if (hitObjectTeamRelations == ETeamRelationType.Ally) {
                ApplyTo(ship);
                Despawn();
            }
        }

        private void Despawn() {
            if (_isLaunched) {
                Stop();
                _boosterSpawner.Despawn(this);
            }
        }
    }
}