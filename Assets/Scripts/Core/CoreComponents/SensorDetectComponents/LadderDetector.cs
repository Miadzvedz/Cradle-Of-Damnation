using UnityEngine;


namespace CoreSystem.CoreComponents.SensorDetectComponents
{
    public class LadderDetector : SensorDetectComponent
    {
        [SerializeField] private Grid grid;
        [SerializeField] public string targetTag;
        [SerializeField] public LayerMask targetLayer;

        [field: Header("CHECKING OFFSETS")]
        [SerializeField] public float upPoinByY;
        [SerializeField] public float middlePoinByY;
        [SerializeField] public float downPointByY;


        protected override Vector2 InitSensorPosition => 
            new Vector2(entityCollider.bounds.center.x, entityCollider.bounds.min.y);

        protected override string SensorName => nameof(LadderDetector);


        private Vector2 UpPointPosition => new(sensor.position.x, sensor.position.y + upPoinByY);
        private Vector2 MiddlePointPosition => new(sensor.position.x, sensor.position.y + middlePoinByY);
        private Vector2 DownPointPosition => new(sensor.position.x, sensor.position.y + downPointByY);

        private Collider2D UpHitPoint => Physics2D.OverlapPoint(UpPointPosition, targetLayer);
        private Collider2D MiddleHitPoint => Physics2D.OverlapPoint(MiddlePointPosition, targetLayer);
        private Collider2D DownHitPoint => Physics2D.OverlapPoint(DownPointPosition, targetLayer);



        public bool TryGetMidOfLadder(out float midOfLadder, LadderPlace fromPlace)
        {
            midOfLadder = default;

            bool isDetected = IsLaderDetected(fromPlace);

            if (isDetected)
            {
                Vector2 detectedPosition = GetDetectedPosition();
                Vector3Int cellPosition = grid.WorldToCell(detectedPosition);
                Vector3 centerOfCell = grid.GetCellCenterWorld(cellPosition);
                midOfLadder = centerOfCell.x;
            }

            return isDetected;
        }


        private bool IsLaderDetected(LadderPlace place)
        {
            return place switch
            {
                LadderPlace.Top => DownHitPoint && !MiddleHitPoint,
                LadderPlace.Mid => UpHitPoint && MiddleHitPoint,
                LadderPlace.Bottom => !DownHitPoint && MiddleHitPoint,
                _ => default
            };
        }


        private Vector2 GetDetectedPosition() => DownHitPoint 
            ? DownPointPosition 
            : MiddlePointPosition;


        protected override void DrawRay()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(new Vector2(InitSensorPosition.x, InitSensorPosition.y + upPoinByY), 0.02f);
            Gizmos.DrawWireSphere(new Vector2(InitSensorPosition.x, InitSensorPosition.y + middlePoinByY), 0.02f);
            Gizmos.DrawWireSphere(new Vector2(InitSensorPosition.x, InitSensorPosition.y + downPointByY), 0.02f);
        }
    }

    public enum LadderPlace : byte
    {
        Top = 0,
        Mid = 1,
        Bottom = 2,
    }
}
