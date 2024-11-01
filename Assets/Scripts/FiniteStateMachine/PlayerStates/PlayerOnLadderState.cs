using Entities;
using UnityEngine;


namespace FiniteStateMachine.PlayerStates
{
    public sealed class PlayerOnLadderState : PlayerState
    {
        private readonly int hashOnLadder = Animator.StringToHash("OnLadder");
        public static Vector2 Position { get; set; } 

        public PlayerOnLadderState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {
        }

        public override void Enter()
        {
            base.Enter();

            Debug.Log(Position);
            player.transform.position = Position;
         
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
