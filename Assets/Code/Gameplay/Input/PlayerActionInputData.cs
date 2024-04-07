namespace SpaceInvaders.Gameplay.Input {
    
    public enum EPlayerAction {

        None = 0,
        MoveHorizontal = 1,
        MoveVertical = 2,
    }

    public struct PlayerActionData {

        public EPlayerAction Action;
        public float Value;

        public PlayerActionData(EPlayerAction action, float value) {
            Action = action;
            Value = value;
        }
    }
}