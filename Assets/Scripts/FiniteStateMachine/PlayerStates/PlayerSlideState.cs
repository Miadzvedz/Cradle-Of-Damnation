using Entities;
using Pool.ItemsPool.AnimationPool;
using System.Collections;
using UnityEngine;

namespace FiniteStateMachine.PlayerStates
{
    public sealed class PlayerSlideState : PlayerAbilityState
    {
        private readonly int hashStartDash = Animator.StringToHash("StartSlide");
        private readonly int hashIsMoving = Animator.StringToHash("isMoving");

        private bool isMoving;
        private bool isTouchedRoof;
        private float finishTime;
        private Dust dust;

        public bool isReady { get; private set; } = true;

        public PlayerSlideState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {

        }

        public override void Enter()
        {
            base.Enter();

            visualFxCore.AnimationFx.CreateAnimationFX(
                DustType.StartSlide,
                sensorCore.GroundDetector.GroundHit.point,
                player.transform.rotation);

            dust = (Dust)visualFxCore.AnimationFx.CreateAnimationFX(
                DustType.Sliding,
                new Vector2(x: 0.4f, y: 0.0f));

            finishTime = Time.time + player.Data.SlideTime;

            physicsCore.Freezing.ResetFreezePos();
            bodyCore.BodyCollision.SetColliderHeight(player.Data.CrouchColiderHeight);

            player.Animator.SetBool(hashIsMoving, true);
            player.Animator.Play(hashStartDash);
            isMoving = true;           
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (!isGrounded)
            {
                isMoving = false;
                isAbilityDone = true;
            }
            else if (Time.time >= finishTime && !isTouchedRoof)
            {
                dust.ReturnToPool();
                player.Animator.SetBool(hashIsMoving, false);
                isMoving = false;

                if (!isAnimFinished)
                {
                    physicsCore.Movement.SetVelocityZero();
                    physicsCore.Freezing.FreezePosOnSlope();
                }
                else
                { 
                    isAbilityDone = true;
                }
            }
            else
            {               
                visualFxCore.AfterImage.CreateAfterImage(distanceBetweenImages: 0.6f);

            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            if (isMoving)
            {
                physicsCore.Movement.MoveAlongSurface(player.Data.SlideSpeed, physicsCore.Flipping.FacingDirection);
            }
        }

        public override void Exit()
        {
            base.Exit();

            isReady = false;
            player.StartCoroutine(CoolDown(player.Data.SlideCooldown));
            dust.ReturnToPool();
            physicsCore.Freezing.ResetFreezePos();
        }

        public override void DoCheck()
        {
            base.DoCheck();

            isTouchedRoof = sensorCore.CeilingDetector.IsCeilingDetect();
        }

        public IEnumerator CoolDown(float delay)
        {
            yield return new WaitForSeconds(delay);
            isReady = true;
        }

        public override void AnimationTrigger()
        {
            float offsetX = 0.4f;

            visualFxCore.AnimationFx.CreateAnimationFX(
                DustType.Brake,
                new Vector2
                {
                    y = player.BodyCollider.bounds.min.y,
                    x = physicsCore.Flipping.IsLeftDirection()
                        ? player.BodyCollider.bounds.min.x - offsetX
                        : player.BodyCollider.bounds.max.x + offsetX,
                },
                player.transform.rotation) ;
        }
    }
}