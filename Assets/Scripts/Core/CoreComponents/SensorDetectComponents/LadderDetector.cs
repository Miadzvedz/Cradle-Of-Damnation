using UnityEngine;


namespace CoreSystem.CoreComponents.SensorDetectComponents
{
    public class LadderDetector : SensorDetectComponent
    {
        [SerializeField] private Grid grid;
        [SerializeField] public string targetTag;
        [SerializeField] public LayerMask targetLayer;

        [field: Header("CHECKING OFFSETS")]
        [SerializeField] public float upOutsidePoint;
        [SerializeField] public float upInsidePoint;
        [SerializeField] public float downInsidePoint;
        [SerializeField] public float downOutsidePoint;

        protected override Vector2 InitSensorPosition => entityCollider.bounds.center;
        protected override string SensorName => nameof(LadderDetector);

        private Vector2 UpOutsidePointPosition => new(sensor.position.x, sensor.position.y + upOutsidePoint);
        private Vector2 UpInsidePointPosition => new(sensor.position.x, sensor.position.y + upInsidePoint);
        private Vector2 DownInsidePointPosition => new(sensor.position.x, sensor.position.y + downInsidePoint);
        private Vector2 DownOutsidePointPosition => new(sensor.position.x, sensor.position.y + downOutsidePoint);

        private Collider2D UpOutsideHitPoint => Physics2D.OverlapPoint(UpOutsidePointPosition, targetLayer);
        private Collider2D UpInsideleHitPoint => Physics2D.OverlapPoint(UpInsidePointPosition, targetLayer);
        private Collider2D DownInsideHitPoint => Physics2D.OverlapPoint(DownInsidePointPosition, targetLayer);
        private Collider2D DownOutsideHitPoint => Physics2D.OverlapPoint(DownOutsidePointPosition, targetLayer);


        public bool TryGetVerticalМidOfLadder(out float midOfLadder, LadderPlace fromPlace)
        {
            midOfLadder = default;

            bool isDetected = IsLaderDetected(fromPlace);

            if (isDetected)
            {
                Vector3Int cellPosition = grid.WorldToCell(sensor.transform.position);
                Vector3 centerOfCell = grid.GetCellCenterWorld(cellPosition);
                midOfLadder = centerOfCell.x;
            }

            return isDetected;
        }

        public bool TryGetTopOfLadderPosition(out Vector2 position)
        {
            position = Vector2.zero;

            bool isDetected = !UpOutsideHitPoint && UpInsideleHitPoint;

            if (isDetected)
            {
                var rayCast = Physics2D.Raycast(UpOutsidePointPosition, Vector2.down, Mathf.NegativeInfinity, targetLayer);
                position = rayCast.point;
            }
            
            return isDetected;
        }

        public bool IsLaderDetected(LadderPlace place)
        {
            return place switch
            {
                LadderPlace.Top => DownOutsideHitPoint && !DownInsideHitPoint,
                LadderPlace.Mid => DownInsideHitPoint && UpInsideleHitPoint,
                LadderPlace.Bottom => !DownOutsideHitPoint && DownInsideHitPoint,
                _ => default
            };
        }

        public bool IsOnLadder() => UpInsideleHitPoint && DownInsideHitPoint;



        protected override void DrawRay()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(new Vector2(InitSensorPosition.x, InitSensorPosition.y + upOutsidePoint), 0.02f);
            Gizmos.DrawWireSphere(new Vector2(InitSensorPosition.x, InitSensorPosition.y + upInsidePoint), 0.02f);
            Gizmos.DrawWireSphere(new Vector2(InitSensorPosition.x, InitSensorPosition.y + downInsidePoint), 0.02f);
            Gizmos.DrawWireSphere(new Vector2(InitSensorPosition.x, InitSensorPosition.y + downOutsidePoint), 0.02f);
        }
    }

    public enum LadderPlace : byte
    {
        Top = 0,
        Mid = 1,
        Bottom = 2,
    }
}
