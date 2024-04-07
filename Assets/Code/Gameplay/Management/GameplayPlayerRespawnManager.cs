using SpaceInvaders.Gameplay.Accessors;
using SpaceInvaders.Gameplay.Entities;
using SpaceInvaders.Gameplay.Meta;
using Zenject;

namespace SpaceInvaders.Gameplay {

    public class GameplayPlayerRespawnManager : BaseGameplayBehaviour {

        private GameplayStats _stats;
        private PlayerShipAccessor _playerShipAccessor;

        [Inject]
        private void HandleInjection(PlayerShipAccessor playerShipAccessor,
            GameplayStats stats) {
            _playerShipAccessor = playerShipAccessor;
            playerShipAccessor.OnShipDestroyed += OnPlayerDeath;

            _stats = stats;
        }

        protected override void OnDestroy() {
            base.OnDestroy();

            if (_playerShipAccessor != null) {
                _playerShipAccessor.OnShipDestroyed -= OnPlayerDeath;
                _playerShipAccessor = null;
            }
        }

        private void OnPlayerDeath(Ship ship) {
            _playerShipAccessor.PlayerShip = null;
            _stats.PlayerLives.Value--;
            if(_stats.PlayerLives.Value > 0) {
                _gameplayCommand.Execute(EGameplayCommand.SpawnPlayer);
            } else {
                _gameplayCommand.Execute(EGameplayCommand.TotalLoss);
            }
        }
    }
}