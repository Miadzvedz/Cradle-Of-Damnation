using Entities;
using UnityEngine;


namespace FiniteStateMachine.PlayerStates
{
    public sealed class PlayerCrouchState : PlayerOnGroundState
    {
        protected override float ColiderHeight => player.Data.CrouchColiderHeight;
        private readonly int hashIdle = Animator.StringToHash("IdleCrouch");


        public PlayerCrouchState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {		
		}

        public override void Enter()
        {
            base.Enter();

            player.Animator.Play(hashIdle);

            player.Input.JumpEvent += OnJumpFromOneWayPlatform;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
          
            if (player.Input.InputVertical != Vector2.down.y)
            {
                stateMachine.ChangeState(player.SitStandState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            physicsCore.Movement.SetVelocityZero();
        }

        public override void Exit()
        {
            base.Exit();

            player.Input.JumpEvent -= OnJumpFromOneWayPlatform;
        }

        public override void DoCheck()
        {
            isGrounded = sensorCore.GroundDetector.IsPlatformDetect() 
                || sensorCore.GroundDetector.IsOneWayPlatformDetect();
        }

        private void OnJumpFromOneWayPlatform()
        {
            if (!sensorCore.GroundDetector.IsOneWayPlatformDetect()) return;
            collisionCore.PlatformCollision.IgnoreOneWayPlatform();
        }
    }
}