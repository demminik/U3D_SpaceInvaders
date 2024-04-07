using SpaceInvaders.Gameplay.Pooling;

namespace SpaceInvaders.Gameplay.Spawners {

    public abstract class GenericSpawner<TConfig, TEntityType, TResult>
        where TEntityType : System.Enum
        where TResult : UnityEngine.Component {

        protected TConfig _config;

        public GenericSpawner(TConfig config) {
            _config = config;
        }

        public bool Spawn(TEntityType type, out TResult result) {
            if (!CanBeSpawned(type)) {
                result = null;
                return false;
            }

            if (!TryGetPrefabFromConfig(type, out var prefab)) {
                UnityEngine.Debug.LogError($"Failed to spawn {type}: no prefab found");
                result = null;
                return false;
            }

            var go = GameObjectsPool.Instance.Get(prefab.gameObject);
            result = go.GetComponent<TResult>();
            SetupSpawnedInstance(type, result);
            return result != null;
        }

        public bool Despawn(TResult instance) {
            if (instance != null) {
                return GameObjectsPool.Instance.Release(instance.gameObject);
            }
            return false;
        }

        protected virtual bool CanBeSpawned(TEntityType type) {
            return true;
        }

        protected abstract bool TryGetPrefabFromConfig(TEntityType type, out TResult result);

        protected virtual void SetupSpawnedInstance(TEntityType type, TResult instance) { }
    }
}