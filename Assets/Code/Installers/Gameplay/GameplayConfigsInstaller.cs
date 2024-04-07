using SpaceInvaders.Configs;
using UnityEngine;
using Zenject;

namespace SpaceInvaders.Gameplay.Installers {

    public class GameplayConfigsInstaller : MonoInstaller {

        [SerializeField]
        private ShipsConfig _shipsConfig;

        [SerializeField]
        private WeaponsConfig _weaponConfig;

        [SerializeField]
        private BoostersConfig _boostersConfig;

        public override void InstallBindings() {
            Container.Bind<ShipsConfig>().FromScriptableObject(_shipsConfig).AsSingle();
            Container.Bind<WeaponsConfig>().FromScriptableObject(_weaponConfig).AsSingle();
            Container.Bind<BoostersConfig>().FromScriptableObject(_boostersConfig).AsSingle();
        }
    }
}