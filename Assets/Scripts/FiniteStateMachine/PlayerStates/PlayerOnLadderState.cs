using Entities;
using UnityEngine;


namespace FiniteStateMachine.PlayerStates
{
    public sealed class PlayerOnLadderState : PlayerState
    {

        public PlayerOnLadderState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {
        }

        public override void Enter()
        {
            base.Enter();
         
            player.transform.position = new Vector2(sensorCore.LadderDetector.CenterByHorizontOfLadder, player.transform.position.y);
            physicsCore.Gravitation.GravitationOff();
            physicsCore.Movement.SetVelocityZero();


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
