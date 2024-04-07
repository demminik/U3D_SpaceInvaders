using SpaceInvaders.Gameplay.Combat;
using SpaceInvaders.Gameplay.Entities;
using System;
using System.Collections.Generic;

namespace SpaceInvaders.Gameplay.Accessors {

    // simple wrapper in case if wrapped type or structure will change
    public class EnemyShipsAccessor : IDisposable {

        // list usage is justified because it's better for this specific case:
        // 1. array iteration is faster but removing item from array will lead to NULLifying the element
        // 2. since it's unity object, it has null reference comparition override which is not fast
        // 3. enemy rows are not expected to be huge (like millions of records) so performance-wise it will almost not differ from arrays iteration
        private List<List<Ship>> _enemyShips;
        public List<List<Ship>> EnemyShips {
            get => _enemyShips;
            set {
                UnsubscribeFromShipEvents();
                _enemyShips = value;
                SubscribeToShipEvents();

                CountShips();
            }
        }

        public int ShipsCount { get; private set; } = 0;

        public event Action<Ship> OnShipDestroyed;

        // TODO: call
        public void Dispose() {
            EnemyShips = null;
            OnShipDestroyed = null;
        }

        private void ProcessShipDestroyed(Ship ship) {
            var shipHealth = ship.GetComponentInChildren<IShipHealth>();
            if (shipHealth != null) {
                shipHealth.OnDeath -= ProcessShipDestroyed;
            }

            if (_enemyShips != null) {
                foreach (var row in _enemyShips) {
                    if (row.Remove(ship)) {
                        ShipsCount--;
                        break;
                    }
                }
            }

            OnShipDestroyed?.Invoke(ship);
        }

        private void SubscribeToShipEvents() {
            if (_enemyShips != null) {
                foreach (var row in _enemyShips) {
                    foreach (var ship in row) {
                        var health = ship.GetComponentInChildren<IShipHealth>();
                        if (health != null) {
                            health.OnDeath -= ProcessShipDestroyed;
                            health.OnDeath += ProcessShipDestroyed;
                        }
                    }
                }
            }
        }

        private void UnsubscribeFromShipEvents() {
            if (_enemyShips != null) {
                foreach (var row in _enemyShips) {
                    foreach (var ship in row) {
                        var health = ship.GetComponentInChildren<IShipHealth>();
                        if (health != null) {
                            health.OnDeath -= ProcessShipDestroyed;
                        }
                    }
                }
            }
        }

        private void CountShips() {
            ShipsCount = 0;
            if (_enemyShips != null) {

                foreach (var row in _enemyShips) {
                    ShipsCount += row.Count;
                }
            }
        }
    }
}