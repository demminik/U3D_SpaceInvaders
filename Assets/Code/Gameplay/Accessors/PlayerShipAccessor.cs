using SpaceInvaders.Gameplay.Combat;
using SpaceInvaders.Gameplay.Entities;
using System;

namespace SpaceInvaders.Gameplay.Accessors {

    // simple wrapper in case if wrapped type or structure will change
    public class PlayerShipAccessor : IDisposable {

        private Ship _playerShip;
        public Ship PlayerShip {
            get => _playerShip;
            set {
                var previousShip = _playerShip;

                UnsubscribeFromShipEvents();
                _playerShip = value;
                SubscribeToShipEvents();

                OnShipChanged?.Invoke(_playerShip);
            }
        }

        // TODO: call
        public void Dispose() {
            PlayerShip = null;
            OnShipDestroyed = null;
        }

        public event Action<Ship> OnShipChanged;
        public event Action<Ship> OnShipDestroyed;

        private void ProcessShipDestroyed(Ship ship) {
            OnShipDestroyed?.Invoke(ship);
        }

        private void SubscribeToShipEvents() {
            if (_playerShip != null) {
                var health = _playerShip.GetComponentInChildren<IShipHealth>();
                if (health != null) {
                    health.OnDeath -= ProcessShipDestroyed;
                    health.OnDeath += ProcessShipDestroyed;
                }
            }
        }

        private void UnsubscribeFromShipEvents() {
            if (_playerShip != null) {
                var health = _playerShip.GetComponentInChildren<IShipHealth>();
                if (health != null) {
                    health.OnDeath -= ProcessShipDestroyed;
                }
            }
        }
    }
}