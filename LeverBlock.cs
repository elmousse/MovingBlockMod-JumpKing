using System.Collections.Generic;
using JumpKing.Level;
using Microsoft.Xna.Framework;

namespace MovingBlockMod
{
    public class LeverBlock : IBlock, IBlockDebugColor
    {
        public Color DebugColor => new Color(0, 210, 180);
        
        private Rectangle _collider;
        private readonly Lever _parentLever;
        public readonly int Screen;

        public LeverBlock(
            Rectangle collider,
            Lever parentLever,
            int screen)
        {
            _parentLever = parentLever;
            _collider = collider;
            Screen = screen;
        }
        
        public Rectangle GetRect()
        {
            return _collider;
        }
        
        public BlockCollisionType Intersects(Rectangle playerHitbox, out Rectangle intersection)
        {
            if (!_collider.Intersects(playerHitbox))
            {
                intersection = Rectangle.Empty;
                return BlockCollisionType.NoCollision;
            }
            
            intersection = Rectangle.Intersect(playerHitbox, _collider);
            return BlockCollisionType.Collision_NonBlocking;
        }
    }
}