using CoreSystem.CoreComponents.SensorDetectComponents;
using Entities;
using UnityEngine;


namespace FiniteStateMachine.PlayerStates
{
    public sealed class PlayerFromLadderState : PlayerState
    {
        private readonly int hashLadderToTop = Animator.StringToHash("LadderToTop");
        private readonly int hashLadderToBottom = Animator.StringToHash("LadderToBottom");
        private readonly float offsetPosition = Physics2D.defaultContactOffset + 0.00003f;
        private Vector2 position;
        private LadderPlace fromPlace;


        public PlayerFromLadderState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {
        }

        public override void Enter()
        {
            base.Enter();

            player.transform.position = new Vector2(position.x, position.y + offsetPosition);

            physicsCore.Freezing.FreezePosY();
            physicsCore.Movement.SetVelocityZero();

            if (fromPlace.Equals(LadderPlace.Top))
                player.Animator.Play(hashLadderToTop);
            else if (fromPlace.Equals(LadderPlace.Bottom))
                player.Animator.Play(hashLadderToBottom);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (isAnimFinished)
            {
                stateMachine.ChangeState(player.StandState);
            }
        }

        public override void Exit()
        {
            base.Exit();

            physicsCore.Freezing.ResetFreezePos();
        }

        public void Initialize(Vector2 position, LadderPlace fromPlace)
        {
            this.position = position;
            this.fromPlace = fromPlace;
        }
    }
}
