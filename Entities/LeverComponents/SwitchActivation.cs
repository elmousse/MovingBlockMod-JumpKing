namespace MovingBlockMod.Entities.LeverComponents
{
    public class SwitchActivation : IActivationType
    {
        public bool GetState(LeverTrigger trigger, bool currentState)
        {
            if (trigger == LeverTrigger.Enter)
            {
                return !currentState;
            }
            return currentState;
        }
    }
}