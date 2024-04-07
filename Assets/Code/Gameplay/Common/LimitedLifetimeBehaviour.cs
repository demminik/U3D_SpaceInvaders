using SpaceInvaders.Utils;
using System;
using UnityEngine;

namespace SpaceInvaders.Gameplay.Common {

    public class LimitedLifetimeBehaviour : BaseGameplayBehaviour {

        [SerializeField]
        private float _lifetime;

        private float _endLifeTime;

        public event Action OnLifetimeEnded;

        public void Launch() {
            _endLifeTime = _lifetime > 0 ? Time.time + _lifetime : float.MaxValue;
            UniRXHelper.SubscribeToUpdate(TickLifetime, ref _updateDisposable);
        }

        public void Stop() {
            UniRXHelper.UnsubscribeFromUpdate(ref _updateDisposable);
        }

        private void TickLifetime(long _) {
            if (Time.time > _endLifeTime) {
                OnLifetimeEnded?.Invoke();
            }
        }
    }
}