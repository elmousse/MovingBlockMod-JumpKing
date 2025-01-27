using System;
using System.Collections.Generic;
using System.Linq;
using BehaviorTree;
using EntityComponent;
using EntityComponent.BT;
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
            var bodyComp = behaviourContext.BodyComp;
            var hitbox = bodyComp.GetHitbox();
            
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
            
            var potentialCollidedBlock = CheckPotentialCollision(bodyComp, velocityBlockReference);

            if (IsCollidingCriticalArea(bodyComp, velocityBlockReference) && potentialCollidedBlock != 0f)
            {
                bodyComp.Position.X = 200;
                /*var player = EntityManager.instance.Find<PlayerEntity>();
                var behaviourTreeComponent = player.GetComponent<BehaviorTreeComp>();
                var behaviourTree = behaviourTreeComponent.GetRaw();
                var jumpState = behaviourTree.FindNode<JumpState>();
                jumpState.ResetResult();*/
                return true;
            }

            bodyComp.Position.X += Math.Sign(velocityBlockReference.ParentPlatform.Velocity.X) * (Math.Abs(velocityBlockReference.ParentPlatform.Velocity.X) - potentialCollidedBlock);
            
            bodyComp.Position.Y += velocityBlockReference.ParentPlatform.Velocity.Y;
            
            return true;
        }

        private static bool IsCollidingCriticalArea(BodyComp bodyComp, MovingBlock block)
        {
            var preHitbox = bodyComp.GetHitbox();
            preHitbox.Offset(block.ParentPlatform.Velocity);
            var collisionInfo = LevelManager.GetCollisionInfo(preHitbox);

            if (collisionInfo == null)
            {
                return false;
            }
            if (!collisionInfo.IsCollidingWith<CriticalAreaBlock>())
            {
                return false;
            }
            return true;
        }
        
        private float CheckPotentialCollision(BodyComp bodyComp, MovingBlock block)
        {
            var preHitbox = bodyComp.GetHitbox();
            preHitbox.Offset(block.ParentPlatform.Velocity);
            var collisionInfo = LevelManager.GetCollisionInfo(preHitbox);
            
            if (collisionInfo == null)
                return 0f;
            if (!collisionInfo.IsCollidingWith<BoxBlock>())
                return 0f;
            
            var intersection = new Rectangle();
            collisionInfo.GetCollidedBlocks<BoxBlock>().FirstOrDefault()?.Intersects(preHitbox, out intersection);
            return intersection.Width;
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            
            /*var prePreHitbox = preHitbox;
            prePreHitbox.Offset(new Vector2(block.ParentPlatform.Velocity.X, block.ParentPlatform.Velocity.Y));
            var collisionInfoSlope = LevelManager.GetCollisionInfo(prePreHitbox);
            if (collisionInfoSlope == null)
                return 0;
            if (collisionInfoSlope.IsCollidingWith<SlopeBlock>() || collisionInfo.IsCollidingWith<SlopeBlock>())
            {
                return 2;
            }*/
            
            /*var prePreHitbox1 = preHitbox;
            prePreHitbox1.Offset(new Vector2(block.ParentPlatform.Velocity.X - 3f, block.ParentPlatform.Velocity.Y));
            var collisionInfoSlope1 = LevelManager.GetCollisionInfo(prePreHitbox1);
            if (collisionInfoSlope1 == null)
                return 0;
            if (collisionInfoSlope1.IsCollidingWith<SlopeBlock>() || collisionInfo.IsCollidingWith<SlopeBlock>())
            {
                return 3;
            }
            
            var prePreHitbox2 = preHitbox;
            prePreHitbox2.Offset(new Vector2(block.ParentPlatform.Velocity.X + 3f, block.ParentPlatform.Velocity.Y));
            var collisionInfoSlope2 = LevelManager.GetCollisionInfo(prePreHitbox2);
            if (collisionInfoSlope2 == null)
                return 0;
            if (collisionInfoSlope2.IsCollidingWith<SlopeBlock>() || collisionInfo.IsCollidingWith<SlopeBlock>())
            {
                return 4;
            }*/
        }
    }
}