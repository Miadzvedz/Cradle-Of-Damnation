using Managers;
using System;
using UnityEngine;

namespace Pool.ItemsPool
{
    public sealed class AfterImageSprite : PooledObject
    {
        [Range(0, 1)]
        [SerializeField]
        private float alphaBegin;
        private float alphaUpdate;

        [SerializeField]
        private float colorLooseRate;

        [SerializeField]
        private SpriteRenderer entitySpriteRenderer;

        private SpriteRenderer spriteRenderer;

        private Color color;


        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();   
        }

        private void OnEnable()
        {
            alphaUpdate = alphaBegin;
            color = spriteRenderer.color;
            spriteRenderer.sprite = entitySpriteRenderer.sprite;
        }

        private void Update()
        {
            alphaUpdate -= colorLooseRate * Time.deltaTime;
            spriteRenderer.color = new Color(color.r, color.g, color.b, alphaUpdate);
            if (spriteRenderer.color.a <= 0)
            {
                PoolManager.Instance.ReturnToPool(gameObject);
            }
        }

        public override void Get(GameObject obj)
        {
            obj.SetActive(false);
        }

        public override GameObject Create(Transform container)
        {
            gameObject.SetActive(false);
            return base.Create(container);
        }

        public void Initialize(SpriteRenderer spriteRendarer, float alphaBegin, float colorLooseRate)
        {
            this.entitySpriteRenderer = spriteRendarer;
            this.alphaBegin = alphaBegin;
            this.colorLooseRate = colorLooseRate;
        }
    }
}