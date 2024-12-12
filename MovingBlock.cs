using JumpKing.Level;
using Microsoft.Xna.Framework;

namespace MovingBlockMod
{
    public class MovingBlock : IBlock, IBlockDebugColor
    {
        private Rectangle _collider;
        private readonly Point _platformRelativePosition;
        private readonly MovingPlatform _parentPlatform;
        
        public Color DebugColor => new Color(233, 233, 233);
        
        public MovingBlock(
            Point platformRelativePosition,
            MovingPlatform parentPlatform)
        {
            _parentPlatform = parentPlatform;
            _platformRelativePosition = platformRelativePosition;
            var blockPosition = parentPlatform.CurrentPosition + platformRelativePosition;
            _collider = new Rectangle(blockPosition, new Point(8, 8));
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
            return BlockCollisionType.Collision_Blocking;
        }
        
        public void UpdatePosition()
        {
            var blockPosition = _parentPlatform.CurrentPosition + _platformRelativePosition;
            _collider.X = blockPosition.X;
            _collider.Y = blockPosition.Y;
        }
    }
}