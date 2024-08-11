using UnityEngine;


namespace CoreSystem.CoreComponents.SensorDetectComponents
{
    public class LedgeDetector : SensorDetectComponent
    {
        [SerializeField] private float hitDistance;
        [SerializeField] private float positionOffsetY;
        [SerializeField] private float spanOfLedge;
        [SerializeField] public LayerMask targetLayer;
        [SerializeField] public Grid grid;

        protected override string SensorName => nameof(LedgeDetector);

        protected override Vector2 InitSensorPosition => 
            new Vector2(entityCollider.bounds.min.x, entityCollider.bounds.center.y);


        private RaycastHit2D LedgeHit => Physics2D.Raycast(
            new Vector2(sensor.position.x, sensor.position.y + positionOffsetY),
            Vector2.right * core.Physics.Flipping.FacingDirection,
            hitDistance,
            targetLayer);


        public bool IsLedgeDetect()
        {
            bool aboveIsEmpty = !Physics2D.Raycast(
                new Vector2(sensor.position.x, sensor.position.y + positionOffsetY + spanOfLedge),
                Vector2.right * core.Physics.Flipping.FacingDirection,
                hitDistance,
                targetLayer);

            bool betweenHitsIsEmpty = !Physics2D.Raycast(
                new Vector2(sensor.position.x, sensor.position.y + positionOffsetY),
                Vector2.up,
                spanOfLedge,
                targetLayer);

            return LedgeHit.collider != null
                && betweenHitsIsEmpty
                && aboveIsEmpty;
        }


        public bool TryGetLedgeCorner(out Vector2 ledgeCorner)
        {
            ledgeCorner = Vector2.zero;
            bool isDetected = IsLedgeDetect();

            if (isDetected)
            {
                Vector3Int cellPosition = grid.WorldToCell(LedgeHit.point);
                Vector3 centerOfCell = grid.GetCellCenterWorld(cellPosition);
                ledgeCorner.Set(centerOfCell.x, centerOfCell.y);
            }

            return isDetected;
        }


        protected override void DrawRay()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(new Vector2(sensor.position.x, sensor.position.y + positionOffsetY), new Vector2(hitDistance * core.Physics.Flipping.FacingDirection, 0)); //ledge ray 1
            Gizmos.DrawRay(new Vector2(sensor.position.x, sensor.position.y + positionOffsetY + spanOfLedge), new Vector2(hitDistance * core.Physics.Flipping.FacingDirection, 0)); //ledge ray 2
            Gizmos.DrawRay(new Vector2(sensor.position.x, sensor.position.y + positionOffsetY + spanOfLedge), new Vector2(0, - spanOfLedge)); //ledge ray 3
        }
    }
}
