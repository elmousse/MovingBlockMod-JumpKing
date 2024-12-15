using HarmonyLib;
using JumpKing.BodyCompBehaviours;

namespace MovingBlockMod.Patches
{
    [HarmonyPatch(typeof(WindVelocityUpdateBehaviour), nameof(WindVelocityUpdateBehaviour.ExecuteBehaviour))]
    public class WindExecuteBehaviourPatch
    {
        public static void Postfix()
        {
            MovingPlatformManager.Instance.UpdateAllPlatforms();
        }
    }
}