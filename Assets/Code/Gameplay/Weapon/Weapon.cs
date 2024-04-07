using SpaceInvaders.Configs;
using SpaceInvaders.Gameplay.Spawners;
using UnityEngine;
using Zenject;

namespace SpaceInvaders.Gameplay.Weapons {

    public class Weapon : BaseGameplayBehaviour {

        [SerializeField]
        private EWeaponType _type;
        public EWeaponType Type => _type;

        [SerializeField]
        protected Transform[] _launchTransforms;
        public Transform[] LaunchTransforms => _launchTransforms;

        // TODO: this should be ni weapon config (not important for demo)
        [SerializeField]
        protected float _fireCooldown;

        public bool ReadyToShoot => Time.time >= _nextPossibleLaunchTime;

        protected float _nextPossibleLaunchTime = 0f;

        private WeaponsConfig _weaponConfig;
        private ProjectileSpawner _projectileSpawner;

        private EProjectileType _projectileType;

        [Inject]
        private void HandleInjection(WeaponsConfig weaponConfig,
            ProjectileSpawner projectileSpawner) {

            _weaponConfig = weaponConfig;
            _projectileSpawner = projectileSpawner;
        }

        public void InstallProjectile(EProjectileType projectileType) {
            _projectileType = projectileType;
        }

        public void Shoot() {
            if(!ReadyToShoot) {
                return;
            }

            SpawnAndLaunchProjectiles();
            StartCooldown();
        }

        protected void StartCooldown() {
            _nextPossibleLaunchTime = Time.time + _fireCooldown;
        }

        protected void SpawnAndLaunchProjectiles() {
            if(_projectileType == EProjectileType.None) {
                return;
            }

            foreach (var launchPoint in _launchTransforms) {
                if(_projectileSpawner.Spawn(_projectileType, out var projectile)) {
                    // TODO: remake teams assignment (not important for demo)
                    projectile.tag = gameObject.tag;
                    projectile.transform.SetPositionAndRotation(launchPoint.transform.position, launchPoint.transform.rotation);
                    projectile.Launch();
                }
            }
        }
    }
}