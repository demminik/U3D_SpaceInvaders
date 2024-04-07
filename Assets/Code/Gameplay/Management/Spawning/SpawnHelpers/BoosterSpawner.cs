using SpaceInvaders.Configs;
using SpaceInvaders.Gameplay.Boosters;

namespace SpaceInvaders.Gameplay.Spawners {

    public class BoosterSpawner : GenericSpawner<BoostersConfig, EBoosterType, Booster> {

        public BoosterSpawner(BoostersConfig config) : base(config) {
        }

        protected override bool CanBeSpawned(EBoosterType type) {
            return type != EBoosterType.None;
        }

        protected override bool TryGetPrefabFromConfig(EBoosterType type, out Booster result) {
            return _config.TryGetBoosterPrefab(type, out result);
        }
    }
}