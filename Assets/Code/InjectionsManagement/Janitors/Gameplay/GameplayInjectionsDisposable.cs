using SpaceInvaders.Gameplay.Accessors;
using UnityEngine;
using Zenject;

namespace SpaceInvaders.Gameplay.Janitors {

    public class GameplayInjectionsDisposable : MonoBehaviour {

        private PlayerShipAccessor _playerShipAccessor;
        private EnemyShipsAccessor _enemyShipsAccessor;

        [Inject]
        private void HandleInjection(PlayerShipAccessor playerShipAccessor,
            EnemyShipsAccessor enemyShipsAccessor) {

            _playerShipAccessor = playerShipAccessor;

            _enemyShipsAccessor = enemyShipsAccessor;
        }

        private void OnDestroy() {
            _playerShipAccessor?.Dispose();
            _enemyShipsAccessor?.Dispose();
        }
    }
}