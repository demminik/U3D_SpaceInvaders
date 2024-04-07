using SpaceInvaders.Configs;
using SpaceInvaders.Gameplay.Entities;
using SpaceInvaders.Ships;

namespace SpaceInvaders.Gameplay.Spawners {

    public class ShipSpawner : GenericSpawner<ShipsConfig, EShipType, Ship> {
        public ShipSpawner(ShipsConfig config) : base(config) {
        }

        protected override bool CanBeSpawned(EShipType type) {
            return type != EShipType.None;
        }

        protected override bool TryGetPrefabFromConfig(EShipType type, out Ship result) {
            return _config.TryGetShipPrefab(type, out result);
        }
    }
}