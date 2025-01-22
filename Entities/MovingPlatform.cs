using System;
using System.Collections.Generic;
using System.Linq;
using EntityComponent;
using JumpKing;
using JumpKing.Level;
using JumpKing.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MovingBlockMod.Blocks;
using MovingBlockMod.Entities.MovingPlatformComponents;
using MovingBlockMod.Utils;


namespace MovingBlockMod.Entities
{
    public class MovingPlatform : Entity
    {
        public Lever Lever { get; private set; }
        private readonly List<MovingBlock> _blocks = new List<MovingBlock>();
        public List<MovingBlock> Blocks => _blocks;
        public int[] PotentialScreens { get; }
        public int[] CurrentScreens
        {
            get
            {
                var screen = ((CurrentPosition.Y % 360 + 360) % 360 - CurrentPosition.Y) / 360;
                if ((((CurrentPosition.Y + Height) % 360 + 360) % 360 - CurrentPosition.Y + Height)/ 360 == screen)
                {
                    return new[] {screen};
                }
                return new[] {screen, screen - 1};
            }
        }

        private int Height { get; }
        
        private readonly Sprite _sprite;
        
        private List<Waypoint> Waypoints { get; }
        public Point CurrentPosition { get; private set; }
        private Point TextureOffset { get; }
        
        private IPlatformActivation _activation;
        
        private TimeSpan _delay = TimeSpan.Zero;
        private TimeSpan _lastTimeActive = TimeSpan.Zero;
        
        public MovingPlatform(
            int[] screens,
            Texture2D texture,
            List<Waypoint> waypoints,
            Point textureOffset,
            IPlatformActivation activation,
            int height)
        {
            PotentialScreens = screens;
            Waypoints = waypoints;
            CurrentPosition = Waypoints[0].Position;
            _sprite = Sprite.CreateSprite(texture);
            TextureOffset = textureOffset;
            _activation = activation;
            Height = height;
        }
        
        public void SetLever(Lever lever)
        {
            Lever = lever;
        }
        
        public void SetPlatformActivation(IPlatformActivation activation)
        {
            _activation = activation;
        }
        
        public void AddBlock(MovingBlock block)
        {
            _blocks.Add(block);
        }
        
        /*protected override void Update(float delta)
        {
            Position = UpdatePlatformPosition();
            foreach (var block in _blocks)
            {
                block.UpdatePosition();
            }
        }*/
        
        public void Update1()
        {
            var time = (TimeSpan)AchievementManagerWrapper.GetTimeSpan();
            var modTime = ((float)time.TotalSeconds - (float)_delay.TotalSeconds) % Waypoints[Waypoints.Count - 1].Time;
            
            var waypointIndex = GetWaypointIndex(modTime);
            
            _activation.UpdatePlatformState(Lever?.State ?? true, waypointIndex);
            
            if (!_activation.PlatformState)
            {
                if (_activation.PlatformStateCache)
                {
                    _lastTimeActive = time;
                }
                return;
            }

            if (!_activation.PlatformStateCache)
            {
                _delay += time - _lastTimeActive;
            }
            

            modTime = ((float)time.TotalSeconds - (float)_delay.TotalSeconds) % Waypoints[Waypoints.Count - 1].Time;
            waypointIndex = GetWaypointIndex(modTime);
            
            CurrentPosition = GetPlatformPosition(modTime, waypointIndex);
            foreach (var block in _blocks)
            {
                block.UpdatePosition();
            }
        }
        
        public override void Draw()
        {
            if (EntityManager.instance.Find<PlayerEntity>() == null
                || !CurrentScreens.Contains(LevelManager.CurrentScreen.GetIndex0()))
            {
                return;
            }
            _sprite.Draw(Camera.TransformVector2((CurrentPosition - TextureOffset).ToVector2()));
        }
        
        private Point GetPlatformPosition(float modTime, int index)
        {
            var timeRatio = (modTime - Waypoints[index].Time) / (Waypoints[index + 1].Time - Waypoints[index].Time);
            var x = Waypoints[index].X + (Waypoints[index + 1].X - Waypoints[index].X) * timeRatio;
            var y = Waypoints[index].Y + (Waypoints[index + 1].Y - Waypoints[index].Y) * timeRatio;
            return new Point((int)x, (int)y);
        }
        
        private int GetWaypointIndex(float modTime)
        {
            for (var i = 0; i < Waypoints.Count - 1; i++)
            {
                if (!(modTime >= Waypoints[i].Time && modTime < Waypoints[i + 1].Time))
                {
                    continue;
                }
                return i;
            }
            return 0;
        }
    }
}