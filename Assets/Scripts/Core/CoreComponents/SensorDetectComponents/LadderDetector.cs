using Triggers;
using UnityEngine;


namespace CoreSystem.CoreComponents.SensorDetectComponents
{
    public class LadderDetector : SensorDetectComponent
    {
        [SerializeField] private Grid grid;
        [SerializeField] private float touchingDistance;

        private LadderTrigger ladderTrigger;

        private bool isOnLadder;


        protected override Vector2 InitSensorPosition => 
            new Vector2(entityCollider.bounds.center.x, entityCollider.bounds.center.y);

        protected override string SensorName => nameof(LadderDetector);


        protected override void Awake()
        {
            base.Awake();

            ladderTrigger = FindAnyObjectByType<LadderTrigger>();

            if (ladderTrigger != null )
            {
                ladderTrigger.OnEnter += OnEnterLadder;
                ladderTrigger.OnExit += OnExitLadder;
            }
        }

        public bool TryGetLadderPosition(out Vector2 positionOnLadder)
        {
            positionOnLadder = Vector2.zero;
            if (isOnLadder)
            {
                Vector2 pointPosition = core.Physics.Flipping.IsLeftDirection()
                    ? new Vector2(entityCollider.bounds.min.x, entityCollider.bounds.center.y)
                    : new Vector2(entityCollider.bounds.max.x, entityCollider.bounds.center.y);

                Vector3 detectedPoint = entityCollider.ClosestPoint(pointPosition);
                Vector3Int cellPosition = grid.WorldToCell(detectedPoint);
                Vector3 centerOfCell = grid.GetCellCenterWorld(cellPosition);

                positionOnLadder = new Vector2(centerOfCell.x, entityCollider.transform.position.y);

            }
            return isOnLadder;
        }


        private void OnEnterLadder()
        {
            isOnLadder = true;
        }


        private void OnExitLadder()
        {
            isOnLadder = false;
        }


        protected override void DrawRay()
        {
            Gizmos.color = Color.red;
        }
    }
}
