using UnityEngine;


namespace CoreSystem.CoreComponents.SensorDetectComponents
{
    
    public class GrabWallDetector : SensorDetectComponent
    {
        [SerializeField] private float span;
        [SerializeField] private float hitDistance;
        [SerializeField] public string targetTag;
        [SerializeField] public LayerMask targetLayer;

        protected override string SensorName => nameof(GrabWallDetector);

        protected override Vector2 InitSensorPosition => entityCollider.bounds.center;


        public RaycastHit2D WallHitUp => Physics2D.Raycast(
            new Vector2(sensor.position.x, sensor.position.y + span),
            Vector2.right * core.Physics.Flipping.FacingDirection,
            hitDistance,
            targetLayer);

        public RaycastHit2D WallHitDown => Physics2D.Raycast(
            new Vector2(sensor.position.x, sensor.position.y - span),
            Vector2.right * core.Physics.Flipping.FacingDirection,
            hitDistance,
            targetLayer);


        public bool IsGrabWallDetect()
        {
            if (WallHitUp.collider is null || WallHitDown.collider is null) return false;

            return WallHitUp.collider.CompareTag(targetTag) && WallHitDown.collider.CompareTag(targetTag);
        }

        public bool TryGetGrabWallPosition(out Vector2 wallPosition)
        {
            wallPosition = Vector2.zero;
            bool isDetected = IsGrabWallDetect();

            if (isDetected)
            {
                wallPosition = WallHitUp.point;
            }

            return isDetected;
        }

        protected override void DrawRay()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(new Vector2(sensor.position.x, sensor.position.y + span), new Vector2(hitDistance * core.Physics.Flipping.FacingDirection, 0));
            Gizmos.DrawRay(new Vector2(sensor.position.x, sensor.position.y - span), new Vector2(hitDistance * core.Physics.Flipping.FacingDirection, 0));
        }
    }
}
