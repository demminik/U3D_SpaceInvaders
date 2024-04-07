using SpaceInvaders.Configs;
using SpaceInvaders.Gameplay.Weapons;

namespace SpaceInvaders.Gameplay.Spawners {

    public class WeaponSpawner : GenericSpawner<WeaponsConfig, EWeaponType, Weapon> {

        public WeaponSpawner(WeaponsConfig config) : base(config) {
        }

        protected override bool CanBeSpawned(EWeaponType type) {
            return type != EWeaponType.None;
        }

        protected override bool TryGetPrefabFromConfig(EWeaponType type, out Weapon result) {
            return _config.TryGetWeaponPrefab(type, out result);
        }

        protected override void SetupSpawnedInstance(EWeaponType type, Weapon instance) {
            var projectileType = _config.GetWeaponProjectile(type);
            instance.InstallProjectile(projectileType);
        }
    }
}