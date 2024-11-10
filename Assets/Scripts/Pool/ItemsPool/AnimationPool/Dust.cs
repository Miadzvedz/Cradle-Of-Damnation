using UnityEngine;


namespace Pool.ItemsPool.AnimationPool
{
    public sealed class Dust : AnimationEffect<DustType>
    {
        #region Hash Animations
        private readonly int afterMove = Animator.StringToHash("AfterMoveDust");
        private readonly int jumpFromGround = Animator.StringToHash("JumpFromGroundDust");
        private readonly int jumpFromWall = Animator.StringToHash("JumpFromWallDust");
        private readonly int landing = Animator.StringToHash("LandingDust");
        private readonly int hardLanding = Animator.StringToHash("HardLandingDust");
        private readonly int tiny = Animator.StringToHash("TinyDust");
        private readonly int startSlide = Animator.StringToHash("StartSlideDust");
        private readonly int brake = Animator.StringToHash("BrakeDust");
        private readonly int sliding = Animator.StringToHash("SlidingDust");
        private readonly int dash = Animator.StringToHash("DashDust");
        private readonly int landingOnWall = Animator.StringToHash("LandingOnWallDust");
        #endregion

        protected override int GetAnimationHash(DustType animationFX)
            => animationFX switch
            {
                DustType.JumpFromGround => jumpFromGround,
                DustType.JumpFromWall => jumpFromWall,
                DustType.HardLanding => hardLanding,
                DustType.StartSlide => startSlide,
                DustType.AfterMove => afterMove,
                DustType.Sliding => sliding,
                DustType.Landing => landing,
                DustType.Brake => brake,
                DustType.Tiny => tiny,
                DustType.Dash => dash,
                DustType.LandingOnWall => landingOnWall,
                _ => default
            };
    }

    public enum DustType : byte
    {
        AfterMove = 1,
        JumpFromGround = 2,
        JumpFromWall = 3,
        Landing = 4,
        Tiny = 5,
        StartSlide = 6,
        Brake = 7,
        HardLanding = 8,
        Sliding = 9,
        Dash = 10,
        LandingOnWall = 11,
    }
}
