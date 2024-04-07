using SpaceInvaders.Gameplay.Accessors;
using SpaceInvaders.Gameplay.Combat;
using SpaceInvaders.Gameplay.Entities;
using SpaceInvaders.Gameplay.Meta;
using UniRx;
using Zenject;

namespace SpaceInvaders.Gameplay {

    // TODO: add global manager for stats reset
    public class GameplayScoreManager : BaseGameplayBehaviour {

        private GameplayStats _stats;
        private EnemyShipsAccessor _enemyShipsAccessor;
        
        [Inject]
        private void HandleInjection(GameplayStats stats,
            EnemyShipsAccessor enemyShipsAccessor) {

            _stats = stats;

            _enemyShipsAccessor = enemyShipsAccessor;
        }

        protected override void ProcessGameplayStateChangedInternal(EGameplayState state) {
            switch (state) {
                case EGameplayState.Lost:
                    ProcessLoss();
                    break;
            }
        }

        protected override void ProcessGameplayCommandInternal(EGameplayCommand command) {
            switch (command) {
                case EGameplayCommand.EnemiesSpawned:
                    ProcessEnemiesSpawned();
                    break;
            }
        }

        private void ProcessLoss() {
            foreach (var row in _enemyShipsAccessor.EnemyShips) {
                foreach (var enemy in row) {
                    var shipHealth = enemy.GetComponentInChildren<IShipHealth>();
                    if (shipHealth != null) {
                        shipHealth.OnDeath -= OnEnemyDeath;
                    }
                }
            }
        }

        private void ProcessEnemiesSpawned() {
            foreach (var row in _enemyShipsAccessor.EnemyShips) {
                foreach (var enemy in row) {
                    var shipHealth = enemy.GetComponentInChildren<IShipHealth>();
                    if (shipHealth != null) {
                        shipHealth.OnDeath += OnEnemyDeath;
                    }
                }
            }
        }

        private void OnEnemyDeath(Ship ship) {
            var shipHealth = ship.GetComponentInChildren<IShipHealth>();
            shipHealth.OnDeath -= OnEnemyDeath;

            _stats.Score.Value++;
        }
    }
}