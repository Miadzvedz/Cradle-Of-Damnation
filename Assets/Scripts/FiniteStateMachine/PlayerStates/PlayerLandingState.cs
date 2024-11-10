using Entities;
using Pool.ItemsPool.AnimationPool;
using System;
using UnityEngine;

namespace FiniteStateMachine.PlayerStates
{
    public sealed class PlayerLandingState : PlayerState
	{
        private readonly int hashSoftLanding = Animator.StringToHash("SoftLanding");
        private readonly int hashHardLanding = Animator.StringToHash("HardLanding");

        private bool isGrounded;
        private float landingForce;
        private Action updateLogic;

        public float LandingForce
        {
            get => Mathf.Abs(landingForce); 
            set => landingForce = value; 
        }

        public PlayerLandingState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {
        }

        public override void Enter()
        {
            base.Enter();
           
            player.AirDashState.ResetAmountOfDash();
			player.JumpState.ResetAmountOfJump();
			physicsCore.Movement.SetVelocityZero();

            if (sensorCore.GroundDetector.GetGroundSlopeAngle() != default)
            {
                physicsCore.Freezing.FreezePosX();
            }

            Vector2 groundSurfacePoint = sensorCore.GroundDetector.GroundHit.point;
            Quaternion rotation = player.transform.rotation;

            if (LandingForce >= player.Data.LandingThreshold)
            {
                visualFxCore.AnimationFx.CreateAnimationFX(DustType.HardLanding, groundSurfacePoint, rotation);
                player.Animator.Play(hashHardLanding);
                updateLogic = () =>
                {
                    if (isAnimFinished) 
                        stateMachine.ChangeState(player.StandState);                    
                };                               
            }
            else
            {
                player.Input.JumpEvent += OnJump;
                visualFxCore.AnimationFx.CreateAnimationFX(DustType.Landing, groundSurfacePoint, rotation);
                player.Animator.Play(hashSoftLanding);
                updateLogic = () =>
                {
                    if (isAnimFinished || player.Input.InputHorizontal != default) 
                        stateMachine.ChangeState(player.StandState);
                };
            }
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (!isGrounded)
            {
                stateMachine.ChangeState(player.InAirState);
            }
            else
            {
                updateLogic.Invoke();
            }
        }

        public override void Exit()
        {
            base.Exit();

            player.Input.JumpEvent -= OnJump;
        }     

        public override void DoCheck()
        {
            isGrounded = sensorCore.GroundDetector.IsPlatformDetect()
                || sensorCore.GroundDetector.IsOneWayPlatformDetect();
        }

        #region Input
        private void OnJump()
        {
            if (player.Input.InputVertical == Vector2.down.y)
            {
                if (!sensorCore.GroundDetector.IsOneWayPlatformDetect()) return;
                collisionCore.PlatformCollision.IgnoreOneWayPlatform();
            }
            else
            {
                stateMachine.ChangeState(player.JumpState);
            }
        }
        #endregion
    }
}
