namespace MovingBlockMod
{
    public class PlatformActivationAlwaysOn : IPlatformActivation
    {
        public bool PlatformState { get; set; } = true;
        public bool PlatformStateCache { get; set; } = true;
        
        public void UpdatePlatformState(bool leverState, int _) {}
    }
}