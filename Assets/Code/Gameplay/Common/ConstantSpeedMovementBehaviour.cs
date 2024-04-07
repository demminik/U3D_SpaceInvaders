using SpaceInvaders.Utils;
using UnityEngine;

namespace SpaceInvaders.Gameplay.Common {

    // TODO: this behaviour is similar to Projectile, need to extract ConstantSpeedMovementBehaviour and LifetimeBehaviour (lazy)
    public class ConstantSpeedMovementBehaviour : BaseGameplayBehaviour {

        [SerializeField]
        private float _movementSpeed = 1f;

        public void Launch() {
            UniRXHelper.SubscribeToUpdate(TickMovement, ref _updateDisposable);
        }

        public void Stop() {
            UniRXHelper.UnsubscribeFromUpdate(ref _updateDisposable);
        }

        protected override void ActualizeBehaviourAciveState(EGameplayState state) {
            IsBehaviourActive = state != EGameplayState.Paused;
        }

        private void TickMovement(long _) {
            if (IsBehaviourActive) {
                transform.position += transform.forward * _movementSpeed * Time.deltaTime;
            }
        }
    }
}