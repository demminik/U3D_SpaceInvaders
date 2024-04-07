using SpaceInvaders.Gameplay.Entities;
using SpaceInvaders.Ships;
using System;
using UnityEngine;

namespace SpaceInvaders.Configs {

    [CreateAssetMenu(fileName = "ShipsConfig", menuName = "Configs/ShipsConfig")]
    public class ShipsConfig : ScriptableObject {

        [Serializable]
        private struct ShipConfig {

            public Ship Prefab;
        }

        [Serializable]
        public struct EnemyRow {

            public EShipType ShipType;
            public int RowSize;
        }

        // TODO: add vaildation (not important for demo)
        [SerializeField]
        private ShipConfig[] _ships;

        // TODO: this shold not be in here (not important for demo)
        [SerializeField]
        private EShipType _playerShipType;
        public EShipType PlayerShipType => _playerShipType;

        // TODO: this shold not be in here (not important for demo)
        [SerializeField]
        private EnemyRow[] _enemyShipRows;
        public EnemyRow[] EnemyShipRows => _enemyShipRows;

        public bool TryGetShipPrefab(EShipType type, out Ship result) {
            // TODO: this shoud be cached (not important for demo)
            foreach (var item in _ships) {
                if(item.Prefab != null && item.Prefab.Type == type) {
                    result = item.Prefab;
                    return true;
                }
            }

            result = null;
            return false;
        }
    }
}