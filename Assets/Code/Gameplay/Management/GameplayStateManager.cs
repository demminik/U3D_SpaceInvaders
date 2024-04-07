using System.Collections;
using UnityEngine;

namespace SpaceInvaders.Gameplay {

    // TODO: despawn boosters and projectiles somewhere
    public class GameplayStateManager : BaseGameplayBehaviour {

        private bool _isPlayerReady = false;
        private bool _isEnemiesReady = false;

        private EGameplayState _prePauseState; 

        protected override void ProcessGameplayCommandInternal(EGameplayCommand command) {
            switch (command) {
                case EGameplayCommand.Restart:
                    Unpause();
                    _isPlayerReady = false;
                    _isEnemiesReady = false;
                    StartCoroutine(DelayedRestart());
                    break;
                case EGameplayCommand.NextWave:
                    _isEnemiesReady = false;
                    SpawnNewWave();
                    break;
                case EGameplayCommand.TotalLoss:
                    EndGame();
                    break;
                case EGameplayCommand.SpawnPlayer:
                    _isPlayerReady = false;
                    break;
                case EGameplayCommand.PlayerEmerged:
                    _isPlayerReady = true;
                    TryStartGame();
                    break;
                case EGameplayCommand.EnemiesEmerged:
                    _isEnemiesReady = true;
                    TryStartGame();
                    break;
                case EGameplayCommand.Pause:
                    _prePauseState = _gameplayState.Value;
                    _gameplayState.Value = EGameplayState.Paused;
                    
                    break;
                case EGameplayCommand.Unpause:
                    _gameplayState.Value = _prePauseState;
                    Unpause();
                    break;
                default:
                    break;
            }
        }

        private IEnumerator DelayedRestart() {
            // delay is necessary for all other restart-based scrips to work before any state changes
            yield return null;

            _gameplayCommand.Execute(EGameplayCommand.DespawnPlayer);
            _gameplayCommand.Execute(EGameplayCommand.DespawnEnemies);

            SpawnNewWave();
            _gameplayCommand.Execute(EGameplayCommand.SpawnPlayer);
        }

        private void SpawnNewWave() {
            _gameplayState.Value = EGameplayState.NewWavePrepartion;
            _gameplayCommand.Execute(EGameplayCommand.SpawnEnemies);
        }

        private void TryStartGame() {
            if (_isPlayerReady && _isEnemiesReady) {
                _gameplayState.Value = EGameplayState.Playing;
            }
        }

        private void EndGame() {
            _isPlayerReady = false;
            _isEnemiesReady = false;

            _gameplayState.Value = EGameplayState.Lost;
        }

        private void Pause() {
            // VERY lazy pause
            Time.timeScale = 0f;
        }

        private void Unpause() {
            // VERY lazy pause
            Time.timeScale = 1f;
        }
    }
}