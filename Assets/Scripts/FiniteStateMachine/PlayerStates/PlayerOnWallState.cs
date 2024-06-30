using Entities;
using Pool.ItemsPool;
using UnityEngine;

namespace FiniteStateMachine.PlayerStates
{
    public sealed class PlayerOnWallState : PlayerState
    {
        public static Vector2 DetectedPosition { private get; set; }

        private readonly int hashLandingOnWall = Animator.StringToHash("LandingOnWall");
        private Vector2 holdPosition;

        public PlayerOnWallState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {
        }

        public override void Enter()
        {
            base.Enter();

            CreateDust(yOffset: 0.15f);

            player.Input.JumpEvent += OnJump;

			player.AirDashState.ResetAmountOfDash();
			player.JumpState.ResetAmountOfJump();

            physicsCore.Movement.SetVelocityZero();

            holdPosition.Set(
                DetectedPosition.x - (player.BodyCollider.size.x / 2 + Physics2D.defaultContactOffset) * physicsCore.Flipping.FacingDirection,
                DetectedPosition.y - player.BodyCollider.size.y);          

            player.transform.position = holdPosition;

            physicsCore.Freezing.FreezePosY();

            player.Animator.Play(hashLandingOnWall);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

        }

        public override void Exit()
        {
            base.Exit();

            physicsCore.Freezing.ResetFreezePos();
            player.Input.JumpEvent -= OnJump;
        }


        #region Input
        private void OnJump()
        {
            if (!isAnimFinished) return;

            stateMachine.ChangeState(player.JumpState);
        }
        #endregion

        private void CreateDust(float yOffset)
        {
            visualFxCore.AnimationFx.CreateAnimationFX(DustType.LandingOnWall,
                new Vector2(DetectedPosition.x, player.BodyCollider.bounds.center.y - yOffset),
                player.transform.rotation);
        }
    }
}
