using Managers;
using Pool;
using Pool.ItemsPool.AnimationPool;
using System;
using UnityEngine;


namespace CoreSystem.CoreComponents.VisualFxComponents
{
    public class AnimationFx : VisualFxComponent
    {
        [SerializeField]
        private AbilityFx abilityFXPrefab;

        [SerializeField]
        private Dust dustPrefab;


        public AnimationEffect<T> CreateAnimationFX<T>(T animationTypeFX, Vector2 offset = default)
            where T : Enum
        {
            AnimationEffect<T> animationFx = PoolManager.Instance.GetFromPool<AnimationEffect<T>>(GetAnimationGameObject(animationTypeFX), entityTransform.position, entityTransform.rotation);

            if (animationFx != null)
            {
                animationFx.Initialize(animationTypeFX, entityTransform, offset);
                animationFx.SetActive(true);
            }
            return animationFx;
        }

        public void CreateAnimationFX<T>(T animationTypeFX, Vector2 position, Quaternion rotation, bool flipHorizontal = false)
            where T : Enum
        {
            AnimationEffect<T> animationFx = PoolManager.Instance.GetFromPool<AnimationEffect<T>>(GetAnimationGameObject(animationTypeFX), position, rotation);

            if (flipHorizontal)
            {
                animationFx.transform.Rotate(0.0f, 180, 0.0f);
            }

            if (animationFx != null)
            {
                animationFx.Initialize(animationTypeFX);
                animationFx.SetActive(true);
            }
        }

        private GameObject GetAnimationGameObject(Enum typeFX)
            => typeFX switch
            {
                AbilityFXType => abilityFXPrefab.gameObject,
                DustType => dustPrefab.gameObject,
                _ => throw new NotImplementedException(),
            };
    }

}
