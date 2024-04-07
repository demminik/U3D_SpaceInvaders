using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SpaceInvaders.Gameplay.UI {

    public class GameplayLossScreen : MonoBehaviour {

        [SerializeField]
        private Button _btnRestart;

        private ReactiveCommand<EGameplayCommand> _gameplayCommand;

        protected CompositeDisposable _disposable = new CompositeDisposable();

        [Inject]
        private void HandleInjection(ReactiveCommand<EGameplayCommand> gameplayCommand) {
            _gameplayCommand = gameplayCommand;
        }

        private void Awake() {
            _btnRestart.onClick.AddListener(StartGame);
        }

        private void StartGame() {
            _gameplayCommand.Execute(EGameplayCommand.Restart);
        }
    }
}