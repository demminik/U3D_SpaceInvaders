using UniRx;

namespace SpaceInvaders.Gameplay.Meta {

    // very lazy all-in-one class
    public class GameplayStats {

        public const int PLAYER_LIVES = 3;

        public ReactiveProperty<int> WaveNumber { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<int> Score { get; } = new ReactiveProperty<int>();
        public ReactiveProperty<int> PlayerLives { get; } = new ReactiveProperty<int>();

        public void Reset() {
            WaveNumber.Value = 0;
            Score.Value = 0;
            PlayerLives.Value = PLAYER_LIVES;
        }
    }
}