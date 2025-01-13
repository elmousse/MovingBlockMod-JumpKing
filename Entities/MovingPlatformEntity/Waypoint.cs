using Microsoft.Xna.Framework;

namespace MovingBlockMod.Entities.MovingPlatform
{
    public class Waypoint
    {
        public int X { get; }
        public int Y { get; }
        
        public Point Position => new Point(X, Y);
        
        // Time from the start of the platform's movement
        public float Time { get; }

        public Waypoint(int x, int y, float time)
        {
            X = x;
            Y = y;
            Time = time;
        }
        
        public Waypoint(Point position, float time)
        {
            X = position.X;
            Y = position.Y;
            Time = time;
        }
    }
}