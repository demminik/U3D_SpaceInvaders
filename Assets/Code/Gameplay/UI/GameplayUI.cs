using UniRx;
using UnityEngine;
using Zenject;

namespace SpaceInvaders.Gameplay.UI {

    public class GameplayUI : MonoBehaviour {

        [SerializeField]
        private GameplayGreetingsScreen _greetingsScreen;

        [SerializeField]
        private GameplayLossScreen _lossScreen;

        [SerializeField]
        private GameplayHUD _gameplayHud;

        [SerializeField]
        private GameplayPauseScreen _pauseScreen;

        private ReactiveProperty<EGameplayState> _gameplayState;

        protected CompositeDisposable _disposable = new CompositeDisposable();

        [Inject]
        private void HandleInjection(ReactiveProperty<EGameplayState> gameplayState) {
            _gameplayState = gameplayState;
            _gameplayState.Subscribe(ProcessGameplayStateChanged).AddTo(_disposable);
        }

        private void Awake() {
            ActualizeScreensDisplay(EGameplayState.Idle);
        }

        private void OnDestroy() {
            if (!_disposable.IsDisposed) {
                _disposable.Dispose();
            }
        }

        private void ProcessGameplayStateChanged(EGameplayState gmeplayState) {
            ActualizeScreensDisplay(gmeplayState);
        }

        private void ActualizeScreensDisplay(EGameplayState gameplayState) {
            _greetingsScreen.gameObject.SetActive(gameplayState == EGameplayState.Idle);
            _gameplayHud.gameObject.SetActive(gameplayState == EGameplayState.Playing || gameplayState == EGameplayState.NewWavePrepartion);
            _lossScreen.gameObject.SetActive(gameplayState == EGameplayState.Lost);
            _pauseScreen.gameObject.SetActive(gameplayState == EGameplayState.Paused);
        }
    }
}