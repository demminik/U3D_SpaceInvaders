using SpaceInvaders.Gameplay.Input;
using SpaceInvaders.Utils;
using UnityEngine;
using Zenject;

namespace SpaceInvaders.Gameplay.Installers {

    public class GameplayInputInstaller : MonoInstaller {

        [SerializeField]
        private GIP_Touch _touchInputPrefab;

        [SerializeField]
        private GIP_Keyboard _keyboardInputPrefab;

        public override void InstallBindings() {
            // very basic platform detection
            if (PlatformUtils.CurrentPlatformType == PlatformUtils.EPlatformType.Mobile) {
                Container.Bind<IGameplayPlayerInputProvider>().To<GIP_Touch>().FromComponentInNewPrefab(_touchInputPrefab).AsSingle();
            } else {
                Container.Bind<IGameplayPlayerInputProvider>().To<GIP_Keyboard>().FromComponentInNewPrefab(_keyboardInputPrefab).AsSingle();
            }
        }
    }
}