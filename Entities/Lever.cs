using System.Collections.Generic;
using System.Linq;
using EntityComponent;
using JumpKing;
using JumpKing.API;
using JumpKing.Level;
using JumpKing.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MovingBlockMod.Blocks;
using MovingBlockMod.Entities.LeverComponents;

namespace MovingBlockMod.Entities
{
    public class Lever : Entity
    {
        public string Id { get; private set; }
        public bool State { get; private set; }
        private IActivationType Activation { get; set; }
        public List<LeverZone> Zones { get; private set; } = new List<LeverZone>();
        public List<LeverBlock> Blocks => Zones.Select(zone => zone.Block).ToList();
        private List<MovingPlatform> _platforms = new List<MovingPlatform>();
        
        private bool _onLeverCache;
        
        public Lever(
            IActivationType activation,
            string id,
            bool startingState)
        {
            Activation = activation;
            Id = id;
            State = startingState;
        }
        
        public void AddZone(LeverZone zone)
        {
            Zones.Add(zone);
        }
        
        public void AddPlatform(MovingPlatform platform)
        {
            _platforms.Add(platform);
        }
        
        private LeverTrigger GetTrigger()
        {
            var player = EntityManager.instance.Find<PlayerEntity>();
            var hitbox = player.m_body.GetHitbox();
            ICollisionQuery levelManager = LevelManager.Instance;
            var collisionInfo = levelManager.GetCollisionInfo(hitbox);
            if (collisionInfo == null)
            {
                return LeverTrigger.Out;
            }

            var collidedBlocks = collisionInfo.GetCollidedBlocks<LeverBlock>().Intersect(Blocks).ToList();
            
            var onLever = collidedBlocks.Any();
            LeverTrigger trigger;

            switch (onLever)
            {
                case true when !_onLeverCache:
                    trigger = LeverTrigger.Enter;
                    break;
                case false when _onLeverCache:
                    trigger = LeverTrigger.Exit;
                    break;
                case true:
                    trigger = LeverTrigger.On;
                    break;
                default:
                    trigger = LeverTrigger.Out;
                    break;
            }

            _onLeverCache = onLever;
            return trigger;
        }
        
        private void UpdateState(LeverTrigger trigger)
        {
            State = Activation.GetState(trigger, State);
        }

        public void Update()
        {
            var trigger = GetTrigger();
            UpdateState(trigger);
        }
        
        public bool GetVisualState()
        {
            return Activation.GetVisualState(State);
        }
    }
}