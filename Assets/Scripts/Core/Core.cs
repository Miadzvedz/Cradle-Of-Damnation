using UnityEngine;
using Interfaces;
using CoreSystem.CoreComponents;


namespace CoreSystem
{  
    public sealed class Core : MonoBehaviour, ILogicUpdate
    {
        public PhysicsManipulation Physics { get; private set; }
        public SensorDetect Sensor { get; private set; }
        public VisualFx VisualFx { get; private set; }
        public CollisionManipulation Collision { get; private set; }


        public void Awake()
        {
            Sensor = GetComponentInChildren<SensorDetect>();
            Physics = GetComponentInChildren<PhysicsManipulation>();
            VisualFx = GetComponentInChildren<VisualFx>();
            Collision = GetComponentInChildren<CollisionManipulation>();
        }

        public void LogicUpdate()
        {
            Physics.LogicUpdate();
            VisualFx.LogicUpdate();
            Collision.LogicUpdate();
            Sensor.LogicUpdate();
        }
    }
}