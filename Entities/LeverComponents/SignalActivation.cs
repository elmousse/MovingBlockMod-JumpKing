using System;
using MovingBlockMod.Utils;

namespace MovingBlockMod.Entities.LeverComponents
{
    public class SignalActivation : IActivationType
    {
        private TimeSpan LastTimeOn { get; set; } = TimeSpan.Zero;
        
        public bool GetState(LeverTrigger trigger, bool currentState)
        {
            var currentTime = (TimeSpan)AchievementManagerWrapper.GetTimeSpan();
            if (trigger == LeverTrigger.Enter)
            {
                LastTimeOn = currentTime;
                return true;
            }
            if (currentTime < TimeSpan.FromSeconds(0.05))
            {
                return false;
            }
            var elapsedTime = currentTime - LastTimeOn;
            if (elapsedTime.TotalSeconds < 0.05)
            {
                return true;
            }
            return false;
        }
        
        public bool GetVisualState(bool currentState)
        {
            if (currentState)
            {
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