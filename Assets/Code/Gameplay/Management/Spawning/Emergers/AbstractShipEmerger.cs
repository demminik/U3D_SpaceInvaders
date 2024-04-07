using SpaceInvaders.Utils;
using System;
using UnityEngine;

namespace SpaceInvaders.Gameplay.Creators {

    public abstract class AbstractShipEmerger : BaseGameplayBehaviour {

        [Serializable]
        public struct EmergeSettings {

            public Transform EmergeableRoot;
            public Vector3 StartOffset;
            public AnimationCurve Curve;

            public float Duration => Curve.length == 0 ? 0f : Curve.keys[Curve.length - 1].time;
        }

        [SerializeField]
        protected EmergeSettings _emergeSettings;

        protected Vector3 _emergeableRootStartPosition;
        private float _emergeTimer = float.MaxValue;

        protected virtual void Awake() {
            if (_emergeSettings.EmergeableRoot != null) {
                _emergeableRootStartPosition = _emergeSettings.EmergeableRoot.transform.position;
            }
        }

        protected virtual void StartEmerge() {
            if (_emergeSettings.EmergeableRoot != null) {
                _emergeTimer = 0f;
                UpdateEmergePosition(_emergeSettings.EmergeableRoot, _emergeTimer);

                UniRXHelper.SubscribeToUpdate(TickEmerge, ref _updateDisposable);
            } else {
                EndEmerge();
            }
        }

        protected virtual void EndEmerge() {
            UniRXHelper.UnsubscribeFromUpdate(ref _updateDisposable);

            _emergeTimer = float.MaxValue;
            UpdateEmergePosition(_emergeSettings.EmergeableRoot, _emergeTimer);
        }

        protected virtual void TickEmerge(long _) {
            _emergeTimer += Time.deltaTime;
            if (_emergeTimer < _emergeSettings.Duration) {
                UpdateEmergePosition(_emergeSettings.EmergeableRoot, _emergeTimer);
            } else {
                EndEmerge();
            }
        }

        protected void UpdateEmergePosition(Transform transform, float time) {
            if (transform != null) {
                var validatedTime = Mathf.Min(_emergeSettings.Duration, _emergeTimer);
                var offsetMultiplier = _emergeSettings.Curve.Evaluate(validatedTime);
                transform.transform.position = _emergeableRootStartPosition + _emergeSettings.StartOffset * offsetMultiplier;
            }
        }
    }
}