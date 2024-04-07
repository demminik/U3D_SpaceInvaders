using SpaceInvaders.Gameplay.Accessors;
using Zenject;

namespace SpaceInvaders.Gameplay.Creators {

    public class EnemyShipsEmerger : AbstractShipEmerger {

        private EnemyShipsAccessor _enemyShipsAccessor;

        [Inject]
        private void HandleInjection(EnemyShipsAccessor enemyShipsAccessor) {
            _enemyShipsAccessor = enemyShipsAccessor;
        }

        protected override void ProcessGameplayCommandInternal(EGameplayCommand command) {
            switch (command) {
                case EGameplayCommand.EnemiesSpawned:
                    StartEmerge();
                    break;
                default:
                    break;
            }
        }

        protected override void EndEmerge() {
            base.EndEmerge();

            _gameplayCommand.Execute(EGameplayCommand.EnemiesEmerged);
        }
    }
}