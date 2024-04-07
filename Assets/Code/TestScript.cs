using SpaceInvaders.Gameplay;
using SpaceInvaders.Gameplay.Input;
using UniRx;
using UnityEngine;
using Zenject;

// TODO: script for dev-in-progress test purposes, need to cleanup
public class TestScript : MonoBehaviour
{
    private IGameplayPlayerInputProvider _inputProvider;

    private ReactiveCommand<EGameplayCommand> _gameplayCommand;

    [Inject]
    private void HandleInjection(IGameplayPlayerInputProvider inputProvider,
        ReactiveCommand<EGameplayCommand> gameplayCommand) {

        _inputProvider = inputProvider;
        _inputProvider.OnPlayerAction += OnAction;

        _gameplayCommand = gameplayCommand;
        //_gameplayCommand.Subscribe(OnGameplayCommand);
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.DownArrow)) {
            _gameplayCommand.Execute(EGameplayCommand.Restart);
        }
    }

    private void OnGameplayCommand(EGameplayCommand command) {
    }

    private void OnAction(PlayerActionData actionData) {
    }
}
