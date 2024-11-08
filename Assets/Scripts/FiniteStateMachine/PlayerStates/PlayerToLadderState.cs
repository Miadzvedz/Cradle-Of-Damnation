using CoreSystem.CoreComponents.SensorDetectComponents;
using Entities;
using UnityEngine;

namespace FiniteStateMachine.PlayerStates
{
    public sealed class PlayerToLadderState : PlayerState
    {
        private readonly int hashTopToLadder = Animator.StringToHash("TopToLadder");
        private readonly int hashBottomToLadder = Animator.StringToHash("BottomToLadder");
        private readonly int hashMidToLadder = Animator.StringToHash("MidToLadder");
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
         
            physicsCore.Gravitation.GravitationOff();
            physicsCore.Movement.SetVelocityZero();

            if (fromPlace.Equals(LadderPlace.Top))          
                player.Animator.Play(hashTopToLadder);           
            else if (fromPlace.Equals(LadderPlace.Bottom))
                player.Animator.Play(hashBottomToLadder);
            else if (fromPlace.Equals(LadderPlace.Mid))
                player.Animator.Play(hashMidToLadder);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (isAnimFinished)
            {
                stateMachine.ChangeState(player.OnLadderState);
            }
        }

        public override void Exit()
        {
            base.Exit();

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
