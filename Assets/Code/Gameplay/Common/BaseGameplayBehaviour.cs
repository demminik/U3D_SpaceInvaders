using SpaceInvaders.Utils;
using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace SpaceInvaders.Gameplay {

    public abstract class BaseGameplayBehaviour : MonoBehaviour {

        public bool IsBehaviourActive { get; protected set; }

        protected ReactiveProperty<EGameplayState> _gameplayState;
        protected ReactiveCommand<EGameplayCommand> _gameplayCommand;

        protected IDisposable _updateDisposable;

        protected CompositeDisposable _disposable = new CompositeDisposable();

        [Inject]
        private void HandleInjectionBase(ReactiveProperty<EGameplayState> gameplayState,
            ReactiveCommand<EGameplayCommand> gameplayCommand) {

            _gameplayState = gameplayState;
            // TODO: probably not each behaviour need this subscription (not important for demo)
            _gameplayState.Subscribe(ProcessGameplayStateChanged).AddTo(_disposable);

            _gameplayCommand = gameplayCommand;
            // TODO: probably not each behaviour need this subscription (not important for demo)
            _gameplayCommand.Subscribe(ProcessGameplayCommand).AddTo(_disposable);
        }

        protected virtual void OnDestroy() {
            UniRXHelper.UnsubscribeFromUpdate(ref _updateDisposable);

            if (!_disposable.IsDisposed) {
                _disposable.Dispose();
            }
        }

        private void ProcessGameplayStateChanged(EGameplayState state) {
            ActualizeBehaviourAciveState(state);
            ProcessGameplayStateChangedInternal(state);
        }

        protected virtual void ProcessGameplayStateChangedInternal(EGameplayState state) {
        }

        private void ProcessGameplayCommand(EGameplayCommand command) {
            ProcessGameplayCommandInternal(command);
        }

        protected virtual void ProcessGameplayCommandInternal(EGameplayCommand command) {
        }

        protected virtual void ActualizeBehaviourAciveState(EGameplayState state) {
            IsBehaviourActive = state == EGameplayState.Playing;
        }
    }
}