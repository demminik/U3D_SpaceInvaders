using SpaceInvaders.Gameplay.Accessors;
using SpaceInvaders.Gameplay.Boosters;
using SpaceInvaders.Gameplay.Entities;
using SpaceInvaders.Gameplay.Spawners;
using System;
using UnityEngine;
using Zenject;

namespace SpaceInvaders.Gameplay {

    public class EnemyLootDropLogic : BaseGameplayBehaviour {

        // TODO: this sould be in enemy or level config (not important for demo)
        [Serializable]
        private struct BoosterDropSettings {

            [Range(0f, 1f)]
            [SerializeField]
            public float BoosterDropChance;

            [SerializeField]
            public EBoosterType[] AvailableBoosters;
        }

        [SerializeField]
        private BoosterDropSettings _boosterDropSettings;

        private EnemyShipsAccessor _enemyShipsAccessor;
        private BoosterSpawner _boostersSpawner;

        [Inject]
        private void HandleInjection(BoosterSpawner boostersSpawner,
            EnemyShipsAccessor enemyShipsAccessor) {

            _boostersSpawner = boostersSpawner;

            _enemyShipsAccessor = enemyShipsAccessor;
            _enemyShipsAccessor.OnShipDestroyed += OnEnemyDeath;
        }

        protected override void OnDestroy() {
            base.OnDestroy();

            if (_enemyShipsAccessor != null) {
                _enemyShipsAccessor.OnShipDestroyed -= OnEnemyDeath;
                _enemyShipsAccessor = null;
            }
        }

        private void OnEnemyDeath(Ship ship) {
            if (UnityEngine.Random.Range(0f, 1f) <= _boosterDropSettings.BoosterDropChance) {
                DropBooster(ship.DropRoot != null ? ship.DropRoot : ship.transform);
            }
        }

        private void DropBooster(Transform startTransform) {
            if (_boosterDropSettings.AvailableBoosters == null ||
                _boosterDropSettings.AvailableBoosters.Length == 0) {

                return;
            }

            var boosterDropType = _boosterDropSettings.AvailableBoosters[UnityEngine.Random.Range(0, _boosterDropSettings.AvailableBoosters.Length)];
            if (!_boostersSpawner.Spawn(boosterDropType, out var booster)) {
                UnityEngine.Debug.LogError($"Booster drop failed: no booster found for {boosterDropType} booster type");
                return;
            }

            booster.transform.SetPositionAndRotation(startTransform.position, startTransform.rotation);
            booster.Launch();
        }
    }
}