using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using HarmonyLib;
using JumpKing;
using JumpKing.Level;
using JumpKing.Level.Sampler;
using MovingBlockMod.Factory;

namespace MovingBlockMod.Patches
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
                
                var leverDtoList = Loader.GetLeverXmlData(modLevelPath);
                var leverFactory = new LeverFactory();
                foreach (var leverDto in leverDtoList)
                {
                    var lever = leverFactory.CreateFromXmlData(leverDto);
                    LeverManager.Instance.RegisterLever(lever);
                }
                
                var movingPlatformDtoList = Loader.GetMovingPlatformXmlData(modLevelPath);
                var movingPlatformFactory = new MovingPlatformFactory();
                foreach (var movingPlatformDto in movingPlatformDtoList)
                {
                    var movingPlatform = movingPlatformFactory.CreateFromXmlData(movingPlatformDto);
                    MovingPlatformManager.Instance.RegisterPlatform(movingPlatform);
                    
                    var lever = LeverManager.Instance.Levers.Find(l => l.Id == movingPlatformDto.LeverId);
                    if (lever == null)
                    {
                        movingPlatform.SetPlatformActivation(new PlatformActivationAlwaysOn());
                        continue;
                    }
                    lever.AddPlatform(movingPlatform);
                    movingPlatform.SetLever(lever);
                }
            }
            var blockList = new List<IBlock>(__result);
            foreach (var movingPlatform in MovingPlatformManager.Instance.Platforms)
            {
                if (movingPlatform.Screen != p_screen)
                {
                    continue;
                }
                blockList.AddRange(movingPlatform.Blocks);
            }
            foreach (var lever in LeverManager.Instance.Levers)
            {
                var blocks = lever.Blocks.FindAll(b => b.Screen == p_screen);
                if (!blocks.Any())
                {
                    continue;
                }
                blockList.AddRange(blocks);
            }
            __result = blockList.ToArray();
        }
    }
}