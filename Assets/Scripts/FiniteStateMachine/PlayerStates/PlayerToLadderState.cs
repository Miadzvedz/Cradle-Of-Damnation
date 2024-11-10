using CoreSystem.CoreComponents.SensorDetectComponents;
using Entities;
using UnityEngine;


namespace FiniteStateMachine.PlayerStates
{
    public sealed class PlayerToLadderState : PlayerState
    {
        private readonly int hashTopToLadder = Animator.StringToHash("TopToLadder");
        private readonly int hashBottomToLadder = Animator.StringToHash("BottomToLadder");
        private readonly float offsetPosition = 1.12f;
        private Vector2 position;
        private LadderPlace fromPlace;

        public PlayerToLadderState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {
        }

        public override void Enter()
        {
            base.Enter();

            player.transform.position = position;

            physicsCore.Freezing.FreezePosY();
            physicsCore.Movement.SetVelocityZero();

            if (fromPlace.Equals(LadderPlace.Top))          
                player.Animator.Play(hashTopToLadder);           
            else if (fromPlace.Equals(LadderPlace.Bottom))
                player.Animator.Play(hashBottomToLadder);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (fromPlace.Equals(LadderPlace.Mid))
            {
                stateMachine.ChangeState(player.OnLadderState);
            }
            else
            {
                if (isAnimFinished)
                {
                    stateMachine.ChangeState(player.OnLadderState);
                }
            }
        }

        public override void Exit()
        {
            base.Exit();

            physicsCore.Freezing.ResetFreezePos();

            if (fromPlace.Equals(LadderPlace.Top))
            {
                collisionCore.PlatformCollision.IgnoreOneWayPlatform();
                var tempVector = new Vector3(player.transform.position.x, player.transform.position.y - offsetPosition);
                player.transform.position = tempVector;
            }

        }

        public void Initialize(Vector2 position, LadderPlace fromPlace)
        {
            this.position = position;
            this.fromPlace = fromPlace;
        }
    }
}
