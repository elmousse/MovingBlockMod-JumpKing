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
            
            var levelRoot = Game1.instance.contentManager.root;
            var contentManager = Game1.instance.contentManager;
            
            
            // la on register toutes les moving platforms
            // comme ca tous les blocks sont creer et les textures sont load
            // yaura plus qu'a les ajouter dans LoadBlocksInterval
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