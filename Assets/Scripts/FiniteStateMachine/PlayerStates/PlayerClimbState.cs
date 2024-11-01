using Entities;
using Pool.ItemsPool.AnimationPool;
using UnityEngine;

namespace FiniteStateMachine.PlayerStates
{
    public sealed class PlayerClimbState : PlayerState
    {
        private readonly int hashClimb = Animator.StringToHash("Climb");

        private Vector2 climbedPosition;
        //private bool isTouchingCeiling;


        public PlayerClimbState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {
        }

        public override void Enter()
        {
            base.Enter();

            player.transform.position = GetClimbedPosition();
            physicsCore.Freezing.FreezePosY();

            player.Animator.Play(hashClimb);
            visualFxCore.Shadow.ShadowIsActive(false);
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

            //if (isTouchingCeiling) player.SetColliderHeight(player.Data.CrouchColiderHeight);
        }

        public override void DoCheck()
        {
            //isTouchingCeiling = IsLowCeiling();
        }

        public override void AnimationTrigger()
        {
            visualFxCore.Shadow.ShadowIsActive(true);
            visualFxCore.AnimationFx.CreateAnimationFX(DustType.Landing, climbedPosition, player.transform.rotation);
        }

        //private bool IsLowCeiling()
        //{
        //    float offset = Physics2D.defaultContactOffset;

        //    return Physics2D.Raycast(
        //        CornerPosition + (Vector2.up * offset) + (offset * player.Core.Movement.FacingDirection * Vector2.right),
        //        Vector2.up,
        //        player.Data.StandColiderHeight,
        //        player.Core.Sensor.TerrainLayer);
        //}

        private Vector2 GetClimbedPosition()
        {
            return new(PlayerHangOnLedgeState.GrapPosition.x + player.BodyCollider.size.x / 2 * physicsCore.Flipping.FacingDirection,
                PlayerHangOnLedgeState.GrapPosition.y + Physics2D.defaultContactOffset);
        }
    }
}
