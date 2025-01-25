using System;
using System.Collections.Generic;
using JumpKing.Mods;
using System.Diagnostics;
using EntityComponent;
using HarmonyLib;
using JumpKing.Player;
using JumpKing.API;
using JumpKing.BodyCompBehaviours;
using MovingBlockMod.BlockBehaviours;
using MovingBlockMod.Blocks;
using MovingBlockMod.BodyCompBehaviours;

namespace MovingBlockMod
{
    [JumpKingMod("elmousse.MovingBlockMod")]
    public class ModEntry
    {
        private static readonly Dictionary<Type, Func<IBlockBehaviour>> BlockBehaviours = new Dictionary<Type, Func<IBlockBehaviour>>
        {
            { typeof(MovingBlock), () => new MovingBlockBehaviour() },
            { typeof(LeverBlock), () => new LeverBlockBehaviour() }
        };

        [BeforeLevelLoad]
        public static void BeforeLevelLoad()
        {
#if DEBUG
            Debugger.Launch();
            Harmony.DEBUG = true;
#endif
            var harmony = new Harmony("elmousse.MovingBlockMod");
            harmony.PatchAll();
        }

        [OnLevelStart]
        public static void OnLevelStart()
        {
            var player = EntityManager.instance.Find<PlayerEntity>();
            if (player?.m_body == null)
            {
                return;
            }
            
            // Register the block behaviours
            foreach (var blockBehaviour in BlockBehaviours)
            {
                player.m_body.RegisterBlockBehaviour(blockBehaviour.Key, blockBehaviour.Value());
            }
            
            // Register the moving platform behaviour
            var existingBehaviours = player.m_body.GetBehaviourList();

            foreach (var behaviour in existingBehaviours)
            {
                if (behaviour.GetType().Name != "CacheCollisionStateBehaviour")
                {
                    continue;
                }
                var newBehaviour = new ExecuteMovingPlatformBehaviour();

                player.m_body.RegisterBehaviourBefore(newBehaviour, behaviour);
                break;
            }
        }

        [OnLevelUnload]
        public static void OnLevelUnload()
        {
            MovingPlatformManager.Instance.Reset();
            LeverManager.Instance.Reset();
        }
    }
}