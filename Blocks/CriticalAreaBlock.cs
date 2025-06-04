using JumpKing.Level;
using Microsoft.Xna.Framework;
using MovingBlockMod.Entities;

namespace MovingBlockMod.Blocks
{
    public class CriticalAreaBlock : IBlock, IBlockDebugColor
    {
        public readonly MovingPlatform ParentPlatform;
        private Rectangle _collider;
        public readonly int SreenIndex;
        public Color DebugColor => new Color(255, 148, 96);

        public CriticalAreaBlock(
            Rectangle collider,
            MovingPlatform parentPlatform,
            int screen)
        {
            ParentPlatform = parentPlatform;
            _collider = collider;
            SreenIndex = screen;
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