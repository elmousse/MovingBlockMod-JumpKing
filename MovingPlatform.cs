using System;
using System.Collections.Generic;
using EntityComponent;
using JumpKing;
using JumpKing.Level;
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
        private Sprite _sprite;
        
        private List<Waypoint> Waypoints { get; }
        public Point CurrentPosition { get; private set; }
        
        private MovingPlatform(
            int screen,
            Texture2D texture,
            List<Waypoint> waypoints)
        {
            Screen = screen;
            Waypoints = waypoints;
            CurrentPosition = Waypoints[0].Position;
            _sprite = Sprite.CreateSprite(texture);
        }

        public static MovingPlatform FromXmlData(MovingPlatformXml data)
        {
            var hitboxTexture = MovingPlatformLoader.LoadTexture(data.HitboxName, "hitboxes");
            var hitbox = MovingPlatformHitbox.FromTexture(hitboxTexture);
            
            var texture = MovingPlatformLoader.LoadTexture(data.TextureName, "textures");

            var waypoints = GetWaypointsFromData(data);
            
            var platform = new MovingPlatform(data.ScreenIndex, texture, waypoints);
            
            // plus tard, ajouter une factory pour les blocks
            for (var i = 0; i < hitbox.Height * hitbox.Width; i++)
            {
                var xPosition = (i % hitbox.Width) * 8;
                var yPosition = (i / hitbox.Width) * 8;
                
                if (hitbox.Hitbox[i].A == 0)
                {
                    continue;
                }
                platform.AddBlock(new MovingBlock(new Point(xPosition, yPosition), platform));
            }
            
            return platform;
        }
        
        private static List<Waypoint> GetWaypointsFromData(MovingPlatformXml data)
        {
            var waypoints = new List<Waypoint>();
            if (!(data.TotalTime == null || data.TotalTime == 0))
            {
                var fullLength = 0f;
                for (var i = 0; i < data.Positions.Count - 1; i++)
                {
                    var position1 = data.Positions[i];
                    var position2 = data.Positions[i + 1];
                    var length = Math.Abs(position1.X - position2.X) + Math.Abs(position1.Y - position2.Y);
                    fullLength += length;
                }
                var timePerUnit = (float)data.TotalTime / fullLength;
                var currentTime = 0f;
                for (var i = 0; i < data.Positions.Count - 1; i++)
                {
                    var position1 = data.Positions[i];
                    var position2 = data.Positions[i + 1];
                    var length = Math.Abs(position1.X - position2.X) + Math.Abs(position1.Y - position2.Y);
                    var time = length * timePerUnit;
                    waypoints.Add(new Waypoint(position1.Position, currentTime));
                    currentTime += time;
                }
                waypoints.Add(new Waypoint(data.Positions[data.Positions.Count - 1].Position, (float)data.TotalTime));
            }
            return waypoints;
        }
        
        private void AddBlock(MovingBlock block)
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
            if (LevelManager.CurrentScreen.GetIndex0() != Screen)
            {
                return;
            }
            _sprite.Draw(Camera.TransformVector2(CurrentPosition.ToVector2()));
        }
        
        private Point GetPlatformPosition()
        {
            var timeSpan = (TimeSpan)AchievementManagerWrapper.GetTimeSpan();
            var time = (float)timeSpan.TotalSeconds;
            
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