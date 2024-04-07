using SpaceInvaders.Gameplay.Common;
using SpaceInvaders.Gameplay.Level;
using UnityEngine;
using Zenject;

namespace SpaceInvaders.Gameplay.Installers {

    public class WorldStuffInstaller : MonoInstaller {

        [SerializeField]
        private Camera _mainCamera;

        public override void InstallBindings() {
            var boundMin = _mainCamera.ScreenToWorldPoint(Vector3.zero);
            var boundMax = _mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0f));

            var worldBounds = new WorldBorders(new Axis(boundMin.x, boundMax.x), new Axis(boundMin.z, boundMax.z));

            Container.Bind<WorldBorders>().FromInstance(worldBounds).AsSingle();
        }
    }
}