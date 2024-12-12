using System.Collections.Generic;
using JumpKing.Mods;
using System.Diagnostics;
using EntityComponent;
using HarmonyLib;
using JumpKing.Player;
using JumpKing;

namespace MovingBlockMod
{
    [JumpKingMod("elmousse.MovingBlockMod")]
    public class ModEntry
    {
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
            player?.m_body?.RegisterBlockBehaviour(typeof(MovingBlock), new MovingBlockBehaviour());
        }
        
        [OnLevelUnload]
        public static void OnLevelUnload()
        {
            MovingPlatformManager.Instance.Reset();
        }
    }
}