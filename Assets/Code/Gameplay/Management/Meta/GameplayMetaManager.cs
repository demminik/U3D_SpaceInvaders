using SpaceInvaders.Gameplay.Meta;
using Zenject;

namespace SpaceInvaders.Gameplay {

    // TODO: add global manager for stats reset
    public class GameplayMetaManager : BaseGameplayBehaviour {

        private GameplayStats _stats;

        [Inject]
        private void HandleInjection(GameplayStats stats) {
            _stats = stats;
        }

        protected override void ProcessGameplayCommandInternal(EGameplayCommand command) {
            switch (command) {
                case EGameplayCommand.Restart:
                    _stats.Reset();
                    break;
            }
        }

        protected override void ProcessGameplayStateChangedInternal(EGameplayState state) {
            switch(state) {
                case EGameplayState.NewWavePrepartion:
                    _stats.WaveNumber.Value++;
                    break;
            }
        }
    }
}