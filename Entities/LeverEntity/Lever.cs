using System.Collections.Generic;
using System.Linq;
using EntityComponent;
using JumpKing;
using JumpKing.API;
using JumpKing.Level;
using JumpKing.Player;
using Microsoft.Xna.Framework.Graphics;
using MovingBlockMod.Blocks;

namespace MovingBlockMod.Entities.LeverEntity
{
    public class Lever : Entity
    {
        public string Id { get; private set; }
        public bool State { get; private set; }
        public List<LeverBlock> Blocks { get; private set; }
        private List<MovingPlatformEntity.MovingPlatform> _platforms = new List<MovingPlatformEntity.MovingPlatform>();
        private IActivationType _activation;
        
        private List<Texture2D> _textures;
        private Sprite _sprite;
        private bool _onLeverCache;
        
        public Lever(
            IActivationType activation,
            string id,
            bool startingState)
        {
            _activation = activation;
            Id = id;
            Blocks = new List<LeverBlock>();
            State = startingState;
        }
        
        public void AddBlock(LeverBlock block)
        {
            Blocks.Add(block);
        }
        
        public void AddPlatform(MovingPlatformEntity.MovingPlatform platform)
        {
            _platforms.Add(platform);
        }
        
        private LeverTrigger GetTrigger()
        {
            var player = EntityManager.instance.Find<PlayerEntity>();
            var hitbox = player.m_body.GetHitbox();
            ICollisionQuery collisionQuery = LevelManager.Instance;
            var collisionInfo = collisionQuery.GetCollisionInfo(hitbox);
            if (collisionInfo == null)
            {
                return LeverTrigger.Out;
            }
            var blocks = collisionInfo.GetCollidedBlocks<LeverBlock>().Intersect(Blocks).ToList();
            
            var onLever = blocks.Any();
            var trigger = LeverTrigger.Out;
            if (onLever && !_onLeverCache)
            {
                trigger = LeverTrigger.Enter;
            }
            else if (!onLever && _onLeverCache)
            {
                trigger = LeverTrigger.Exit;
            }
            else if (onLever)
            {
                trigger = LeverTrigger.On;
            }
            _onLeverCache = onLever;
            return trigger;
        }
        
        private void UpdateState(LeverTrigger trigger)
        {
            State = _activation.GetState(trigger, State);
        }

        public void Update()
        {
            var trigger = GetTrigger();
            UpdateState(trigger);
        }
    }
    
    public enum LeverTrigger
    {
        Enter,
        Exit,
        On,
        Out
    }
}