using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework;
using MovingBlockMod.Entities.MovingPlatformComponents;

namespace MovingBlockMod.XmlData
{
    [XmlRoot("MovingPlatforms")]
    public class MovingPlatformsXml
    {
        [XmlElement("MovingPlatform")]
        public List<MovingPlatformXml> Platforms { get; set; } = new List<MovingPlatformXml>();
    }
    
    public class MovingPlatformXml
    {
        public int ScreenIndex { get; set; }
        
        [XmlElement("hitboxName")]
        public string HitboxName { get; set; }

        [XmlElement("textureName")]
        public string TextureName { get; set; }

        [XmlElement("totalTime")]
        public float? TotalTime { get; set; }
        
        [XmlElement("pingPongMode")]
        public bool PingPongMode { get; set; }
        
        [XmlElement("textureOffsetX")]
        public int? TextureOffsetX { get; set; }
        
        [XmlElement("textureOffsetY")]
        public int? TextureOffsetY { get; set; }
        
        [XmlElement("leverId")]
        public string LeverId { get; set; }
        
        [XmlElement("movingBehaviour")]
        public string SerializedMovingBehaviour { get; set; }
        
        
        [XmlArray("Waypoints")]
        [XmlArrayItem("Waypoint")]
        public List<WaypointXml> Waypoints { get; set; } = new List<WaypointXml>();
        
        [XmlArray("CriticalAreas")]
        [XmlArrayItem("CriticalArea")]
        public List<CriticalAreaXml> CriticalAreas { get; set; } = new List<CriticalAreaXml>();

        public void Setup(int screenIndex)
        {
            ScreenIndex = screenIndex;
            foreach (var waypoint in Waypoints)
            {
                waypoint.Setup(this);
            }
            foreach (var criticalArea in CriticalAreas)
            {
                criticalArea.Setup(this);
            }
        }
        
        public List<Waypoint> GetWaypointsFromData()
        {
            var waypoints = new List<Waypoint>();
            
            if (!(TotalTime == null || TotalTime == 0))
            {
                var fullLength = 0f;
                for (var i = 0; i < Waypoints.Count - 1; i++)
                {
                    var position1 = Waypoints[i];
                    var position2 = Waypoints[i + 1];
                    var length = Math.Abs(position1.X - position2.X) + Math.Abs(position1.Y - position2.Y);
                    fullLength += length;
                }
                var timePerUnit = (float)TotalTime / fullLength;
                var currentTime = 0f;
                for (var i = 0; i < Waypoints.Count - 1; i++)
                {
                    var position1 = Waypoints[i];
                    var position2 = Waypoints[i + 1];
                    var length = Math.Abs(position1.X - position2.X) + Math.Abs(position1.Y - position2.Y);
                    var time = length * timePerUnit;
                    waypoints.Add(new Waypoint(position1.Position, currentTime));
                    currentTime += time;
                }
                waypoints.Add(new Waypoint(Waypoints[Waypoints.Count - 1].Position, (float)TotalTime));
            }
            else if (IndividualTimeIsAvailable())
            {
                waypoints.Add(new Waypoint(Waypoints[0].Position, 0f));
                
                for (var i = 1; i < Waypoints.Count; i++)
                {
                    var time = Waypoints[i].Time ?? Waypoints[i].RelativeTime + waypoints[i-1].Time ?? 0f;
                    waypoints.Add(new Waypoint(Waypoints[i].Position, time));
                }
            }

            if (!PingPongMode)
                return waypoints;
            
            for (var i = waypoints.Count - 2; i >= 0; i--)
            {
                waypoints.Add(new Waypoint(waypoints[i].Position, waypoints[i+1].Time - waypoints[i].Time + waypoints[waypoints.Count - 1].Time));
            }
            return waypoints;
        }
        
        private bool IndividualTimeIsAvailable()
        {
            var waypointsWithTime = 0;

            for (var i = 1; i < Waypoints.Count; i++)
            {
                if ((Waypoints[i].Time == null || Waypoints[i].Time == 0) &&
                    (Waypoints[i].RelativeTime == null || Waypoints[i].RelativeTime == 0))
                    continue;

                waypointsWithTime++;
            }

            return waypointsWithTime == Waypoints.Count - 1;
        }
    }
    
    public class WaypointXml
    {
        [XmlElement("X")]
        public int X { get; set; }

        [XmlElement("Y")]
        public int Y { get; set; }
        
        [XmlElement("screenOffset", IsNullable = true)]
        public int? ScreenOffset { get; set; }

        [XmlElement("time", IsNullable = true)]
        public float? Time { get; set; }
        
        [XmlElement("relativeTime", IsNullable = true)]
        public float? RelativeTime { get; set; }
        
        public Point Position => new Point(X, Y - (_parentPlatform.ScreenIndex + (ScreenOffset ?? 0)) * 360);
        
        private MovingPlatformXml _parentPlatform;
        public void Setup(MovingPlatformXml parentPlatform)
        {
            _parentPlatform = parentPlatform;
        }
    }
    
    public class CriticalAreaXml
    {
        [XmlElement("X")]
        public int X { get; set; }

        [XmlElement("Y")]
        public int Y { get; set; }
        
        [XmlElement("width")]
        public int Width { get; set; }
        
        [XmlElement("height")]
        public int Height { get; set; }
        
        [XmlElement("screenOffset", IsNullable = true)]
        public int? ScreenOffset { get; set; }
        
        public Rectangle Area => new Rectangle(X, Y - (_parentPlatform.ScreenIndex + (ScreenOffset ?? 0)) * 360, Width, Height);
        
        public int ScreenIndex => _parentPlatform.ScreenIndex + (ScreenOffset ?? 0);
        
        private MovingPlatformXml _parentPlatform;
        public void Setup(MovingPlatformXml parentPlatform)
        {
            _parentPlatform = parentPlatform;
        }
    }
}