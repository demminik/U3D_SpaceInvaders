using UnityEngine;
using UnityEngine.UI;

namespace SpaceInvaders.Gameplay.UI {

    // simple and stupid all-in-one HUD
    public class GameplayPauseScreen : BaseGameplayBehaviour {

        [SerializeField]
        private Button _btnUnpause;

        [SerializeField]
        private Button _btnRestart;

        private void Awake() {
            _btnUnpause.onClick.AddListener(ProcessUnpauseClick);
            _btnRestart.onClick.AddListener(ProcessRestartClick);
        }

        private void ProcessUnpauseClick() {
            _gameplayCommand.Execute(EGameplayCommand.Unpause);
        }

        private void ProcessRestartClick() {
            _gameplayCommand.Execute(EGameplayCommand.Restart);
        }
    }
}