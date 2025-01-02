using System;
using System.Collections.Generic;
using EntityComponent;
using JumpKing;
using JumpKing.Level;
using JumpKing.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MovingBlockMod.XmlData;

namespace MovingBlockMod
{
    public class MovingPlatform : Entity
    {
        private readonly List<MovingBlock> _blocks = new List<MovingBlock>();
        public List<MovingBlock> Blocks => _blocks;
        public int Screen { get; }
        private readonly Sprite _sprite;
        
        private List<Waypoint> Waypoints { get; }
        public Point CurrentPosition { get; private set; }
        private Point _textureOffset { get; }
        
        public MovingPlatform(
            int screen,
            Texture2D texture,
            List<Waypoint> waypoints,
            Point textureOffset)
        {
            Screen = screen;
            Waypoints = waypoints;
            CurrentPosition = Waypoints[0].Position;
            _sprite = Sprite.CreateSprite(texture);
            _textureOffset = textureOffset;
        }
        
        public void AddBlock(MovingBlock block)
        {
            _blocks.Add(block);
        }
        
        /*protected override void Update(float delta)
        {
            Position = UpdatePlatformPosition();
            foreach (var block in _blocks)
            {
                block.UpdatePosition();
            }
        }*/
        
        public void Update1()
        {
            CurrentPosition = GetPlatformPosition();
            foreach (var block in _blocks)
            {
                block.UpdatePosition();
            }
        }
        
        public override void Draw()
        {
            if (EntityManager.instance.Find<PlayerEntity>() == null
                || LevelManager.CurrentScreen.GetIndex0() != Screen) return;
            _sprite.Draw(Camera.TransformVector2((CurrentPosition - _textureOffset).ToVector2()));
        }
        
        private Point GetPlatformPosition()
        {
            var time = (float)((TimeSpan)AchievementManagerWrapper.GetTimeSpan()).TotalSeconds;
            
            var modTime = time % Waypoints[Waypoints.Count - 1].Time;
            
            for (var i = 0; i < Waypoints.Count - 1; i++)
            {
                if (!(modTime >= Waypoints[i].Time && modTime < Waypoints[i + 1].Time))
                {
                    continue;
                }
                var time1 = Waypoints[i].Time;
                var time2 = Waypoints[i + 1].Time;
                var timeDiff = time2 - time1;
                var timeIntoSegment = modTime - time1;
                var timeRatio = timeIntoSegment / timeDiff;
                var x = Waypoints[i].X + (Waypoints[i + 1].X - Waypoints[i].X) * timeRatio;
                var y = Waypoints[i].Y + (Waypoints[i + 1].Y - Waypoints[i].Y) * timeRatio;
                return new Point((int)x, (int)y);
            }
            return Point.Zero;
        }
    }
}