using SpaceInvaders.Utils;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace SpaceInvaders.Gameplay.UI {

    public class GameplayGreetingsScreen : MonoBehaviour {

        [SerializeField]
        private RectTransform _controlsMobile;

        [SerializeField]
        private RectTransform _controlsStandalone;

        [SerializeField]
        private Button _btnPlay;

        private ReactiveCommand<EGameplayCommand> _gameplayCommand;

        protected CompositeDisposable _disposable = new CompositeDisposable();

        [Inject]
        private void HandleInjection(ReactiveCommand<EGameplayCommand> gameplayCommand) {
            _gameplayCommand = gameplayCommand;
        }

        private void Awake() {
            _controlsMobile.gameObject.SetActive(PlatformUtils.CurrentPlatformType == PlatformUtils.EPlatformType.Mobile);
            _controlsStandalone.gameObject.SetActive(PlatformUtils.CurrentPlatformType == PlatformUtils.EPlatformType.Standalone);

            _btnPlay.onClick.AddListener(StartGame);
        }

        private void StartGame() {
            _gameplayCommand.Execute(EGameplayCommand.Restart);
        }
    }
}