using SpaceInvaders.Gameplay.Weapons;

namespace SpaceInvaders.Gameplay.Combat {

    public interface IProjectileHitReceiver {

        public bool CanReceiveHitFrom(Projectile hitter);

        public bool ReceiveHit(Projectile hitter);
    }
}