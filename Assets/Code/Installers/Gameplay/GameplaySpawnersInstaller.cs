using SpaceInvaders.Configs;
using SpaceInvaders.Gameplay.Spawners;
using UnityEngine;
using Zenject;

namespace SpaceInvaders.Gameplay.Installers {

    public class GameplaySpawnersInstaller : MonoInstaller {

        [SerializeField]
        private ShipsConfig _shipsConfig;

        [SerializeField]
        private WeaponsConfig _weaponsConfig;

        [SerializeField]
        private BoostersConfig _boostersConfig;

        public override void InstallBindings() {
            Container.Bind<ShipSpawner>().FromInstance(new ShipSpawner(_shipsConfig)).AsSingle();
            Container.Bind<WeaponSpawner>().FromInstance(new WeaponSpawner(_weaponsConfig)).AsSingle();
            Container.Bind<ProjectileSpawner>().FromInstance(new ProjectileSpawner(_weaponsConfig)).AsSingle();
            Container.Bind<BoosterSpawner>().FromInstance(new BoosterSpawner(_boostersConfig)).AsSingle();
        }
    }
}