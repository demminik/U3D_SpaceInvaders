using SpaceInvaders.Gameplay.Accessors;
using SpaceInvaders.Gameplay.Entities;
using SpaceInvaders.Gameplay.Meta;
using SpaceInvaders.Utils;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SpaceInvaders.Gameplay {

    public class EnemyShipsHiveShootLogic : BaseGameplayBehaviour {

        [SerializeField]
        private AnimationCurve _shotCooldownCurve;

        private bool _isEnemiesReady = false;

        private GameplayStats _stats;
        private EnemyShipsAccessor _enemyShipsAccessor;

        private float _nextShotTimestamp = 0f;

        private List<Ship> _temp = new List<Ship>();

        [Inject]
        private void HandleInjection(GameplayStats stats,
            EnemyShipsAccessor enemyShipsAccessor) {

            _stats = stats;

            _enemyShipsAccessor = enemyShipsAccessor;
        }

        protected override void OnDestroy() {
            base.OnDestroy();

            _temp = null;
        }

        protected override void ProcessGameplayCommandInternal(EGameplayCommand command) {
            switch (command) {
                case EGameplayCommand.Restart:
                case EGameplayCommand.NextWave:
                case EGameplayCommand.SpawnEnemies:
                case EGameplayCommand.DespawnEnemies:
                    _isEnemiesReady = false;
                    ActializeShootingState();
                    break;
                case EGameplayCommand.EnemiesEmerged:
                    _isEnemiesReady = true;
                    ActializeShootingState();
                    break;
            }
        }

        protected override void ProcessGameplayStateChangedInternal(EGameplayState state) {
            ActializeShootingState();
        }

        private void ActializeShootingState() {
            if(_gameplayState.Value == EGameplayState.Playing && _isEnemiesReady) {
                UniRXHelper.SubscribeToUpdate(TickShooting, ref _updateDisposable);
            } else {
                UniRXHelper.UnsubscribeFromUpdate(ref _updateDisposable);
            }
        }

        private void TickShooting(long _) {
            if(_enemyShipsAccessor.EnemyShips != null) {
                if(Time.time >= _nextShotTimestamp) {
                    SelectShootingEnemies();
                    if(TryShoot()) {
                        // TODO: add excessive time compensation logic (not important for demo)
                        _nextShotTimestamp = Time.time + _shotCooldownCurve.Evaluate(_stats.WaveNumber.Value);
                    }
                }

                for (int i = _enemyShipsAccessor.EnemyShips.Count; i >= 0; i--) {

                }
            }
        }

        // lasy and stupid shooting enemies selection (not important for demo)
        private void SelectShootingEnemies() {
            _temp.Clear();
            foreach (var row in _enemyShipsAccessor.EnemyShips) {
                foreach (var ship in row) {
                    if (ship.CurrentWeapon != null && ship.CurrentWeapon.ReadyToShoot) {
                        if(Random.Range(0, 2) > 0) {
                            _temp.Add(ship);
                        }
                    }
                }
            }
        }

        private bool TryShoot() {
            if (_temp.Count > 0) {
                var index = Random.Range(0, _temp.Count - 1);
                var ship = _temp[index];
                ship.CurrentWeapon.Shoot();
                return true;
            }
            return false;
        }
    }
}