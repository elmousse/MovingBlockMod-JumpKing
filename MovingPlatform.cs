using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace MovingBlockMod
{
    public class MovingPlatform
    {
        private List<MovingBlock> _blocks = new List<MovingBlock>();
        public int Screen;
        
        public Point Position { get; private set; }
        
        public MovingPlatform(int screen)
        {
            Screen = screen;
        }
        
        public void AddBlock(MovingBlock block)
        {
            _blocks.Add(block);
        }
        
        public void Update()
        {
            Position = UpdatePlatformPosition();
            foreach (var block in _blocks)
            {
                block.UpdatePosition();
            }
        }
        
        private Point UpdatePlatformPosition()
        {
            var timeSpan = (TimeSpan)AchievementManagerWrapper.GetTimeSpan();
            var time = (float)timeSpan.TotalSeconds;
            var x = (float)( 100 + Math.Abs( 200 * ( time / 10 - Math.Floor( time / 10 ) - 0.5 )));
            return new Point((int)x, 100);
        }
    }
}