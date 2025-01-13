namespace MovingBlockMod.Entities.MovingPlatformComponents
{
    public class PlatformActivationOffAfterOneWay : IPlatformActivation
    {
        public bool PlatformState { get; set; } = true;
        public bool PlatformStateCache { get; set; } = true;
        private int _maxWaypoint;
        private float _travelRatioCache;
        
        public PlatformActivationOffAfterOneWay(int maxWaypoint)
        {
            _maxWaypoint = maxWaypoint - 1;
        }
        
        public void UpdatePlatformState(bool leverState, int waypointIndex)
        {
            PlatformStateCache = PlatformState;
            var travelRatio = (float)(waypointIndex + 1) / _maxWaypoint;
            if (leverState)
            {
                PlatformState = true;
                _travelRatioCache = travelRatio;
                return;
            }
            
            if ((_travelRatioCache == 1f && travelRatio != 1f) || (_travelRatioCache == 0.5f && travelRatio > 0.5f))
            {
                PlatformState = false;
            }
            _travelRatioCache = travelRatio;
        }
    }
}