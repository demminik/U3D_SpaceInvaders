using SpaceInvaders.Gameplay.Accessors;
using Zenject;

namespace SpaceInvaders.Gameplay.Installers  {

    public class GameplayEntitiesInstaller : MonoInstaller {

        public override void InstallBindings() {
            Container.Bind<PlayerShipAccessor>().FromInstance(new PlayerShipAccessor()).AsSingle();
            Container.Bind<EnemyShipsAccessor>().FromInstance(new EnemyShipsAccessor()).AsSingle();
        }
    }
}