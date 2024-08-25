using UnityEngine;

namespace Objects
{
    public struct Ladder
    {
        public float TopY { get; set; }
        public float BotY { get; set; }
        public float CenterX { get; set; }

        public Ladder(float topY, float botY, float centerX)
        {
            TopY = topY;
            BotY = botY;
            CenterX = centerX;
        }

        public void Set(float topY, float botY,float centerX)
        {
            TopY = topY;
            BotY = botY;
            CenterX = centerX;
        }

        override public string ToString() =>
            $"topY: {TopY}, botY: {BotY}, centerX: {CenterX}";
        
    }
}
