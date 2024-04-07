using SpaceInvaders.Gameplay.Accessors;
using System;
using UnityEngine;
using Zenject;

namespace SpaceInvaders.Gameplay.Creators {

    public class PlayerShipEmerger : AbstractShipEmerger {

        private PlayerShipAccessor _playerShipAccessor;

        [Inject]
        private void HandleInjection(PlayerShipAccessor playerShipAccessor) {
            _playerShipAccessor = playerShipAccessor;
        }

        protected override void ProcessGameplayCommandInternal(EGameplayCommand command) {
            switch (command) {
                case EGameplayCommand.PlayerSpawned:
                    StartEmerge();
                    break;
                default:
                    break;
            }
        }

        protected override void EndEmerge() {
            base.EndEmerge();

            _gameplayCommand.Execute(EGameplayCommand.PlayerEmerged);
        }
    }
}