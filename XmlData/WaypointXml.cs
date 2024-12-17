using System.Xml.Serialization;
using Microsoft.Xna.Framework;

namespace MovingBlockMod.XmlData
{
    public class WaypointXml
    {
        [XmlElement("X")]
        public int X { get; set; }

        [XmlElement("Y")]
        public int Y { get; set; }

        [XmlElement("time", IsNullable = true)]
        public float? Time { get; set; }
        
        [XmlElement("relativeTime", IsNullable = true)]
        public float? RelativeTime { get; set; }
        
        public Point Position => new Point(X, Y - _parentPlatform.ScreenIndex * 45 * 8);
        
        private MovingPlatformXml _parentPlatform;
        public void Setup(MovingPlatformXml parentPlatform)
        {
            _parentPlatform = parentPlatform;
        }
    }
}