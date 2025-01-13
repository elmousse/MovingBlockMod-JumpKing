using JumpKing.API;
using JumpKing.BodyCompBehaviours;
using JumpKing.Level;
using MovingBlockMod.Blocks;

namespace MovingBlockMod.BlockBehaviours
{
    /// <summary>
    /// An implementation of <see cref="IBlockBehaviour"/> representing how the <see cref="MovingBlock"/> will
    /// affect the player during runtime
    /// </summary>
    public class MovingBlockBehaviour : IBlockBehaviour
    {
        /// <inheritdoc/>
        public float BlockPriority => 2f;

        /// <inheritdoc/>
        public bool IsPlayerOnBlock { get; set; }

        /// <inheritdoc/>
        public bool AdditionalXCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext)
        {
            return false;
        }

        /// <inheritdoc/>
        public bool AdditionalYCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext)
        {
            return false;
        }

        /// <inheritdoc/>
        public float ModifyXVelocity(float inputXVelocity, BehaviourContext behaviourContext)
        {
            return inputXVelocity;
        }

        /// <inheritdoc/>
        public float ModifyYVelocity(float inputYVelocity, BehaviourContext behaviourContext)
        {
            return inputYVelocity;
        }

        /// <inheritdoc/>
        public float ModifyGravity(float inputGravity, BehaviourContext behaviourContext)
        {
            return inputGravity;
        }

        /// <inheritdoc/>
        public bool ExecuteBlockBehaviour(BehaviourContext behaviourContext)
        {
            if (behaviourContext?.CollisionInfo?.PreResolutionCollisionInfo == null)
            {
                return true;
            }
            IsPlayerOnBlock = behaviourContext.CollisionInfo.PreResolutionCollisionInfo.IsCollidingWith<MovingBlock>();
            return true;
        }
    }
}