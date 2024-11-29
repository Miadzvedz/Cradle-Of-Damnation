using UnityEngine;

namespace Managers
{
    public abstract class BaseManager<T> : MonoBehaviour
        where T : BaseManager<T>
    {
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance != null)
                Destroy(Instance.gameObject);
            else 
                Instance = (T)this;
        }
    }
}