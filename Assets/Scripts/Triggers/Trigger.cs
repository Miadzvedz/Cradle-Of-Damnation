using Extensions;
using System;
using UnityEngine;


namespace Triggers
{
    public class Trigger : MonoBehaviour
    {
        [SerializeField] protected LayerMask targetLayer;
        
        public event Action OnEnter;
        public event Action OnStay;
        public event Action OnExit;


        protected void OnTriggerEnter2D(Collider2D collision)
        {
            if (targetLayer.IsMatch(collision.transform.gameObject.layer))
            {
                OnEnter?.Invoke();
            }
        }

        protected void OnTriggerStay2D(Collider2D collision)
        {
            if (targetLayer.IsMatch(collision.transform.gameObject.layer))
            {
                OnStay?.Invoke();
            }
        }

        protected void OnTriggerExit2D(Collider2D collision)
        {
            if (targetLayer.IsMatch(collision.transform.gameObject.layer))
            {
                OnExit?.Invoke();
            }
        }
    }
}