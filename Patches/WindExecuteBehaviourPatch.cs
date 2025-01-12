using HarmonyLib;
using JumpKing.BodyCompBehaviours;

namespace MovingBlockMod.Patches
{
    [HarmonyPatch(typeof(PlayBumpSFXBehaviour), nameof(PlayBumpSFXBehaviour.ExecuteBehaviour))]
    public class PlayBumpSfxBehaviourPatch
    {
        public static void Postfix()
        {
            LeverManager.Instance.UpdateAllLevers();
            MovingPlatformManager.Instance.UpdateAllPlatforms();
        }
    }
}