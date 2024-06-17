using Entities;
using Pool.ItemsPool;
using UnityEngine;


namespace FiniteStateMachine.PlayerStates
{
    public sealed class PlayerStandState : PlayerOnGroundState
    {
        protected override float ColiderHeight => player.Data.StandColiderHeight;

        private readonly int hashIsMoving = Animator.StringToHash("isMoving");
        private readonly int hashIdle = Animator.StringToHash("IdleStand");
        private readonly int hashMove = Animator.StringToHash("RunStart");


        public PlayerStandState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {			
		}

        public override void Enter()
        {
            base.Enter();

            player.Input.JumpEvent += OnJump;
            player.Input.DashEvent += OnDash;

            player.Animator.Play(player.Input.InputHorizontal != default ? hashMove : hashIdle);
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            player.Animator.SetBool(hashIsMoving, player.Input.InputHorizontal != default);  
            
            if(player.Input.InputVertical == Vector2.down.y)
            {
                stateMachine.ChangeState(player.SitStandState);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            physicsCore.Movement.Move(player.Data.StandMoveSpeed, player.Input.InputHorizontal);
        }

        public override void Exit()
        {
            base.Exit();
            physicsCore.Movement.SetVelocityZero();
            player.Input.JumpEvent -= OnJump;
            player.Input.DashEvent -= OnDash;
        }

        public override void AnimationTrigger()
        {
            if (player.Input.InputHorizontal != default)
            {
                visualFxCore.AnimationFx.CreateAnimationFX(
                    DustType.Tiny,
                    sensorCore.GroundDetector.GroundHit.point,
                    player.transform.rotation);
            }
            else
            {
                visualFxCore.AnimationFx.CreateAnimationFX(
                    DustType.AfterMove,
                    new Vector2()
                    {
                        x = physicsCore.Flipping.IsLeftDirection()
                            ? player.BodyCollider.bounds.min.x
                            : player.BodyCollider.bounds.max.x,
                        y = sensorCore.GroundDetector.GroundHit.point.y,
                    },
                    player.transform.rotation);
            }
        }

        #region Input
		private void OnJump()
		{
            stateMachine.ChangeState(player.JumpState); 
        }

		private void OnDash()
		{
            if (player.Input.InputHorizontal == physicsCore.Flipping.FacingDirection)
            {
                if (!player.SlideState.isReady) return;
			    stateMachine.ChangeState(player.SlideState);
            }
            else
            {
                if (!player.KnockBackState.isReady) return;
                stateMachine.ChangeState(player.KnockBackState);
            }
        }
        #endregion
    }
}
