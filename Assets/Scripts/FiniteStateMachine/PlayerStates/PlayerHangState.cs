using Entities;
using System.Collections;
using UnityEngine;

namespace FiniteStateMachine.PlayerStates
{
    public abstract class PlayerHangState : PlayerState
    {
        private readonly int hashHanging = Animator.StringToHash("Grab");
        protected bool isHanging;
        public bool isReady { get; private set; } = true;
        public static Vector2 GrapPosition { get; set; }

        public PlayerHangState(StateMachine stateMachine, Player player) : base(stateMachine, player)
        {            
        }

        public override void Enter()
        {
            base.Enter();

            player.Animator.Play(hashHanging);

            player.JumpState.ResetAmountOfJump();
            physicsCore.Movement.SetVelocityZero();
            physicsCore.Freezing.FreezePosY();

            player.transform.position = GetHangingPosition();
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (player.Input.IsDownInput && isHanging)
            {
                isReady = false;
                player.StartCoroutine(CoolDown(0.2f));
                physicsCore.Movement.SetVelocityY(0.1f);
                stateMachine.ChangeState(player.InAirState);                            
            }
        }

        public override void Exit()
        {
            base.Exit();
            isHanging = false;
            physicsCore.Freezing.ResetFreezePos();               
        }

        public override void AnimationTrigger()
            => isHanging = true;


        public IEnumerator CoolDown(float delay)
        {
            yield return new WaitForSeconds(delay);
            isReady = true;
        }

        protected abstract Vector2 GetHangingPosition();
    }
}
