using SpaceInvaders.Gameplay.Entities;
using SpaceInvaders.Gameplay.Spawners;
using SpaceInvaders.Gameplay.Utils;
using SpaceInvaders.Gameplay.Weapons;
using System;
using UnityEngine;
using Zenject;

namespace SpaceInvaders.Gameplay.Combat {

    // TODO: add invincibility window after respawn (not important for demo)
    public class ShipHitBehaviour : MonoBehaviour, IProjectileHitReceiver, IShipHitReceiver, IShipHealth {

        [SerializeField]
        private Ship _ship;

        private ShipSpawner _shipSpawner;

        public event Action<Ship> OnDeath;

        [Inject]
        private void HandleInjection(ShipSpawner shipSpawner) {
            _shipSpawner = shipSpawner;
        }

        public bool CanReceiveHitFrom(Projectile hitter) {
            // TODO: add teams support (probably not important for demo)
            return true;
        }

        public bool ReceiveHit(Projectile hitter) {
            if(CanReceiveHitFrom(hitter) && _ship != null) {
                PerformDeathSequence();
                return true;
            }

            return false;
        }

        public bool CanReceiveHitFrom(Ship hitter) {
            return TeamUtils.GetTeamRelationType(hitter.gameObject, gameObject) == ETeamRelationType.Enemy;
        }

        public bool ReceiveHit(Ship hitter) {
            if (CanReceiveHitFrom(hitter) && _ship != null) {
                PerformDeathSequence();
                return true;
            }

            return false;
        }

        private void PerformDeathSequence() {
            OnDeath?.Invoke(_ship);

            _ship.InstallWeapon(EWeaponType.None);
            _shipSpawner.Despawn(_ship);
        }
    }
}