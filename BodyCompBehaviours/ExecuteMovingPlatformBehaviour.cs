using System.Collections.Generic;
using System.Linq;
using EntityComponent;
using JumpKing.API;
using JumpKing.BodyCompBehaviours;
using JumpKing.Level;
using JumpKing.Player;
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
                       (hitbox.Bottom == blockRect.Top && hitbox.Right > blockRect.Left && hitbox.Left < blockRect.Right);
            });
            
            if (collidedBlock != null)
            {
                player.m_body.Position.Y += collidedBlock.ParentPlatform.Velocity.Y;
                player.m_body.Position.X += collidedBlock.ParentPlatform.Velocity.X;
            }
            else if (preCollidedBlock != null)
            {
                player.m_body.Position.Y += preCollidedBlock.ParentPlatform.Velocity.Y;
                player.m_body.Position.X += preCollidedBlock.ParentPlatform.Velocity.X;
            }
            
            
            return true;
        }
    }
}