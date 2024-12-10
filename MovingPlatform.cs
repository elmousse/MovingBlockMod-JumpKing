using System;
using System.Collections.Generic;
using EntityComponent;
using JumpKing;
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
        private Texture2D Texture { get; }

        public Point Position { get; private set; }
        private Point _startPosition;
        
        private MovingPlatform(
            int screen,
            Texture2D texture,
            Point startPosition)
        {
            Screen = screen;
            Texture = texture;
            _startPosition = startPosition;
            Position = startPosition;
        }

        public static MovingPlatform FromXmlData(MovingPlatformXml data)
        {
            var hitboxTexture = MovingPlatformLoader.LoadTexture(data.HitboxName, "hitboxes");
            var hitbox = MovingPlatformHitbox.FromTexture(hitboxTexture);
            
            var texture = MovingPlatformLoader.LoadTexture(data.TextureName, "textures");
            
            var startPosition = data.Positions[0].Position;
            
            var platform = new MovingPlatform(data.ScreenIndex, texture, startPosition);
            
            // plus tard ajouter une factory pour les blocks
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
            Position = UpdatePlatformPosition();
            foreach (var block in _blocks)
            {
                block.UpdatePosition();
            }
        }
        
        public override void Draw() 
        {
            if (Texture == null)
            {
                return;
            }
            var spriteBatch = Game1.spriteBatch;
            spriteBatch.Draw(Texture, Position.ToVector2(), Color.White);
        }
        
        private Point UpdatePlatformPosition()
        {
            var timeSpan = (TimeSpan)AchievementManagerWrapper.GetTimeSpan();
            var time = (float)timeSpan.TotalSeconds;
            var x = (float)( _startPosition.X + Math.Abs( 200 * ( time / 10 - Math.Floor( time / 10 ) - 0.5 )));
            return new Point((int)x, _startPosition.Y);
        }
    }
}