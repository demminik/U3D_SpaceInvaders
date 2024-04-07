using SpaceInvaders.Gameplay.Accessors;
using SpaceInvaders.Gameplay.Utils;
using UnityEngine;
using Zenject;

namespace SpaceInvaders.Gameplay {

    public class LoseTriggerBehaviour : BaseGameplayBehaviour {

        private PlayerShipAccessor _playerShipAccessor;

        [Inject]
        private void HandleInjection(PlayerShipAccessor playerShipAccessor) {
            _playerShipAccessor = playerShipAccessor;
        }

        private void OnTriggerEnter(Collider other) {
            if(_playerShipAccessor.PlayerShip == null) {
                return;
            }

            var goOther = other.gameObject;

            var hitObjectTeamRelations = TeamUtils.GetTeamRelationType(_playerShipAccessor.PlayerShip.gameObject, goOther);
            if (hitObjectTeamRelations == ETeamRelationType.Enemy) {
                _gameplayCommand.Execute(EGameplayCommand.TotalLoss);
            }
        }
    }
}