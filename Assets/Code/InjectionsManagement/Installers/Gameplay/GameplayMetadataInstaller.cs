using SpaceInvaders.Gameplay.Meta;
using Zenject;

namespace SpaceInvaders.Gameplay.Installers {

    public class GameplayMetadataInstaller : MonoInstaller {

        public override void InstallBindings() {
            Container.Bind<GameplayStats>().FromInstance(new GameplayStats()).AsSingle();
        }
    }
}