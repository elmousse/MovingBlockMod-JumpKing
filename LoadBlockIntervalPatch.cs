using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;
using HarmonyLib;
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
            var blockList = new List<IBlock>(__result);
            
            // Add MovingPlatform.Blocks to the blockList for MovingPlatform equal to the current screen
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