using System.Collections.Generic;
using System.Linq;
using EntityComponent;
using JumpKing.API;
using JumpKing.BodyCompBehaviours;
using JumpKing.Level;
using JumpKing.Player;
using Microsoft.Xna.Framework;
using MovingBlockMod.Blocks;

namespace MovingBlockMod.BodyCompBehaviours
{
    public class ExecuteMovingPlatformBehaviour : IBodyCompBehaviour
    {
        public bool ExecuteBehaviour(BehaviourContext behaviourContext)
        {
            var player = EntityManager.instance.Find<PlayerEntity>();
            var hitbox = player.m_body.GetHitbox();
            
            var currentPlayerTopScreen = ((hitbox.Top % 360 + 360) % 360 - hitbox.Top) / 360;
            var currentPlayerBottomScreen = ((hitbox.Bottom % 360 + 360) % 360 - hitbox.Bottom) / 360;
            var screens = currentPlayerTopScreen == currentPlayerBottomScreen 
                ? new[] { currentPlayerTopScreen } 
                : new[] { currentPlayerTopScreen, currentPlayerBottomScreen };
            
            var preMovingBlockOnScreen = new List<MovingBlock>();
            foreach (var screen in screens)
            {
                preMovingBlockOnScreen.AddRange(MovingPlatformManager.Instance.GetMovingBlocksOnScreen(screen));
            }
            
            var preCollidedBlock = preMovingBlockOnScreen.FirstOrDefault(block => 
            {
                var blockRect = block.GetRect();
                return blockRect.Intersects(hitbox) ||
                       (hitbox.Bottom == blockRect.Top && hitbox.Right > blockRect.Left && hitbox.Left < blockRect.Right);
            });
            
            LeverManager.Instance.UpdateAllLevers();
            MovingPlatformManager.Instance.UpdateAllPlatforms();

            var movingBlockOnScreen = new List<MovingBlock>();
            foreach (var screen in screens)
            {
                movingBlockOnScreen.AddRange(MovingPlatformManager.Instance.GetMovingBlocksOnScreen(screen));
            }
            
            var collidedBlock = movingBlockOnScreen.FirstOrDefault(block => 
            {
                var blockRect = block.GetRect();
                return blockRect.Intersects(hitbox) ||
                       (hitbox.Bottom == blockRect.Top && hitbox.Right > blockRect.Left && hitbox.Left < blockRect.Right) // ||
                    // (hitbox.Right == blockRect.Left && hitbox.Bottom > blockRect.Top && hitbox.Top < blockRect.Bottom) ||
                    // (hitbox.Right == blockRect.Left && hitbox.Bottom > blockRect.Top && hitbox.Top < blockRect.Bottom)
                    ;
            });

            MovingBlock velocityBlockReference;
            
            if (collidedBlock != null)
            {
                velocityBlockReference = collidedBlock;
            }
            else if (preCollidedBlock != null)
            {
                velocityBlockReference = preCollidedBlock;
            }
            else
            {
                return true;
            }
            
            player.m_body.Position.Y += velocityBlockReference.ParentPlatform.Velocity.Y;
            
            if (!CheckPotentialWallCollision(hitbox, velocityBlockReference))
            {
                player.m_body.Position.X += velocityBlockReference.ParentPlatform.Velocity.X;
            }
            
            return true;
        }
        
        private bool CheckPotentialWallCollision(Rectangle hitbox, MovingBlock block)
        {
            var preHitbox = hitbox;
            preHitbox.Offset(block.ParentPlatform.Velocity);
            var collisionInfo = LevelManager.GetCollisionInfo(preHitbox);
            if (collisionInfo == null)
                return false;
            if (collisionInfo.IsCollidingWith<BoxBlock>())
            {
                return true;
            }
            
            var prePreHitbox = preHitbox;
            prePreHitbox.Offset(block.ParentPlatform.Velocity);
            var collisionInfo2 = LevelManager.GetCollisionInfo(prePreHitbox);
            if (collisionInfo2 == null)
                return false;
            if (collisionInfo2.IsCollidingWith<SlopeBlock>() || collisionInfo.IsCollidingWith<SlopeBlock>())
            {
                return true;
            }
            return false;
        }
    }
}