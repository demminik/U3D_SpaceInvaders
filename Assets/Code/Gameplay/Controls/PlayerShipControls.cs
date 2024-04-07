using SpaceInvaders.Gameplay.Accessors;
using SpaceInvaders.Gameplay.Common;
using SpaceInvaders.Gameplay.Input;
using SpaceInvaders.Gameplay.Level;
using SpaceInvaders.Utils;
using UnityEngine;
using Zenject;

namespace SpaceInvaders.Gameplay.Controls {

    /// <summary>
    /// Very basic movement script
    /// </summary>
    public class PlayerShipControls : BaseGameplayBehaviour {

        // TODO: this should be in level config (not important for demo)
        [Tooltip("Max distance player ship can move vertically from start position")]
        [SerializeField]
        private float _maxVerticalOffset = 2f;

        [SerializeField]
        private Vector2 _movementSpeed;

        [SerializeField]
        private float _worldBordersHorizontalOverride = 5f;

        //[Tooltip("Reduce or extend horizonal border restrictions")]
        //[SerializeField]
        //private float _worldBordersOffset = 1f;

        private IGameplayPlayerInputProvider _inputProvider;
        private PlayerShipAccessor _playerShipAccessor;
        private WorldBorders _worldBorders;

        private Vector2 _currentInput;

        private bool _isSpawned = false;

        [Inject]
        private void HandleInjection(IGameplayPlayerInputProvider inputProvider,
            PlayerShipAccessor playerShipAccessor,
            WorldBorders worldBorders) {

            _inputProvider = inputProvider;
            _inputProvider.OnPlayerAction += ProcessInputReceived;

            _playerShipAccessor = playerShipAccessor;

            _worldBorders = worldBorders;
        }

        private void Awake() {
            ActualizeUpdateSubscription();
        }

        private void Start() {
            var horizAdjusted = new Axis(-_worldBordersHorizontalOverride, _worldBordersHorizontalOverride);
            _worldBorders.Horizontal = horizAdjusted;
            var vertAdjusted = new Axis(0f, _maxVerticalOffset);
            _worldBorders.Vertical = vertAdjusted;
        }

        protected override void ProcessGameplayCommandInternal(EGameplayCommand command) {
            switch (command) {
                case EGameplayCommand.Restart:
                case EGameplayCommand.DespawnPlayer:
                case EGameplayCommand.SpawnPlayer:
                    _isSpawned = false;
                    ActualizeUpdateSubscription();
                    break;
                case EGameplayCommand.PlayerEmerged:
                    _isSpawned = _playerShipAccessor.PlayerShip != null && _playerShipAccessor.PlayerShip.gameObject == gameObject;
                    ActualizeUpdateSubscription();
                    break;
            }
        }

        protected override void ProcessGameplayStateChangedInternal(EGameplayState state) {
            ActualizeUpdateSubscription();
        }

        private void ActualizeUpdateSubscription() {
            if ((_gameplayState.Value == EGameplayState.NewWavePrepartion || _gameplayState.Value == EGameplayState.Playing) &&
                _isSpawned) {

                UniRXHelper.SubscribeToUpdate(Tick, ref _updateDisposable);
            } else {
                UniRXHelper.UnsubscribeFromUpdate(ref _updateDisposable);
            }
        }

        private void Tick(long _) {
            var deltaTime = Time.deltaTime;

            TickHorizontalMovement(deltaTime);
            TickVerticalMovement(deltaTime);

            _currentInput = Vector2.zero;
        }

        private void ProcessInputReceived(PlayerActionData actionData) {
            if (actionData.Action == EPlayerAction.MoveHorizontal) {
                _currentInput.x = actionData.Value;
            }
            if (actionData.Action == EPlayerAction.MoveVertical) {
                _currentInput.y = actionData.Value;
            }
        }

        private void TickHorizontalMovement(float deltaTime) {
            var newPosition = transform.localPosition + new Vector3(_currentInput.x * _movementSpeed.x * deltaTime, 0f, 0f);
            newPosition.x = Mathf.Clamp(newPosition.x, _worldBorders.Horizontal.Min, _worldBorders.Horizontal.Max);
            transform.localPosition = newPosition;
        }

        private void TickVerticalMovement(float deltaTime) {
            var newPosition = transform.localPosition + new Vector3(0f, _currentInput.y * _movementSpeed.y * deltaTime, 0f);
            newPosition.y = Mathf.Clamp(newPosition.y, _worldBorders.Vertical.Min, _worldBorders.Vertical.Max);
            transform.localPosition = newPosition;
        }
    }
}