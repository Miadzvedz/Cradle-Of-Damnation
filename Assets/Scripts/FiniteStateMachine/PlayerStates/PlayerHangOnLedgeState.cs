using Entities;
using Pool.ItemsPool.AnimationPool;
using UnityEngine;

namespace FiniteStateMachine.PlayerStates
{
    public sealed class PlayerHangOnLedgeState : PlayerHangState
    {
        public PlayerHangOnLedgeState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {
            
        }

        public override void Enter()
        {
            base.Enter();

            player.Input.JumpEvent += OnClimb;

            visualFxCore.AnimationFx.CreateAnimationFX(
                DustType.Tiny,
                GrapPosition,
                new Quaternion() { y = physicsCore.Flipping.IsLeftDirection() ? 0 : 180 });
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (player.Input.InputVertical == Vector2.up.y && isHanging)
            {
                stateMachine.ChangeState(player.ClimbState);
            }
        }

        public override void Exit()
        {
            base.Exit();
       
            player.Input.JumpEvent -= OnClimb;
        }

        protected override Vector2 GetHangingPosition()
        {
            return new(GrapPosition.x - (player.BodyCollider.size.x / 2 + Physics2D.defaultContactOffset) * physicsCore.Flipping.FacingDirection,
                GrapPosition.y - player.BodyCollider.size.y + Physics2D.defaultContactOffset);
        }

        #region Input
        private void OnClimb()
        {
            if (!isHanging) return;

            stateMachine.ChangeState(player.ClimbState);
        }
        #endregion
    }
}
