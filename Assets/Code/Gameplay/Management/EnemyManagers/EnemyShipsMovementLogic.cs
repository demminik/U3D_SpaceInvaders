using SpaceInvaders.Gameplay.Common;
using SpaceInvaders.Utils;
using System;
using UnityEngine;

namespace SpaceInvaders.Gameplay {

    public class EnemyShipsMovementLogic : BaseGameplayBehaviour {

        [Serializable]
        private struct WaveMovementSettings {

            [Serializable]
            public struct HorizontalSettings {

                public float MaxOffset;
                public float Speed;
                public float StartDirection;
            }

            [Serializable]
            public struct VerticalSettings {

                public float AdvanceTimeInterval;
                public float AdvanceDistance;
                public float AdvanceDuration;
                public AnimationCurve AdvanceProgressCurve;
            }

            public HorizontalSettings Horizontal;
            public VerticalSettings Vertical;
        }

        private struct RuntimeWaveData {

            public float StartHorizontalPosition;
            public float HorizontalDirection;

            public Axis CurrentAdvanceAxis;
            public float CurrentAdvanceCooldown;
            public float AdvanceCooldown;
        }

        [SerializeField]
        private Transform _enemyShipsRoot;

        [SerializeField]
        private WaveMovementSettings _waveMovementSettings;

        private float CurrentHorizontalPosition {
            get => _enemyShipsRoot.transform.position.x;
            set {
                var position = _enemyShipsRoot.transform.position;
                position.x = value;
                _enemyShipsRoot.transform.position = position;
            }
        }
        private float CurrentVerticalPosition {
            get => _enemyShipsRoot.transform.position.y;
            set {
                var position = _enemyShipsRoot.transform.position;
                position.y = value;
                _enemyShipsRoot.transform.position = position;
            }
        }

        private RuntimeWaveData _runtimeData;

        private void Awake() {
            _runtimeData.StartHorizontalPosition = CurrentHorizontalPosition;
            _runtimeData.HorizontalDirection = 1f * Mathf.Sign(_waveMovementSettings.Horizontal.StartDirection);
        }

        protected override void ProcessGameplayCommandInternal(EGameplayCommand command) {
            switch (command) {
                case EGameplayCommand.Restart:
                case EGameplayCommand.NextWave:
                case EGameplayCommand.SpawnEnemies:
                case EGameplayCommand.DespawnEnemies:
                case EGameplayCommand.EnemiesEmerged:
                    ResetRuntimeData();
                    break;
            }
        }

        protected override void ProcessGameplayStateChangedInternal(EGameplayState state) {
            if (state == EGameplayState.Playing) {
                UniRXHelper.SubscribeToUpdate(Tick, ref _updateDisposable);
            } else {
                UniRXHelper.UnsubscribeFromUpdate(ref _updateDisposable);
            }
        }

        private void Tick(long _) {
            TickHorizontalMovement();
            TickAdvance();
        }

        private void TickHorizontalMovement() {
            // don't perform horizontal movement during vertical
            if(_runtimeData.CurrentAdvanceCooldown > 0f) {
                return;
            }
            var targetPosition = CurrentHorizontalPosition + _waveMovementSettings.Horizontal.Speed * Time.deltaTime * _runtimeData.HorizontalDirection;
            var currentOffset = Mathf.Abs(targetPosition - _runtimeData.StartHorizontalPosition);
            if (currentOffset >= _waveMovementSettings.Horizontal.MaxOffset) {
                CurrentHorizontalPosition = _runtimeData.StartHorizontalPosition + _waveMovementSettings.Horizontal.MaxOffset * _runtimeData.HorizontalDirection;
                _runtimeData.HorizontalDirection *= -1f;
            } else {
                CurrentHorizontalPosition = targetPosition;
            }
        }

        private void TickAdvance() {
            // block incorrect settings
            if (_waveMovementSettings.Vertical.AdvanceTimeInterval <= 0f) {
                return;
            }

            // tick current advance
            if (_runtimeData.CurrentAdvanceCooldown > 0f) {
                _runtimeData.CurrentAdvanceCooldown = Mathf.Max(0f, _runtimeData.CurrentAdvanceCooldown - Time.deltaTime);
                var advanceRatio = 1f - _runtimeData.CurrentAdvanceCooldown / _waveMovementSettings.Vertical.AdvanceDuration;
                var advanceRatioEvaluated = _waveMovementSettings.Vertical.AdvanceProgressCurve.Evaluate(advanceRatio);

                var targetPosition = _runtimeData.CurrentAdvanceAxis.Min + ((_runtimeData.CurrentAdvanceAxis.Max - _runtimeData.CurrentAdvanceAxis.Min) * advanceRatioEvaluated);
                CurrentVerticalPosition = targetPosition;
            }

            _runtimeData.AdvanceCooldown -= Time.deltaTime;

            if (_runtimeData.AdvanceCooldown <= 0f) {
                _runtimeData.AdvanceCooldown += _waveMovementSettings.Vertical.AdvanceTimeInterval;

                // start advance
                _runtimeData.CurrentAdvanceCooldown = _waveMovementSettings.Vertical.AdvanceDuration;
                _runtimeData.CurrentAdvanceAxis = new Axis(CurrentVerticalPosition, CurrentVerticalPosition + _waveMovementSettings.Vertical.AdvanceDistance);
            }
        }

        private void ResetRuntimeData() {
            _runtimeData.AdvanceCooldown = _waveMovementSettings.Vertical.AdvanceTimeInterval;
            _runtimeData.CurrentAdvanceCooldown = 0f;
        }
    }
}