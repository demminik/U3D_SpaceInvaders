using SpaceInvaders.Gameplay.Entities;
using System;
using UnityEngine;

namespace SpaceInvaders.Gameplay.Combat {

    public class ShipCollisionBehaviour : BaseGameplayBehaviour {

        [Serializable]
        private struct OtherShipCollisionSettings {

            public LayerMask LayerMask;
        }

        [SerializeField]
        private Ship _ship;

        [SerializeField]
        private OtherShipCollisionSettings _otherShipCollisionSettings;

        private RaycastHit[] _raycastHits = new RaycastHit[10];

        private void OnCollisionEnter(Collision collision) {
            if (_otherShipCollisionSettings.LayerMask == (_otherShipCollisionSettings.LayerMask | (1 << collision.gameObject.layer))) {
                ProcessOtherShipCollision(collision);
            }
        }

        private void ProcessOtherShipCollision(Collision collision) {
            var goOther = collision.gameObject;

            var otherShip = goOther.GetComponent<Ship>();
            if(otherShip == null) {
                UnityEngine.Debug.LogError($"Object {goOther.name} should be a ship but it's not!", goOther);
                return;
            }

            var hitReceiver = GetComponent<IShipHitReceiver>();
            if (hitReceiver != null) {
                hitReceiver.ReceiveHit(otherShip);
            }
        }
    }
}