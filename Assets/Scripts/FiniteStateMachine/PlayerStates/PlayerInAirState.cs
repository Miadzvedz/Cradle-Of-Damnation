using Entities;
using Extensions;
using UnityEngine;

namespace FiniteStateMachine.PlayerStates
{
    public sealed class PlayerInAirState : PlayerState
    {
        private readonly int hashVelocityY = Animator.StringToHash("velocityY");
        private readonly int hashInAir = Animator.StringToHash("InAirState");

        private bool isOneWayPlatform;
        private bool isPlatform;
        private bool isLedgeDetected;
        private bool isGrabWallDetected;
        private bool isGirderDetected;
        private bool isFalling;
        private float fallingForce;

        public PlayerInAirState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {
        }

        public override void Enter()
        {
            base.Enter();

            player.Input.JumpEvent += OnJump;
            player.Input.DashEvent += OnAirDash;

            physicsCore.Freezing.ResetFreezePos();
            bodyCore.BodyCollision.SetColliderHeight(player.Data.StandColiderHeight);

            player.Animator.Play(hashInAir);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            //if (sensorCore.LadderDetector.IsOnLadder)
            //{
            //    Debug.Log(sensorCore.LadderDetector.IsOnLadder);
            //    if (player.Input.InputVertical == Vector2.up.y)
            //    {
            //        stateMachine.ChangeState(player.OnLadderState);
            //    }
            //}
            if (isOneWayPlatform || isPlatform)
            {               
                player.LandingState.LandingForce = fallingForce;
                stateMachine.ChangeState(player.LandingState);
            }
            else if (isLedgeDetected && isFalling) 
            {
				stateMachine.ChangeState(player.HangOnLedgeState);
            }
            else if (isGrabWallDetected && isFalling)
            {
				stateMachine.ChangeState(player.OnWallState);
            }
            else if(isGirderDetected && isFalling)
            {
                stateMachine.ChangeState(player.HangOnGirderState);
            }
            else
            {
                physicsCore.Flipping.FlipToDirection(player.Input.InputHorizontal);
                player.Animator.SetFloat(hashVelocityY, physicsCore.Movement.CurrentVelocity.y);
            }
        }

        public override void PhysicsUpdate() 
        {
            base.PhysicsUpdate();

            physicsCore.Movement.SetVelocityX(player.Input.InputHorizontal * player.Data.InAirMoveSpeed);
        }

        public override void Exit()
        {
            base.Exit();

            Reset();

            player.Input.JumpEvent -= OnJump;
            player.Input.DashEvent -= OnAirDash;

            player.JumpState.DisableDoubleJumpFX();
        }

        public override void DoCheck()
        {
            TrackingFallingForce();

            isFalling = physicsCore.Movement.CurrentVelocity.y <= 0;

            isPlatform = sensorCore.GroundDetector.IsPlatformDetect();

            if (isFalling) 
            {
                isOneWayPlatform = sensorCore.GroundDetector.IsOneWayPlatformDetect();
            }
                
            if (isGrabWallDetected = sensorCore.GrabWallDetector.TryGetGrabWallPosition(out Vector2 wallPosition))
            {
                PlayerOnWallState.DetectedPosition = wallPosition;
            }                
          
            if (player.HangOnLedgeState.isReady)
            {
                if (isLedgeDetected = sensorCore.LedgeDetector.TryGetLedgeCorner(out Vector2 ledgeCorner))
                    PlayerHangState.GrapPosition = ledgeCorner;
            }
            
            if (player.HangOnGirderState.isReady)
            {
                if (isGirderDetected = sensorCore.GirderDetector.TryGetGirderPosition(out Vector2 girderPosition))
                    PlayerHangState.GrapPosition = girderPosition;
            }
        }

		public override void AnimationTrigger() 
            => player.Animator.Play(hashInAir);


        private void TrackingFallingForce() 
        {
            if (physicsCore.Movement.CurrentVelocity.y < fallingForce)
            {
                fallingForce = physicsCore.Movement.CurrentVelocity.y;
            }
        }

        private void Reset()
        {
            isGirderDetected = false;
            isLedgeDetected = false;
            isGrabWallDetected = false;
            isOneWayPlatform = false;
            isPlatform = false;
            fallingForce = default;
        }

        #region Input
        private void OnJump()
        {
            if (!player.JumpState.CanJump()) return;

            stateMachine.ChangeState(player.JumpState);
        }

        private void OnAirDash()
        {
			if (!player.AirDashState.CanDash()) return;

            stateMachine.ChangeState(player.AirDashState);
        }
        #endregion
    }
}