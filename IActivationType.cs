using System;

namespace MovingBlockMod
{
    public interface IActivationType
    {
        bool GetState(LeverTrigger trigger, bool currentState);
    }
}