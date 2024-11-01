using Entities;
using Pool.ItemsPool.AnimationPool;
using System.Collections;
using UnityEngine;

namespace FiniteStateMachine.PlayerStates
{
    public sealed class PlayerKnockBackState : PlayerAbilityState
    {
        private readonly int hashKnockBacking = Animator.StringToHash("KnockBacking");
        private readonly int hashIsMoving = Animator.StringToHash("isMoving");
        private float finishTime;
        private bool isMoving;

        public bool isReady { get; private set; } = true;

        public PlayerKnockBackState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {
        }

        public override void Enter()
        {
            base.Enter();

            finishTime = Time.time + player.Data.KnockBackTime;
            physicsCore.Freezing.ResetFreezePos();
            player.Animator.SetBool(hashIsMoving, true);
            player.Animator.Play(hashKnockBacking);
            isMoving = true;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if(!isGrounded) 
            {
                isMoving = false;
                isAbilityDone = true;
            }
            else if (Time.time >= finishTime)
            {
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
                visualFxCore.AfterImage.CreateAfterImage(distanceBetweenImages: 0.3f);
            }
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            if (isMoving)
            {
                physicsCore.Movement.MoveAlongSurface(player.Data.KnockBackSpeed, physicsCore.Flipping.FacingDirection * -1);
            }
        }

        public override void Exit()
        {
            base.Exit();

            isReady = false;
            player.StartCoroutine(CoolDown(player.Data.KnockBackCooldown));
            physicsCore.Freezing.ResetFreezePos();
        }

        public IEnumerator CoolDown(float delay)
        {
            yield return new WaitForSeconds(delay);
            isReady = true;
        }

        public override void AnimationTrigger()
        {
            visualFxCore.AnimationFx.CreateAnimationFX(
                DustType.Brake,
                sensorCore.GroundDetector.GroundHit.point,
                player.transform.rotation, 
                flipHorizontal: true);
        }
    }
}
