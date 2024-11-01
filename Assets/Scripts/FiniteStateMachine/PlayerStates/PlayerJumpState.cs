using Entities;
using Pool.ItemsPool.AnimationPool;
using System;
using UnityEngine;

namespace FiniteStateMachine.PlayerStates
{
    public sealed class PlayerJumpState : PlayerAbilityState
    {
		private readonly int hashVelocityY = Animator.StringToHash("velocityY");
		private readonly int hashInAir = Animator.StringToHash("InAirState");
		private byte amountOfJumpLeft;
		private float finishTime;
		private Action jumpUpdate;
		private AbilityFx wingsDoubleJump;

        public PlayerJumpState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {
			amountOfJumpLeft = player.Data.AmountOfJump;
		}

        public override void Enter()
        {
            base.Enter();

			if (player.PreviousState is PlayerOnWallState)
			{
                visualFxCore.AnimationFx.CreateAnimationFX(DustType.JumpFromWall,
				new Vector2()
				{
					x = physicsCore.Flipping.IsLeftDirection()
						? player.BodyCollider.bounds.min.x
						: player.BodyCollider.bounds.max.x,
					y = player.BodyCollider.bounds.min.y,
				},
                player.transform.rotation, true);

                finishTime = Time.time + player.Data.WallJumpTime;
				player.Animator.Play(hashInAir);
				physicsCore.Flipping.Flip();
                physicsCore.Movement.SetVelocity(
					player.Data.JumpForce,
					new Vector2(1, 2),
                    physicsCore.Flipping.FacingDirection);
                jumpUpdate = UpdateJumpFromWall;
			}
            else if (player.PreviousState is PlayerInAirState)
			{
                wingsDoubleJump = (AbilityFx)visualFxCore.AnimationFx.CreateAnimationFX(
					AbilityFXType.WingsDoubleJump,
					new Vector2(x:0, y:0.9f));
                physicsCore.Movement.SetVelocityY(player.Data.DoubleJumpForce);
                jumpUpdate = UpdateJump;
			}
            else if (player.PreviousState is PlayerHangOnGirderState)
            {
                physicsCore.Movement.SetVelocityY(player.Data.JumpForce);
                jumpUpdate = UpdateJump;
            }
            else
			{
                visualFxCore.AnimationFx.CreateAnimationFX(
                    DustType.JumpFromGround,
                    sensorCore.GroundDetector.GroundHit.point,
                    player.transform.rotation);

                physicsCore.Movement.SetVelocityY(player.Data.JumpForce);
                jumpUpdate = UpdateJump;
            }
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            jumpUpdate.Invoke();
        }

        public override void Exit()
        {
            base.Exit();

            DecreaseAmountOfJump();
		}

		#region Update
		private void UpdateJump()
		{
			if (!isGrounded) isAbilityDone = true;
		}

		private void UpdateJumpFromWall()
		{			
			player.Animator.SetFloat(hashVelocityY, physicsCore.Movement.CurrentVelocity.y);
			if (Time.time >= finishTime) isAbilityDone = true;
		}
		#endregion

		public void ResetAmountOfJump() => amountOfJumpLeft = player.Data.AmountOfJump;
        public void DecreaseAmountOfJump() => amountOfJumpLeft--;
		public bool CanJump() => amountOfJumpLeft > 0;
		public void DisableDoubleJumpFX()
		{
			if (wingsDoubleJump != null)
			{
				if (wingsDoubleJump.isActiveAndEnabled)
				{
                    wingsDoubleJump.ReturnToPool();
                }
			}
        }
	}
}