using System;

namespace MovingBlockMod.Entities.LeverComponents
{
    public interface IActivationType
    {
        bool GetState(LeverTrigger trigger, bool currentState);
        
        bool GetVisualState(bool currentState);
    }
}