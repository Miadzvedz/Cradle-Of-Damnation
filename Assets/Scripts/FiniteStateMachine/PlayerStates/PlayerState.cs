using Entities;
using Interfaces;
using CoreSystem.CoreComponents;
using UnityEngine;


namespace FiniteStateMachine.PlayerStates
{
    public abstract class PlayerState : IState
    {
        protected StateMachine stateMachine;
        protected Player player;
        protected bool isAnimFinished;

        #region Core
        protected PhysicsManipulation physicsCore;
        protected SensorDetect sensorCore;
        protected VisualFx visualFxCore;
        protected CollisionManipulation collisionCore;
        #endregion

        public PlayerState(StateMachine stateMachine, Player player)
        {
            this.stateMachine = stateMachine;
            this.player = player;
            physicsCore = player.Core.Physics;
            sensorCore = player.Core.Sensor;
            visualFxCore = player.Core.VisualFx;
            collisionCore = player.Core.Collision;
        }

        public virtual void Enter()
        {
            Debug.Log(this);

            isAnimFinished = false;
            DoCheck();
        }

        public virtual void LogicUpdate()
        {
            
        }

        public virtual void PhysicsUpdate()
        {
            DoCheck();
        }

        public virtual void Exit()
        {
            player.PreviousState = this;
        }

        public virtual void DoCheck() 
        {
        }

        public virtual void AnimationFinishTrigger()
            => isAnimFinished = true;

        public virtual void AnimationTrigger()
        {
        }
    }
}