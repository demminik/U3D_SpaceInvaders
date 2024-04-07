using SpaceInvaders.Gameplay.Boosters;
using System;
using UnityEngine;

namespace SpaceInvaders.Configs {

    [CreateAssetMenu(fileName = "BoostersConfig", menuName = "Configs/BoostersConfig")]
    public class BoostersConfig : ScriptableObject {

        [Serializable]
        private struct BoosterConfig {

            public Booster Prefab;
        }

        // TODO: add vaildation (not important for demo)
        [SerializeField]
        private BoosterConfig[] _boosters;

        public bool TryGetBoosterPrefab(EBoosterType type, out Booster result) {
            // TODO: this shoud be cached (not important for demo)
            foreach (var item in _boosters) {
                if (item.Prefab != null && item.Prefab.Type == type) {
                    result = item.Prefab;
                    return true;
                }
            }

            result = null;
            return false;
        }
    }
}