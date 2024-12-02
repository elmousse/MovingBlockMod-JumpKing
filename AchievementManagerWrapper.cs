using System;
using System.Reflection;

namespace MovingBlockMod
{
    public class AchievementManagerWrapper
    {
        private static object _instance;
        private static Type _type;

        static AchievementManagerWrapper()
        {
            _type = Type.GetType("JumpKing.MiscSystems.Achievements.AchievementManager, JumpKing");
            var instanceField = _type?.GetField("instance", BindingFlags.NonPublic | BindingFlags.Static);
            _instance = instanceField?.GetValue(null);
        }

        public static object GetCurrentStats()
        {
            var getCurrentStatsMethod = _type.GetMethod("GetCurrentStats", BindingFlags.Public | BindingFlags.Instance);
            return getCurrentStatsMethod?.Invoke(_instance, null);
        }

        public static object GetTimeSpan()
        {
            var currentStats = GetCurrentStats();
            var timeSpanProperty =
                currentStats.GetType().GetProperty("timeSpan", BindingFlags.Public | BindingFlags.Instance);
            return timeSpanProperty?.GetValue(currentStats);
        }
    }
}