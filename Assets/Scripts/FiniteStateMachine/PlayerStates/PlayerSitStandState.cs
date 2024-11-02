using Entities;
using System;
using UnityEngine;

namespace FiniteStateMachine.PlayerStates
{
    public sealed class PlayerSitStandState : PlayerState
    {
        private readonly int hashStandUp = Animator.StringToHash("StandUp");
        private readonly int hashSitDown = Animator.StringToHash("SitDown");

        private bool isGrounded;
        private Action update;

        public PlayerSitStandState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {
        }

        public override void Enter()
        {
            base.Enter();

            player.Input.JumpEvent += OnJump;

            physicsCore.Movement.SetVelocityZero();
            physicsCore.Freezing.FreezePosOnSlope();

            switch (player.PreviousState)
            {
                case PlayerCrouchState:
                    player.Animator.Play(hashStandUp);
                    update = () => stateMachine.ChangeState(player.StandState);
                    break;
                case PlayerStandState:
                    player.Animator.Play(hashSitDown);
                    update = () => stateMachine.ChangeState(player.CrouchState);
                    break;
            }
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (isAnimFinished)
            {
                update.Invoke();
            }

            if (!isGrounded)
            {
                physicsCore.Freezing.ResetFreezePos();
                stateMachine.ChangeState(player.InAirState);
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
    }
}
