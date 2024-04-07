using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpaceInvaders.Gameplay.Pooling {

    // simple singleton that exist only in gameplay scene, no need any zenjects or other excessive stuff
    // very basic pool of game objects
    public class GameObjectsPool : MonoBehaviour {

        private static bool _isInstanceCreationAllowed = true;

        private static GameObjectsPool _instance;
        public static GameObjectsPool Instance {
            get {
                if(_instance == null ) {
                    AssignInstance(FindObjectOfType<GameObjectsPool>(true));
                    if( _instance != null ) {
                        // just make sure existing scene object is active
                        _instance.gameObject.SetActive(true);
                        _instance.enabled = true;
                    }
                }
                if( _instance == null && _isInstanceCreationAllowed) {
                    AssignInstance(new GameObject(typeof(GameObjectsPool).ToString()).AddComponent<GameObjectsPool>());
                }
                return _instance;
            }
        }

        private static void AssignInstance(GameObjectsPool value) {
            _instance = value;
            _hasInstance = _instance != null;
            if (_hasInstance) {
                DontDestroyOnLoad(_instance.gameObject);
            }
        }

        private static bool _hasInstance;
        public static bool HasInstance => _hasInstance;

        private Dictionary<GameObject, List<GameObject>> _pool = new Dictionary<GameObject, List<GameObject>>();
        private Dictionary<int, GameObject> _idToPrefabMap = new Dictionary<int, GameObject>();

        private void Awake() {
            if (_instance != null && _instance != this) {
                UnityEngine.Debug.LogError($"Found invalid GameObject for {typeof(GameObjectsPool)}... Destroying");
                Destroy(this);
                return;
            }
            AssignInstance(this);
        }

        private void OnDestroy() {
            if(_instance != null && _instance == this) {
                _instance = null;
                _hasInstance = false;
            }

            _pool.Clear();
            _idToPrefabMap.Clear();
        }

        private void OnApplicationQuit() {
            _isInstanceCreationAllowed = false;
            _hasInstance = false;
        }

        public GameObject Get(GameObject prefab) {
            if(prefab == null) {
                throw new NullReferenceException("Failed to get instance from pool: prefab is null");
            }

            GameObject instance = null;

            if (_pool.TryGetValue(prefab, out var pool) && pool.Count > 0) {
                instance = pool[0];
                pool.RemoveAt(0);
            } else {
                instance = GameObject.Instantiate(prefab);

                // important to it separately, otherwise injection borks
                instance.transform.parent = transform;
                instance.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            }

            _idToPrefabMap[instance.GetInstanceID()] = prefab;

            instance.SetActive(true);

            return instance;
        }

        public bool Release(GameObject instance) {
            if (instance == null) {
                return false;
            }

            var instanceId = instance.GetInstanceID();
            if (!_idToPrefabMap.TryGetValue(instanceId, out var prefab)) {
                UnityEngine.Debug.LogError($"Failed to release instance to pool: no suitable id found ({instanceId})", instance);
                return false;
            }
            _idToPrefabMap.Remove(instanceId);

            if (!_pool.TryGetValue(prefab, out var pool)) {
                pool = new List<GameObject>();
                _pool[prefab] = pool;
            }
            pool.Add(instance);

            instance.SetActive(false);

            return true;
        }

        public bool HasPooled(GameObject prefab) {
            if(_pool.TryGetValue(prefab, out var pool)) {
                return pool.Count > 0;
            }
            return false;
        }
    }
}