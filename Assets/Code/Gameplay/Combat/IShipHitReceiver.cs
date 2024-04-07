using SpaceInvaders.Gameplay.Entities;

namespace SpaceInvaders.Gameplay.Combat {

    public interface IShipHitReceiver {

        public bool CanReceiveHitFrom(Ship hitter);

        public bool ReceiveHit(Ship hitter);
    }
}