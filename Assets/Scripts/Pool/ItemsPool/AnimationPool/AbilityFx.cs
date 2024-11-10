using UnityEngine;

namespace Pool.ItemsPool.AnimationPool
{
    public sealed class AbilityFx : AnimationEffect<AbilityFXType>
    {
        #region Hash Animations
        private readonly int wingsDoubleJump = Animator.StringToHash("WingsDoubleJump");
        #endregion

        protected override int GetAnimationHash(AbilityFXType animationFX)
            => animationFX switch
            {
                AbilityFXType.WingsDoubleJump => wingsDoubleJump,
                _ => default
            };
    }

    public enum AbilityFXType : byte
    {
        WingsDoubleJump = 1,
    }
}
