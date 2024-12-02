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
            Debug.WriteLine($"Recherche dans le type : {type.FullName}");
            var method = type.GetMethod("LoadBlocksInterval", BindingFlags.NonPublic | BindingFlags.Static);
            if (method == null)
            {
                Debug.WriteLine("La méthode LoadBlocksInterval est introuvable.");
            }
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
            
            // Regarder si le fichier du screen existe
            // Si oui, dans le fichier du screen correspondant, pour chaque MovingPlatform
            // Parcourir chaque pixel de la texture du MovingPlatform
            // Ajouter le block correspondant dans blockList
            
            __result = blockList.ToArray();
        }
    }
}