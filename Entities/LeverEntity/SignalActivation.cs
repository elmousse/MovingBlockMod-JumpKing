namespace MovingBlockMod.Entities.LeverEntity
{
    public class SignalActivation : IActivationType
    {
        public bool GetState(LeverTrigger trigger, bool currentState)
        {
            if (trigger == LeverTrigger.Enter)
            {
                return true;
            }
            return false;
        }
    }
}