using System.Collections.Generic;

namespace MovingBlockMod
{
    public class MovingPlatformManager
    {
        private static MovingPlatformManager _instance;

        public List<MovingPlatform> Platforms { get; }
        
        private MovingPlatformManager()
        {
            Platforms = new List<MovingPlatform>();
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
            Platforms.Clear();
        }
        
        public void RegisterPlatform(MovingPlatform platform)
        {
            Platforms.Add(platform);
        }
        
        public void UnregisterPlatform(MovingPlatform platform)
        {
            Platforms.Remove(platform);
        }
        
        public void UpdateAllPlatforms()
        {
            foreach (var platform in Platforms)
            {
                platform.Update1();
            }
        }
    }
}