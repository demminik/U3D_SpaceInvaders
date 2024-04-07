using SpaceInvaders.Configs;
using SpaceInvaders.Gameplay.Accessors;
using SpaceInvaders.Gameplay.Entities;
using SpaceInvaders.Gameplay.Pooling;
using SpaceInvaders.Gameplay.Spawners;
using SpaceInvaders.Ships;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace SpaceInvaders.Gameplay.Creators {

    public class ShipCreator : BaseGameplayBehaviour {

        [SerializeReference]
        private Transform _playerShipRoot;

        [SerializeReference]
        private Transform _enemyShipsRoot;

        [SerializeField]
        private Vector2 _enemyShipOffset = new Vector2(1.5f, 1.5f);

        private ShipsConfig _shipsConfig;
        private WeaponsConfig _weaponConfig;

        private ShipSpawner _shipSpawner;

        private PlayerShipAccessor _playerShipAccessor;
        private EnemyShipsAccessor _enemyShipsAccessor;

        [Inject]
        private void HandleInjection(ShipsConfig shipsConfig,
            WeaponsConfig weaponsConfig,
            ShipSpawner shipSpawner,
            PlayerShipAccessor playerShipAccessor,
            EnemyShipsAccessor enemyShipsAccessor) {

            _shipsConfig = shipsConfig;
            _weaponConfig = weaponsConfig;

            _shipSpawner = shipSpawner;

            _playerShipAccessor = playerShipAccessor;
            _enemyShipsAccessor = enemyShipsAccessor;
        }

        protected override void ProcessGameplayCommandInternal(EGameplayCommand command) {
            switch (command) {
                case EGameplayCommand.SpawnPlayer:
                    SpawnPlayer();
                    break;
                case EGameplayCommand.SpawnEnemies:
                    SpawnEnemies();
                    break;
                case EGameplayCommand.DespawnPlayer:
                    DespawnPlayer();
                    break;
                case EGameplayCommand.DespawnEnemies:
                    DespawnEnemies();
                    break;
                case EGameplayCommand.TotalLoss:
                    // TODO: despawn projectiles somewhere (not important for demo)
                    DespawnPlayer();
                    DespawnEnemies();
                    break;
            }
        }

        private void SpawnPlayer() {
            if (_playerShipAccessor.PlayerShip != null) {
                UnityEngine.Debug.LogError($"Failed to spawn player: already spawned");
                return;
            }

            var playerShipType = _shipsConfig.PlayerShipType;
            var player = CreateShip(_shipsConfig, playerShipType);
            if (player == null) {
                UnityEngine.Debug.LogError($"Failed to spawn player");
                return;
            }
            player.InstallWeapon(_weaponConfig.GetDefaultWeaponType(playerShipType));
            player.transform.parent = _playerShipRoot;
            player.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

            _playerShipAccessor.PlayerShip = player;

            _gameplayCommand.Execute(EGameplayCommand.PlayerSpawned);
        }

        private void DespawnPlayer() {
            if (_playerShipAccessor.PlayerShip == null) {
                return;
            }

            DespawnWeapon(_playerShipAccessor.PlayerShip);
            GameObjectsPool.Instance.Release(_playerShipAccessor.PlayerShip.gameObject);

            _playerShipAccessor.PlayerShip = null;
        }

        private void SpawnEnemies() {
            if (_enemyShipsAccessor.ShipsCount > 0) {
                UnityEngine.Debug.LogError($"Failed to spawn enemies: already spawned");
                return;
            }

            if (_shipsConfig.EnemyShipRows == null || _shipsConfig.EnemyShipRows.Length == 0) {
                UnityEngine.Debug.LogError($"Failed to spawn enemies: no rows data found");
                return;
            }

            var enemies = new List<List<Ship>>();
            foreach (var item in _shipsConfig.EnemyShipRows) {
                var rowCollection = new List<Ship>();
                enemies.Add(rowCollection);

                for (int i = 0; i < item.RowSize; i++) {
                    var spawningShiType = item.ShipType;
                    var ship = CreateShip(_shipsConfig, item.ShipType);
                    if (ship == null) {
                        UnityEngine.Debug.LogError($"Failed to spawn enemy ship");
                        continue;
                    }

                    ship.InstallWeapon(_weaponConfig.GetDefaultWeaponType(spawningShiType));
                    ship.transform.parent = _enemyShipsRoot;

                    rowCollection.Add(ship);

                }
            }
            PositionEnemyShips(enemies);

            _enemyShipsAccessor.EnemyShips = enemies;

            _gameplayCommand.Execute(EGameplayCommand.EnemiesSpawned);
        }

        private void DespawnEnemies() {
            if (_enemyShipsAccessor.EnemyShips == null) {
                return;
            }

            foreach (var row in _enemyShipsAccessor.EnemyShips) {
                foreach (var enemy in row) {
                    DespawnWeapon(enemy);
                    _shipSpawner.Despawn(enemy);
                }
            }

            _enemyShipsAccessor.EnemyShips = null;
        }

        private Ship CreateShip(ShipsConfig config, EShipType type) {
            if (config == null) {
                UnityEngine.Debug.LogError($"Ship creation failed: invalid config");
                return null;
            }

            if(!config.TryGetShipPrefab(type, out var prefab)) { 
                UnityEngine.Debug.LogError($"Ship creation failed: failed to find prefab for {type}");
                return null;
            }

            _shipSpawner.Spawn(type, out var ship);
            return ship;
        }

        private void DespawnWeapon(Ship ship) {
            if(ship == null || ship.CurrentWeapon == null) {
                return;
            }

            ship.InstallWeapon(Weapons.EWeaponType.None);
        }

        private void PositionEnemyShips(List<List<Ship>> enemies) {
            var rowNumber = 0;
            foreach (var row in enemies) {
                var rowSize = row.Count;
                var startPositionInRow = (Mathf.FloorToInt(rowSize * 0.5f) * _enemyShipOffset.x) + (_enemyShipOffset.x * (rowSize % 2 > 0 ? 0.5f : 0f));
                var nextPositionInRow = startPositionInRow * -1f;
                foreach (var enemy in row) {
                    enemy.transform.SetLocalPositionAndRotation(new Vector3(nextPositionInRow, -_enemyShipOffset.y * rowNumber, 0f), Quaternion.Euler(new Vector3(0f, 0f, 180f)));
                    nextPositionInRow += _enemyShipOffset.x;
                }
                rowNumber++;
            }
        }
    }
}