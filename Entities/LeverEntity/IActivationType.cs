using System;

namespace MovingBlockMod.Entities.LeverEntity
{
    public interface IActivationType
    {
        bool GetState(LeverTrigger trigger, bool currentState);
    }
}