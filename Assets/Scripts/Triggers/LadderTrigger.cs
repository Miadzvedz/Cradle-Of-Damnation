using Extensions;
using System;
using UnityEngine;


namespace Triggers
{
    public class LadderTrigger : MonoBehaviour
    {
        [SerializeField] private LayerMask targetLayer;
        
        public event Action OnEnter;
        public event Action OnStay;
        public event Action OnExit;


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (targetLayer.IsMatch(collision.transform.gameObject.layer))
            {
                OnEnter?.Invoke();
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            OnStay?.Invoke();
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (targetLayer.IsMatch(collision.transform.gameObject.layer))
            {
                OnExit?.Invoke();
            }
        }
    }
}