namespace MovingBlockMod.Entities.MovingPlatformComponents
{
    public class PlatformActivationOffAfterLoop : IPlatformActivation
    {
        public bool PlatformState { get; set; } = true;
        public bool PlatformStateCache { get; set; } = true;
        private int _maxWaypoint;
        private float _travelRatioCache = 1f;
        
        public PlatformActivationOffAfterLoop(int maxWaypoint)
        {
            _maxWaypoint = maxWaypoint - 2;
        }
        
        public void UpdatePlatformState(bool leverState, int waypointIndex)
        {
            PlatformStateCache = PlatformState;
            var travelRatio = (float) waypointIndex / _maxWaypoint;
            if (leverState)
            {
                PlatformState = true;
                _travelRatioCache = travelRatio;
                return;
            }
            
            if (_travelRatioCache == 1f && travelRatio == 0f)
            {
                PlatformState = false;
            }
            _travelRatioCache = travelRatio;
        }
    }
}