using System;
using MovingBlockMod.Utils;

namespace MovingBlockMod.Entities.LeverComponents
{
    public class SignalActivation : IActivationType
    {
        private TimeSpan LastTimeOn { get; set; } = TimeSpan.Zero;
        
        public bool GetState(LeverTrigger trigger, bool currentState)
        {
            if (trigger == LeverTrigger.Enter)
            {
                return true;
            }
            return false;
        }
        
        public bool GetVisualState(bool currentState)
        {
            if (currentState)
            {
                LastTimeOn = (TimeSpan)AchievementManagerWrapper.GetTimeSpan();
                return true;
            }
            var elapsedTime = (TimeSpan)AchievementManagerWrapper.GetTimeSpan() - LastTimeOn;
            if (elapsedTime.TotalSeconds < 0.3)
            {
                return true;
            }
            return false;
        }
    }
}