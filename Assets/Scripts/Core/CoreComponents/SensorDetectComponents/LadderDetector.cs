using Triggers;
using UnityEngine;


namespace CoreSystem.CoreComponents.SensorDetectComponents
{
    public class LadderDetector : SensorDetectComponent
    {
        [SerializeField] private Grid grid;
        [SerializeField] private float touchingDistance;

        private LadderTrigger ladderTrigger;

        public bool IsOnLadder { get; private set; }
        public float CenterByHorizontOfLadder {  get; private set; }


        protected override Vector2 InitSensorPosition => 
            new Vector2(entityCollider.bounds.center.x, entityCollider.bounds.center.y);

        protected override string SensorName => nameof(LadderDetector);


        protected override void Awake()
        {
            base.Awake();

            ladderTrigger = FindAnyObjectByType<LadderTrigger>();

            ladderTrigger.OnEnter += OnEnterLadder;
            ladderTrigger.OnExit += OnExitLadder;
        }


        private void OnEnterLadder()
        {
            Vector2 pointPosition = core.Physics.Flipping.IsLeftDirection()
                ? new Vector2(entityCollider.bounds.min.x, entityCollider.bounds.center.y)
                : new Vector2(entityCollider.bounds.max.x, entityCollider.bounds.center.y);

            Vector3 detectedPoint = entityCollider.ClosestPoint(pointPosition);
            Vector3Int cellPosition = grid.WorldToCell(detectedPoint);
            Vector3 centerOfCell = grid.GetCellCenterWorld(cellPosition);

            CenterByHorizontOfLadder = centerOfCell.x;

            ladderTrigger.OnStay += OnStayLadder;
        }

        private void OnStayLadder()
        {
            IsOnLadder = touchingDistance >= Mathf.Abs(CenterByHorizontOfLadder - entityCollider.transform.position.x);
            //Debug.Log(IsOnLadder);
        }

        private void OnExitLadder()
        {
            ladderTrigger.OnStay -= OnStayLadder;
        }


        protected override void DrawRay()
        {
            Gizmos.color = Color.red;

        }
    }
}
