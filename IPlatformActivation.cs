namespace MovingBlockMod
{
    public interface IPlatformActivation
    {
        bool PlatformState { get; set;  }
        bool PlatformStateCache { get; set; }
        
        void UpdatePlatformState(bool leverState, int waypointIndex);
    }
}