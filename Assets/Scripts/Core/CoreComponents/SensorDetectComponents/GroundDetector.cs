using UnityEngine;


namespace CoreSystem.CoreComponents.SensorDetectComponents
{
    public class GroundDetector : SensorDetectComponent
    {
        [SerializeField] private float hitDistance;
        [SerializeField] public string platformTag;
        [SerializeField] public string oneWayPlatformTag;
        [SerializeField] public LayerMask targetLayer;

        private float inactiveGroundSensorDistance;

        protected override string SensorName => nameof(GroundDetector);

        protected override Vector2 InitSensorPosition => entityCollider.bounds.center;


        protected override void Awake()
        {
            base.Awake();

            inactiveGroundSensorDistance = sensor.position.y - entityCollider.bounds.min.y;
        }


        public RaycastHit2D GroundHit => Physics2D.Raycast(
            sensor.position,
            Vector2.down,
            Mathf.NegativeInfinity,
            targetLayer);

        public bool IsGroundDetect()
            => hitDistance >= GroundHit.distance;

        public bool IsPlatformDetect()
            => GroundHit.collider.CompareTag(platformTag)
            && hitDistance >= GroundHit.distance;

        public bool IsOneWayPlatformDetect()
            => GroundHit.collider.CompareTag(oneWayPlatformTag)
            && hitDistance >= GroundHit.distance
            && inactiveGroundSensorDistance <= GroundHit.distance;

        public float GetGroundSlopeAngle()
            => Vector2.Angle(GroundHit.normal, Vector2.up);

        public bool IsGroundSlope()
            => GetGroundSlopeAngle() != default;

        public Vector2 GetGroundPerperdicular()
            => Vector2.Perpendicular(GroundHit.normal).normalized;

        
        protected override void DrawRay()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(sensor.position, new Vector2(0, -hitDistance));
        }
    }
}
