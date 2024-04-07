using SpaceInvaders.Gameplay.Entities;
using SpaceInvaders.Gameplay.Weapons;
using UnityEngine;

namespace SpaceInvaders.Gameplay.Boosters {

    public class WeaponBooster : Booster {

        [SerializeField]
        private EWeaponType _weaponType;

        protected override void ApplyTo(Ship target) {
            if(target.BoostBehaviour != null) {
                target.BoostBehaviour.Accept(this);
            }
            target.InstallWeapon(_weaponType);
        }
    }
}