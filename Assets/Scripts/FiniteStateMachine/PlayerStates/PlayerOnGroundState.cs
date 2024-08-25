using CoreSystem;
using Entities;
using Objects;
using UnityEngine;


namespace FiniteStateMachine.PlayerStates
{
    public abstract class PlayerOnGroundState : PlayerState
    {
        protected bool isGrounded; 
        protected abstract float ColiderHeight { get; }


        protected PlayerOnGroundState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {
        }


        public override void Enter()
        {
            base.Enter();

            bodyCore.BodyCollision.SetColliderHeight(ColiderHeight);           
        }

        public override void LogicUpdate() 
        {
            base.LogicUpdate();


            if(sensorCore.LadderDetector.TryGetLadderOnBottom(out Ladder ladder))
            {
                Debug.Log(ladder.ToString());
            }

            physicsCore.Flipping.FlipToDirection(player.Input.InputHorizontal);

            if (!isGrounded)
            {
                physicsCore.Freezing.ResetFreezePos();
                player.JumpState.DecreaseAmountOfJump();
				stateMachine.ChangeState(player.InAirState);
            }
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void DoCheck()
        {
            isGrounded = sensorCore.GroundDetector.IsGroundDetect();
        }
	}
}
