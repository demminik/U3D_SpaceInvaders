using SpaceInvaders.Configs;
using SpaceInvaders.Gameplay.Weapons;

namespace SpaceInvaders.Gameplay.Spawners {

    public class ProjectileSpawner : GenericSpawner<WeaponsConfig, EProjectileType, Projectile> {

        public ProjectileSpawner(WeaponsConfig config) : base(config) {
        }

        protected override bool CanBeSpawned(EProjectileType type) {
            return type != EProjectileType.None;
        }

        protected override bool TryGetPrefabFromConfig(EProjectileType type, out Projectile result) {
            return _config.TryGetProjectilePrefab(type, out result);
        }
    }
}