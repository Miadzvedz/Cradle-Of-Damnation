using Entities;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;


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
         
            player.transform.position = new Vector2(sensorCore.LadderDetector.CenterByHorizontOfLadder, player.transform.position.y);
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
