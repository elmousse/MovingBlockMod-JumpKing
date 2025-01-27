using System.Reflection;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HarmonyLib;
using JumpKing;
using JumpKing.Level;
using JumpKing.Level.Sampler;
using MovingBlockMod.Entities.MovingPlatformComponents;
using MovingBlockMod.Factories;
using MovingBlockMod.Utils;

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
            
            // Add MovingBlocks
            blockList.AddRange(MovingPlatformManager.Instance.Platforms
                .Where(movingPlatform => movingPlatform.PotentialScreens.Contains(p_screen))
                .SelectMany(movingPlatform => movingPlatform.Blocks));
            
            // Add CriticalAreaBlocks
            blockList.AddRange(MovingPlatformManager.Instance.Platforms
                .SelectMany(movingPlatform => movingPlatform.CriticalAreas)
                .Where(block => block.SreenIndex == p_screen));
            
            // Add LeverBlocks
            blockList.AddRange(LeverManager.Instance.Levers
                .SelectMany(lever => lever.Zones
                    .Where(zone => zone.Screen == p_screen)
                    .Select(zone => zone.Block)));
            
            __result = blockList.ToArray();
        }
    }
}