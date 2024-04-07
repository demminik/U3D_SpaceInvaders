using SpaceInvaders.Gameplay.Combat;
using SpaceInvaders.Gameplay.Spawners;
using SpaceInvaders.Gameplay.Weapons;
using SpaceInvaders.Ships;
using System;
using UnityEngine;
using Zenject;

namespace SpaceInvaders.Gameplay.Entities {
    
    // TODO: add more behaviours (state, etc, not important for demo)
    public class Ship : BaseGameplayBehaviour {

        [SerializeField]
        private EShipType _type;
        public EShipType Type => _type;

        [SerializeField]
        private ShipBoostBehaviour _boostBehaviour;
        public ShipBoostBehaviour BoostBehaviour => _boostBehaviour;

        [SerializeField]
        private Transform _weaponRoot;

        [SerializeField]
        private Transform _dropRoot;
        public Transform DropRoot => _dropRoot;

        private Weapon _currentWeapon;
        public Weapon CurrentWeapon {
            get => _currentWeapon;
            private set {
                _currentWeapon = value;
                OnWeaponChanged?.Invoke(_currentWeapon);
            }
        }

        private WeaponSpawner _weaponSpawner;

        public event Action<Weapon> OnWeaponChanged;

        [Inject]
        private void HandleInjection(WeaponSpawner weaponSpawner) {
            _weaponSpawner = weaponSpawner;
        }

        protected override void OnDestroy() {
            base.OnDestroy();

            OnWeaponChanged = null;
        }

        public bool InstallWeapon(EWeaponType weaponType) {
            UninstallCurrentWeapon();

            if (weaponType == EWeaponType.None) {
                return true;
            }

            if (!_weaponSpawner.Spawn(weaponType, out var newWeapon)) {
                return false;
            }

            // TODO: remake teams assignment (not important for demo)
            newWeapon.tag = gameObject.tag;

            CurrentWeapon = newWeapon;
            CurrentWeapon.transform.parent = _weaponRoot;
            CurrentWeapon.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

            return true;
        }

        private void UninstallCurrentWeapon() {
            if (CurrentWeapon != null) {
                _weaponSpawner.Despawn(CurrentWeapon);
                CurrentWeapon = null;
            }
        }
    }
}