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
            
            var contentManager = Game1.instance.contentManager;
            var modLevelPath = (contentManager.root + @"moving_platforms\");
            
            var movingPlatformDtoList = MovingPlatformLoader.GetXmlData(modLevelPath);
            foreach (var movingPlatformDto in movingPlatformDtoList)
            {
                var movingPlatform = MovingPlatform.FromXmlData(movingPlatformDto);
                MovingPlatformManager.Instance.RegisterPlatform(movingPlatform);
            }
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