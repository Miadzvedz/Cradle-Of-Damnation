using Objects;
using System;
using UnityEngine;
using UnityEngine.UIElements;


namespace CoreSystem.CoreComponents.SensorDetectComponents
{
    public class LadderDetector : SensorDetectComponent
    {
        [SerializeField] private Grid grid;
        [SerializeField] public string targetTag;
        [SerializeField] public LayerMask targetLayer;

        [field: Header("CHECKING OFFSETS")]
        [SerializeField] public float firstOffset;
        [SerializeField] public float secondOffset;
        [SerializeField] public float thirdOffset;


        protected override Vector2 InitSensorPosition => 
            new Vector2(entityCollider.bounds.center.x, entityCollider.bounds.min.y);

        protected override string SensorName => nameof(LadderDetector);



        /*public bool TryGetLadderPosition(out Vector2 positionOnLadder)
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
            
        }*/

        public bool TryGetLadderOnBottom(out Ladder ladder)
        {
            ladder = new Ladder();
            bool isDetected = false;

            Collider2D under = Physics2D.OverlapPoint(new Vector2(InitSensorPosition.x, InitSensorPosition.y + firstOffset), targetLayer);
            Collider2D above = Physics2D.OverlapPoint(new Vector2(InitSensorPosition.x, InitSensorPosition.y + secondOffset), targetLayer);

            if (under == null && above != null)
            {   
                if (isDetected = above.CompareTag(targetTag))
                {
                    ladder.Set(above.bounds.max.y, above.bounds.min.y, above.bounds.center.x);
                }
            }

            return isDetected;
        }

        public bool TryGetLadderOnTop(out Ladder ladder)
        {
            ladder = new Ladder();
            bool isDetected = false;

            Collider2D under = Physics2D.OverlapPoint(new Vector2(InitSensorPosition.x, InitSensorPosition.y + firstOffset), targetLayer);
            Collider2D above = Physics2D.OverlapPoint(new Vector2(InitSensorPosition.x, InitSensorPosition.y + secondOffset), targetLayer);

            if (under != null && above == null)
            {
                if (isDetected = above.CompareTag(targetTag))
                {
                    ladder.Set(under.bounds.max.y, under.bounds.min.y, under.bounds.center.x);
                }
            }

            return isDetected;
        }

        public bool TryGetLadderOnCenter(out Ladder ladder)
        {
            ladder = new Ladder();
            bool isDetected = false;

            Collider2D top = Physics2D.OverlapPoint(new Vector2(InitSensorPosition.x, InitSensorPosition.y + thirdOffset), targetLayer);
            Collider2D bot = Physics2D.OverlapPoint(new Vector2(InitSensorPosition.x, InitSensorPosition.y + secondOffset), targetLayer);

            if (top != null && top != null)
            {
                if (isDetected = bot.CompareTag(targetTag) && top.CompareTag(targetTag))
                {
                    ladder.Set(bot.bounds.max.y, bot.bounds.min.y, bot.bounds.center.x);
                }
            }

            return isDetected;
        }


        protected override void DrawRay()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(new Vector2(InitSensorPosition.x, InitSensorPosition.y + firstOffset), 0.02f);
            Gizmos.DrawWireSphere(new Vector2(InitSensorPosition.x, InitSensorPosition.y + secondOffset), 0.02f);
            Gizmos.DrawWireSphere(new Vector2(InitSensorPosition.x, InitSensorPosition.y + thirdOffset), 0.02f);
        }
    }
}
