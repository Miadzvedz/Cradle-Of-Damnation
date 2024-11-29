using Managers;
using UnityEngine;

namespace Pool.ItemsPool
{
    public class PooledObject : MonoBehaviour
    {
        [field: SerializeField]
        public int PoolId {  get; set; }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void ReturnToPool()
        {
            if(gameObject.activeSelf) 
            {
                PoolManager.Instance.ReturnToPool(gameObject);
            }
        }

        public virtual void Get(GameObject obj)
        {
            obj.SetActive(true);
        }

        public virtual void Release(GameObject obj)
        {
            obj.SetActive(false);
        }

        public virtual GameObject Create(Transform container)
        {
            return GameObject.Instantiate(gameObject, container);
        }

        public virtual void Destroy(GameObject obj)
        {
            GameObject.Destroy(obj);
        }
    }
}
