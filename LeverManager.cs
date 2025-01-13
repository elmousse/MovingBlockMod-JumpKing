using System.Collections.Generic;
using MovingBlockMod.Entities;

namespace MovingBlockMod
{
    public class LeverManager
    {
        private static LeverManager _instance;

        public List<Lever> Levers { get; }
        
        private LeverManager()
        {
            Levers = new List<Lever>();
        }
        
        public static LeverManager Instance => _instance ?? (_instance = new LeverManager());
        
        public void Reset()
        {
            Levers.Clear();
        }
        
        public void RegisterLever(Lever lever)
        {
            Levers.Add(lever);
        }
        
        public void UpdateAllLevers()
        {
            foreach (var lever in Levers)
            {
                lever.Update();
            }
        }
    }
}