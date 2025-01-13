namespace MovingBlockMod.Entities.MovingPlatformEntity
{
    public class PlatformActivationOnActiveOffInactive : IPlatformActivation
    {
        public bool PlatformState { get; set; } = true;
        public bool PlatformStateCache { get; set; } = true;
        
        public void UpdatePlatformState(bool leverState, int waypointIndex)
        {
            PlatformStateCache = PlatformState;
            PlatformState = leverState;
        }
    }
}