using System.Collections.Generic;
using System.Linq;
using MovingBlockMod.Blocks;
using MovingBlockMod.Entities;

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
            if (platform == null)
            {
                return;
            }
            Platforms.Add(platform);
        }
        
        public void UpdateAllPlatforms()
        {
            foreach (var platform in Platforms)
            {
                platform.Update1();
            }
        }
        
        public List<MovingBlock> GetMovingBlocksOnScreen(int screenIndex)
        {
            var platforms = Platforms.FindAll(platform => platform.CurrentScreens.Contains(screenIndex));
            return platforms.SelectMany(platform => platform.Blocks).ToList();
        }
    }
}