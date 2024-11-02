using Entities;
using UnityEngine;


namespace FiniteStateMachine.PlayerStates
{
    public sealed class PlayerOnLadderState : PlayerState
    {
        private readonly int hashOnLadder = Animator.StringToHash("OnLadder");

        public PlayerOnLadderState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {
        }

        public override void Enter()
        {
            base.Enter();
         
            physicsCore.Gravitation.GravitationOff();
            physicsCore.Movement.SetVelocityZero();

            player.Animator.Play(hashOnLadder);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
        }

        public override void Exit()
        {
            base.Exit();

        }
    }
}
