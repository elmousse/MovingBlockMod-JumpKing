using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using HarmonyLib;
using JumpKing;
using JumpKing.Level;
using JumpKing.Level.Sampler;

namespace MovingBlockMod
{
    [HarmonyPatch]
    public class LoadBlockIntervalPatch
    {
        static MethodBase TargetMethod()
        {
            var type = typeof(LevelManager);
            var method = type.GetMethod("LoadBlocksInterval", BindingFlags.NonPublic | BindingFlags.Static);
            return method;
        }

        static void Postfix(
            ref IBlock[] __result,
            LevelTexture p_src,
            JumpKing.Workshop.Level level,
            int p_screen,
            ref bool wind_enabled,
            ref TeleportLink[] teleport,
            ref float wind_int,
            ref bool? wind_direction)
        {
            if (p_screen == 0)
            {
                MovingPlatformManager.Instance.Reset();
                
                var contentManager = Game1.instance.contentManager;
                var sep = Path.DirectorySeparatorChar;
                var modLevelPath = $"{contentManager.root}{sep}moving_platforms{sep}".Replace('\\', '/');
                
                var movingPlatformDtoList = MovingPlatformLoader.GetXmlData(modLevelPath);
                foreach (var movingPlatformDto in movingPlatformDtoList)
                {
                    var movingPlatform = MovingPlatform.FromXmlData(movingPlatformDto);
                    MovingPlatformManager.Instance.RegisterPlatform(movingPlatform);
                }
            }
            var blockList = new List<IBlock>(__result);
            foreach (var movingPlatform in MovingPlatformManager.Instance.Platforms)
            {
                if (movingPlatform.Screen == p_screen)
                {
                    blockList.AddRange(movingPlatform.Blocks);
                }
            }
            __result = blockList.ToArray();
        }
    }
}