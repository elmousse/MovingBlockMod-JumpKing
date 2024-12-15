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
        
        private MovingPlatform(
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

        public static MovingPlatform FromXmlData(MovingPlatformXml data)
        {
            var hitboxTexture = MovingPlatformLoader.LoadTexture(data.HitboxName, "hitboxes");
            var hitbox = MovingPlatformHitbox.FromTexture(hitboxTexture);
            
            var texture = MovingPlatformLoader.LoadTexture(data.TextureName, "textures");

            var waypoints = data.GetWaypointsFromData();
            
            if (waypoints.Count == 0) 
                return null;
            
            var textureOffset = new Point(data.TextureOffsetX ?? 0, data.TextureOffsetY ?? 0);
            
            var platform = new MovingPlatform(data.ScreenIndex, texture, waypoints, textureOffset);
            
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
            if (EntityManager.instance.Find<PlayerEntity>() == null)
            {
                return;
            }
            if (LevelManager.CurrentScreen.GetIndex0() != Screen)
            {
                return;
            }
            _sprite.Draw(Camera.TransformVector2((CurrentPosition - _textureOffset).ToVector2()));
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