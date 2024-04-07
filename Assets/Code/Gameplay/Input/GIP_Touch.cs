using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SpaceInvaders.Gameplay.Input {

    /// <summary>
    /// Very lazy player input for touch
    /// </summary>
    public class GIP_Touch : BaseGameplayBehaviour, IGameplayPlayerInputProvider {

        // TODO: this is lazy joystick implementation, nrmally it should be separate prefab (not important for demo)
        [SerializeField]
        private Canvas _joystickCanvas;

        [SerializeField]
        private RectTransform _joystickCanvasRectTransform;

        [SerializeField]
        private Image _joystickBg;

        [SerializeField]
        private Image _joystickFg;

        [SerializeField]
        private float _joystickMaxOffset = 10f;

        private bool _isInputActive;

        public event Action<PlayerActionData> OnPlayerAction;

        protected override void ActualizeBehaviourAciveState(EGameplayState state) {
            IsBehaviourActive = state == EGameplayState.Playing;
            if (!IsBehaviourActive) {
                DeactivateInput();
            }
        }

        private void Awake() {
            DeactivateInput();
        }

        private void Update() {
            if (Application.isEditor) {
                // mouse input for easier testing
                if (UnityEngine.Input.GetMouseButtonDown(0)) {
                    if (IsBehaviourActive && !EventSystem.current.IsPointerOverGameObject()) {
                        ActivateInput(new Vector2(UnityEngine.Input.mousePosition.x, UnityEngine.Input.mousePosition.y));
                    }
                } else if (UnityEngine.Input.GetMouseButtonUp(0)) {
                    DeactivateInput();
                } else if (UnityEngine.Input.GetMouseButton(0)) {
                    ProcessInput(new Vector2(UnityEngine.Input.mousePosition.x, UnityEngine.Input.mousePosition.y));
                }
            }

            if (UnityEngine.Input.touchCount > 0) {
                var firstTouch = UnityEngine.Input.touches[0];

                if (firstTouch.phase == TouchPhase.Began) {
                    if (IsBehaviourActive && !EventSystem.current.IsPointerOverGameObject(firstTouch.fingerId)) {
                        ActivateInput(firstTouch.position);
                    }
                } else if (firstTouch.phase == TouchPhase.Ended || firstTouch.phase == TouchPhase.Canceled) {
                    DeactivateInput();
                } else if (firstTouch.phase == TouchPhase.Moved || firstTouch.phase == TouchPhase.Stationary) {
                    ProcessInput(firstTouch.position);
                }
            }
        }

        private void ActivateInput(Vector2 inputPosition) {
            _isInputActive = true;

            _joystickCanvas.enabled = _isInputActive;

            SetImageToInputPosition(_joystickBg, inputPosition);
            SetImageToInputPosition(_joystickFg, inputPosition);
            RestrictJoystickFgPosition();

            ProcessInput(inputPosition);
        }

        private void DeactivateInput() {
            _isInputActive = false;
            _joystickCanvas.enabled = _isInputActive;
        }

        private void ProcessInput(Vector2 inputPosition) {
            if (_isInputActive) {
                SetImageToInputPosition(_joystickFg, inputPosition);
                RestrictJoystickFgPosition();
                DoInput();

            }
        }

        private void SetImageToInputPosition(Image img, Vector2 inputPosition) {
            img.rectTransform.position = inputPosition;
        }

        private void RestrictJoystickFgPosition() {
            if (_joystickFg.rectTransform.anchoredPosition.magnitude > _joystickMaxOffset) {
                _joystickFg.rectTransform.anchoredPosition = _joystickFg.rectTransform.anchoredPosition.normalized * _joystickMaxOffset;
            }
        }

        private void DoInput() {
            // lazy dead zone
            var deadZone = _joystickMaxOffset * 0.15f;

            var hasHorizontalInput = true;
            var horizontalInput = _joystickFg.rectTransform.anchoredPosition.x;
            if (horizontalInput > deadZone) {
                horizontalInput = 1f;
            } else if (horizontalInput < -deadZone) {
                horizontalInput = -1f;
            } else {
                hasHorizontalInput = false;
            }

            var hasVerticalInput = true;
            var verticalInput = _joystickFg.rectTransform.anchoredPosition.y;
            if (verticalInput > deadZone) {
                verticalInput = 1f;
            } else if (verticalInput < -deadZone) {
                verticalInput = -1f;
            } else {
                hasVerticalInput = false;
            }

            if (hasHorizontalInput) {
                OnPlayerAction?.Invoke(new PlayerActionData(EPlayerAction.MoveHorizontal, horizontalInput));
            }
            if (hasVerticalInput) {
                OnPlayerAction?.Invoke(new PlayerActionData(EPlayerAction.MoveVertical, verticalInput));
            }
        }
    }
}