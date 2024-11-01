using Interfaces;
using System;
using UnityEngine;


namespace Pool.ItemsPool.AnimationPool
{
    public abstract class AnimationEffect<T> : PooledObject, IAnimationFinishTrigger
        where T : Enum
    {
        [SerializeField]
        protected Transform updatedTransform;
        [SerializeField]
        protected Vector2 offsetPosition;
        protected Vector2 workingVector;

        [SerializeField]
        protected int currentHashAnimation;
        protected Animator animator;


        protected void Awake()
        {
            animator = GetComponent<Animator>();
        }

        protected void OnEnable()
        {
            if(currentHashAnimation != default)
            {
                animator.Play(currentHashAnimation);
            }
        }

        protected void Update()
        {
            if (updatedTransform == null) return;
            
            workingVector.Set(
                updatedTransform.rotation.y < 0 
                    ? updatedTransform.position.x - offsetPosition.x 
                    : updatedTransform.position.x + offsetPosition.x,
                updatedTransform.position.y + offsetPosition.y);

            gameObject.transform.SetPositionAndRotation(workingVector, updatedTransform.rotation);           
        }

        protected void OnDisable()
        {
            updatedTransform = null;   
            currentHashAnimation = default;
            offsetPosition = default;
        }

        public override void Get(GameObject obj)
        {
            obj.SetActive(false);
        }

        public override GameObject Create(Transform container)
        {
            gameObject.SetActive(false);
            return base.Create(container);
        }

        public void Initialize(T animationFX) 
            => currentHashAnimation = GetAnimationHash(animationFX);

        public void Initialize(T animationFX, Transform transform, Vector2 offset)
        {
            currentHashAnimation = GetAnimationHash(animationFX);
            updatedTransform = transform;
            offsetPosition = offset;
        }

        protected abstract int GetAnimationHash(T animationFX);

        public void AnimationFinishTrigger()
             => ReturnToPool();
    }
}