using UnityEngine;


namespace CoreSystem.CoreComponents.SensorDetectComponents
{
    [RequireComponent(typeof(SensorDetect))]
    public abstract class SensorDetectComponent : MonoBehaviour
    {
        [SerializeField]
        protected bool isShowRay;

        protected Core core;
        protected CapsuleCollider2D entityCollider;
        protected Transform sensor;


        protected abstract Vector2 InitSensorPosition { get; }
        protected abstract string SensorName { get; }


        protected virtual void Awake()
        {
            this.core = GetComponent<SensorDetect>().Core;
            entityCollider = core.GetComponentInParent<CapsuleCollider2D>();
            CreateSensor();          
        }


        private void CreateSensor()
        {
            GameObject obj = new GameObject(SensorName);
            obj.transform.parent = transform;
            obj.transform.position = InitSensorPosition;
            sensor = obj.transform;
        }


        protected abstract void DrawRay();

        private void DrawGizmos()
        {
            if (!Application.isPlaying) return;
            if (!isShowRay) return;

            DrawRay();
        }

        private void OnDrawGizmos()
        {
            DrawGizmos();
        }
    }
}