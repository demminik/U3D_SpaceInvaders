using SpaceInvaders.Gameplay;
using UniRx;
using Zenject;

namespace SpaceInvaders.Gameplay.Installers {

    public class UniRXGameplayInstaller : MonoInstaller {

        public override void InstallBindings() {
            // simple implementation of global events
            Container.Bind<ReactiveCommand<EGameplayCommand>>().FromInstance(new ReactiveCommand<EGameplayCommand>()).AsSingle();
            Container.Bind<ReactiveProperty<EGameplayState>>().FromInstance(new ReactiveProperty<EGameplayState>()).AsSingle();
        }
    }
}