using Entities;
using Extensions;
using Pool.ItemsPool.AnimationPool;
using UnityEngine;

namespace FiniteStateMachine.PlayerStates
{
    public sealed class PlayerAirDashState : PlayerAbilityState
    {
        private readonly int hashDash = Animator.StringToHash("AirDashing");
		private readonly int amountOfDash = 1;
		private int amountOfDashLeft;
        private float finishTime;

		public PlayerAirDashState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {
            amountOfDashLeft = amountOfDash;
        }

        public override void Enter()
        {
            base.Enter();

            finishTime = Time.time + player.Data.DashTime;
            physicsCore.Freezing.FreezePosY();
            physicsCore.Gravitation.GravitationOff();
			player.Animator.Play(hashDash);
            visualFxCore.AnimationFx.CreateAnimationFX(DustType.Dash, player.BodyCollider.bounds.center, player.transform.rotation);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (Time.time >= finishTime)
            {
                isAbilityDone = true;
            }
            else
            {
                visualFxCore.AfterImage.CreateAfterImage(0.6f);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            physicsCore.Movement.SetVelocity(player.Data.DashSpeed, Vector2.right, physicsCore.Flipping.FacingDirection);
        }

        public override void Exit()
        {
            base.Exit();

			DecreaseAmountOfDash();
            physicsCore.Movement.SetVelocityZero();
            physicsCore.Freezing.ResetFreezePos();
            physicsCore.Gravitation.GravitationOn();
		}

        public bool CanDash() => amountOfDashLeft.IsPositive();
		public void ResetAmountOfDash() => amountOfDashLeft = amountOfDash;
		public void DecreaseAmountOfDash() => amountOfDashLeft--;
	}
}
