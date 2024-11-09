using CoreSystem.CoreComponents.SensorDetectComponents;
using Entities;
using UnityEngine;


namespace FiniteStateMachine.PlayerStates
{
    public sealed class PlayerOnLadderState : PlayerState
    {
        private readonly int hashOnLadder = Animator.StringToHash("OnLadder");
        private readonly int verticalMoving = Animator.StringToHash("verticalMoving");
        private bool isMoovingUp;
        private bool isMoovingDown;
        private bool isGrounded;
        private bool isOnLadder;
        private bool isTopLadderDetected;

        public PlayerOnLadderState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {
        }

        public override void Enter()
        {
            base.Enter();

            player.Input.JumpEvent += OnJump;

            physicsCore.Gravitation.GravitationOff();
            physicsCore.Freezing.FreezePosX();
            physicsCore.Movement.SetVelocityZero();

            player.Animator.Play(hashOnLadder);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            player.Animator.SetFloat(verticalMoving, player.Input.InputVertical);

            physicsCore.Flipping.FlipToDirection(player.Input.InputHorizontal);

            isMoovingDown = player.Input.IsDownInput;
            isMoovingUp = player.Input.IsUpInput;

            if (isMoovingDown)
            {             
                if (isGrounded)
                {
                    player.FromLadderState.Initialize(
                        sensorCore.GroundDetector.GroundHit.point,
                        LadderPlace.Bottom);

                    stateMachine.ChangeState(player.FromLadderState);
                }
            }

            if (isMoovingUp)
            {
                if (isTopLadderDetected)
                {
                    stateMachine.ChangeState(player.FromLadderState);
                }
            }

            if (!isOnLadder)
            {
                if (isGrounded) stateMachine.ChangeState(player.InAirState);
                else stateMachine.ChangeState(player.InAirState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            physicsCore.Movement.SetVelocityY(player.Input.InputVertical * player.Data.OnLadderMoveSpeed);
        }

        public override void DoCheck()
        {
            base.DoCheck();

            isOnLadder = sensorCore.LadderDetector.IsOnLadder();

            if (isMoovingUp)
            {
                if (isTopLadderDetected = sensorCore.LadderDetector.TryGetTopOfLadderPosition(out Vector2 position))
                    player.FromLadderState.Initialize(position, LadderPlace.Top);
            }

            if (isMoovingDown)
            {
                isGrounded = sensorCore.GroundDetector.IsGroundDetect(Physics2D.defaultContactOffset);
            }

        }

        public override void Exit()
        {
            base.Exit();

            isGrounded = false;
            isTopLadderDetected = false;
            isMoovingDown = false;
            isMoovingUp = false;

            physicsCore.Gravitation.GravitationOn();

            player.Input.JumpEvent -= OnJump;
        }


        #region Input
        private void OnJump()
        {
            physicsCore.Gravitation.GravitationOn();
            stateMachine.ChangeState(player.JumpState);
        }
        #endregion
    }
}
