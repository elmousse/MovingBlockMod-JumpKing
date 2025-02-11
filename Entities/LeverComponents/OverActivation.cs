namespace MovingBlockMod.Entities.LeverComponents
{
    public class OverActivation : IActivationType
    {
        public bool GetState(LeverTrigger trigger, bool currentState)
        {
            switch (trigger)
            {
                case LeverTrigger.On:
                case LeverTrigger.Enter:
                    return true;
                case LeverTrigger.Out:
                case LeverTrigger.Exit:
                default:
                    return false;
            }
        }
        
        public bool GetVisualState(bool currentState)
        {
            return currentState;
        }
    }
}