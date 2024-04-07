using SpaceInvaders.Gameplay.Entities;
using SpaceInvaders.Gameplay.Utils;
using SpaceInvaders.Gameplay.Weapons;
using SpaceInvaders.Utils;
using System;
using UnityEngine;

namespace SpaceInvaders.Gameplay.Combat {

    public class ShipAutoShootBehaviour : BaseGameplayBehaviour {

        [Serializable]
        private struct TargetDetectionSettings {

            public ETeamRelationType TeamRelations;
            public LayerMask LayerMask;
            public float MaxCastDistance;
            public float CastRadius;
        }

        [SerializeField]
        private Ship _ship;

        [SerializeField]
        private TargetDetectionSettings _targetDetectionSettings;

        private RaycastHit[] _raycastHits = new RaycastHit[10];

        private void Awake() {
            if (_ship != null) {
                ProcessShipWeaponChanged(_ship.CurrentWeapon);
                _ship.OnWeaponChanged += ProcessShipWeaponChanged;
            }
        }

        protected override void OnDestroy() {
            _raycastHits = null;

            if (_ship != null) {
                _ship.OnWeaponChanged -= ProcessShipWeaponChanged;
            }
        }

        protected override void ProcessGameplayStateChangedInternal(EGameplayState state) {
            ActualizeTickSubscription();
        }

        private void ActualizeTickSubscription() {
            if (IsBehaviourActive &&
                _ship != null &&
                _ship.CurrentWeapon != null &&
                _targetDetectionSettings.TeamRelations != ETeamRelationType.None) {

                UniRXHelper.SubscribeToUpdate(TickAutoShootingLogic, ref _updateDisposable);
            } else {
                UniRXHelper.UnsubscribeFromUpdate(ref _updateDisposable);
            }
        }

        private void ProcessShipWeaponChanged(Weapon weapon) {
            ActualizeTickSubscription();
        }

        private void TickAutoShootingLogic(long _) {
            if (_ship.CurrentWeapon.ReadyToShoot) {
                foreach (var raycastOrigin in _ship.CurrentWeapon.LaunchTransforms) {
                    var hitsAmount = Physics.SphereCastNonAlloc(raycastOrigin.position, _targetDetectionSettings.CastRadius, raycastOrigin.forward, _raycastHits, _targetDetectionSettings.MaxCastDistance, _targetDetectionSettings.LayerMask);
                    UnityEngine.Debug.DrawLine(raycastOrigin.position, raycastOrigin.position + raycastOrigin.forward * _targetDetectionSettings.MaxCastDistance, Color.green);
                    for (int i = 0; i < hitsAmount; i++) {
                        var hitObject = _raycastHits[i];
                        if (_targetDetectionSettings.TeamRelations.HasFlag(TeamUtils.GetTeamRelationType(hitObject.collider.gameObject, gameObject))) {
                            _ship.CurrentWeapon.Shoot();
                            return;
                        }
                    }
                }
            }
        }
    }
}