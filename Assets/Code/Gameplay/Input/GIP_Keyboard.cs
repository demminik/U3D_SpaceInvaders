using System;
using UnityEngine;

namespace SpaceInvaders.Gameplay.Input {

    /// <summary>
    /// Vaery lazy player input for keyboard
    /// </summary>
    public class GIP_Keyboard : MonoBehaviour, IGameplayPlayerInputProvider {

        public event Action<PlayerActionData> OnPlayerAction;

        private void Update() {
            if (OnPlayerAction != null) {
                if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.A)) {
                    OnPlayerAction(new PlayerActionData(EPlayerAction.MoveHorizontal, -1f));
                }
                if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.D)) {
                    OnPlayerAction(new PlayerActionData(EPlayerAction.MoveHorizontal, 1f));
                }
                if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.S)) {
                    OnPlayerAction(new PlayerActionData(EPlayerAction.MoveVertical, -1f));
                }
                if (UnityEngine.Input.GetKey(UnityEngine.KeyCode.W)) {
                    OnPlayerAction(new PlayerActionData(EPlayerAction.MoveVertical, 1f));
                }
            }
        }
    }
}