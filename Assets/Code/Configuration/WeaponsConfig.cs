using SpaceInvaders.Gameplay.Weapons;
using SpaceInvaders.Ships;
using System;
using UnityEngine;

namespace SpaceInvaders.Configs {

    [CreateAssetMenu(fileName = "WeaponsConfig", menuName = "Configs/WeaponsConfig")]
    public class WeaponsConfig : ScriptableObject {

        [Serializable]
        private struct WeaponConfig {

            public Weapon Prefab;

            public EProjectileType ProjectileType;
        }

        [Serializable]
        private struct ProjectileConfig {

            public Projectile Prefab;
        }

        // TODO: whis should be in separate config (not important for demo)
        [Serializable]
        public struct DefaultWeapon {

            public EShipType ShipType;
            public EWeaponType WeaponType;
        }

        // TODO: add vaildation (not important for demo)
        [SerializeField]
        private WeaponConfig[] _weapons;

        // TODO: add vaildation (not important for demo)
        [SerializeField]
        private ProjectileConfig[] _projectiles;

        // TODO: add vaildation (not important for demo)
        [SerializeField]
        private DefaultWeapon[] _defaultWeapons;

        public bool TryGetWeaponPrefab(EWeaponType type, out Weapon result) {
            // TODO: cache (not important for demo)
            foreach (var item in _weapons) {
                if (item.Prefab != null && item.Prefab.Type == type) {
                    result = item.Prefab;
                    return true;
                }
            }

            result = null;
            return false;
        }

        public bool TryGetProjectilePrefab(EProjectileType type, out Projectile result) {
            // TODO: cache (not important for demo)
            foreach (var item in _projectiles) {
                if (item.Prefab != null && item.Prefab.Type == type) {
                    result = item.Prefab;
                    return true;
                }
            }

            result = null;
            return false;
        }

        public EWeaponType GetDefaultWeaponType(EShipType shipType) {
            // TODO: cache (not important for demo)
            foreach (var item in _defaultWeapons) {
                if (item.ShipType == shipType) {
                    return item.WeaponType;
                }
            }
            return EWeaponType.None;
        }

        public EProjectileType GetWeaponProjectile(EWeaponType weaponType) {
            // TODO: cache (not important for demo)
            foreach (var item in _weapons) {
                if (item.Prefab != null && item.Prefab.Type == weaponType) {
                    return item.ProjectileType;
                }
            }

            return EProjectileType.None;
        }
    }
}