using Entities;
using UnityEngine;


namespace FiniteStateMachine.PlayerStates
{
    public sealed class PlayerOnLadderState : PlayerState
    {
        private readonly int hashOnLadder = Animator.StringToHash("OnLadder");
        private readonly int verticalMoving = Animator.StringToHash("verticalMoving");

        public PlayerOnLadderState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {

        }

        public override void Enter()
        {
            base.Enter();

            player.Input.JumpEvent += OnJump;

            physicsCore.Gravitation.GravitationOff();
            physicsCore.Movement.SetVelocityZero();

            player.Animator.Play(hashOnLadder);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            player.Animator.SetFloat(verticalMoving, player.Input.InputVertical);

        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            physicsCore.Movement.SetVelocityY(player.Input.InputVertical * player.Data.OnLadderMoveSpeed);
        }

        public override void Exit()
        {
            base.Exit();

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
