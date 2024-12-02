using System.Collections.Generic;

namespace MovingBlockMod
{
    public class MovingPlatformManager
    {
        private static MovingPlatformManager _instance;

        private List<MovingPlatform> _platforms;
        
        private MovingPlatformManager()
        {
            _platforms = new List<MovingPlatform>();
        }
        
        public static MovingPlatformManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new MovingPlatformManager();
                }
                return _instance;
            }
        }
        
        public void Reset()
        {
            _platforms.Clear();
        }
        
        public void RegisterPlatform(MovingPlatform platform)
        {
            _platforms.Add(platform);
        }
        
        public void UnregisterPlatform(MovingPlatform platform)
        {
            _platforms.Remove(platform);
        }
        
        public void UpdateAllPlatforms()
        {
            foreach (var platform in _platforms)
            {
                platform.Update();
            }
        }
    }
}