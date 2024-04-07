using SpaceInvaders.Gameplay.Accessors;
using SpaceInvaders.Gameplay.Entities;
using Zenject;

namespace SpaceInvaders.Gameplay {

    public class GameplayWavesManager : BaseGameplayBehaviour {

        private EnemyShipsAccessor _enemyShipsAccessor;

        [Inject]
        private void HandleInjection(EnemyShipsAccessor enemyShipsAccessor) {
            _enemyShipsAccessor = enemyShipsAccessor;
            _enemyShipsAccessor.OnShipDestroyed += OnEnemyDeath;
        }

        protected override void OnDestroy() {
            base.OnDestroy();

            if (_enemyShipsAccessor != null) {
                _enemyShipsAccessor.OnShipDestroyed -= OnEnemyDeath;
                _enemyShipsAccessor = null;
            }
        }

        private void OnEnemyDeath(Ship ship) {
            if(_enemyShipsAccessor.ShipsCount <= 0) {
                _gameplayCommand.Execute(EGameplayCommand.NextWave);
            }
        }
    }
}