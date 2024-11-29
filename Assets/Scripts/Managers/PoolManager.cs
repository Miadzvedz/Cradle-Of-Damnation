using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Pool.ItemsPool;
using Pool;
using Assets.Scripts.Managers;

namespace Managers
{
    public class PoolManager : BaseManager<PoolManager>
    {
        [SerializeField] private List<PoolItem> poolableObjects = new List<PoolItem>();

        private Dictionary<int, GameObjectPool> poolDictionary = new Dictionary<int, GameObjectPool>();


        protected override void Awake()
        {
            base.Awake();

            if (poolableObjects.Any())
            {
                poolableObjects.ForEach(obj => CreatePool(obj.Prefab, obj.PoolCount));
            }
        }

        public void CreatePool(GameObject prefab, int poolSize)
        {
            int poolKey = prefab.GetInstanceID();
            Transform container = GetContainer(prefab.name);

            if (!poolDictionary.ContainsKey(poolKey))
            {          
                prefab.GetOrAddComponent<PooledObject>().PoolId = poolKey;
                poolDictionary.Add(poolKey, new GameObjectPool(prefab, container, poolSize));
            }
        }

        public void GetFromPool(GameObject prefab, Vector2 position, Quaternion rotation)
        {
            int poolKey = prefab.GetInstanceID();

            if (poolDictionary.TryGetValue(poolKey, out GameObjectPool pool))
            {
                GameObject obj = pool.Get();
                obj.transform.SetPositionAndRotation(position, rotation);
            }
        }

        public T GetFromPool<T>(GameObject prefab, Vector2 position, Quaternion rotation)
        {
            int poolKey = prefab.GetInstanceID();

            if (!poolDictionary.TryGetValue(poolKey, out GameObjectPool pool)) return default;
          
            GameObject obj = pool.Get();
            obj.transform.SetPositionAndRotation(position, rotation);

            return obj.GetComponent<T>();
        }

        public void ReturnToPool(GameObject prefab)
        {
            int poolKey = prefab.GetComponent<PooledObject>().PoolId;

            if (poolDictionary.ContainsKey(poolKey))
            {
                poolDictionary[poolKey].Release(prefab);
            }
        }

        private Transform GetContainer(string name)
        {
            GameObject poolHolder = new GameObject($"Container [{name}]");
            poolHolder.transform.parent = transform;
            return poolHolder.transform;
        }
    }
}