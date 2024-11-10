using FiniteStateMachine;
using UnityEngine;
using Interfaces;
using CoreSystem;


namespace Entities
{
    public abstract class Entity : MonoBehaviour, IAnimationFinishTrigger, IAnimationTrigger
    {
        protected StateMachine stateMachine;
        public Animator Animator { get; private set; }
        public Rigidbody2D Rigidbody {  get; private set; }
        public CapsuleCollider2D BodyCollider {  get; private set; }   
        public Core Core { get; private set; }


        protected virtual void Awake()
        {
            Core = GetComponentInChildren<Core>();

            stateMachine = new StateMachine();

            Animator = GetComponent<Animator>();
            Rigidbody = GetComponent<Rigidbody2D>();
            BodyCollider = GetComponent<CapsuleCollider2D>();
        }

        protected virtual void Start()
        {
        }

        protected virtual void Update()
        {
            Core.LogicUpdate();
            stateMachine.CurrentState.LogicUpdate();           
        }

        protected virtual void FixedUpdate()
        {
            stateMachine.CurrentState.PhysicsUpdate();
        }

        public void AnimationFinishTrigger() 
            => stateMachine.CurrentState.AnimationFinishTrigger();

        public void AnimationTrigger() 
            => stateMachine.CurrentState.AnimationTrigger();
    }
}