using System.Xml.Serialization;
using Microsoft.Xna.Framework;

namespace MovingBlockMod.XmlData
{
    public class WaypointXml
    {
        [XmlElement("X")] // Correspond à la balise <X>
        public int X { get; set; }

        [XmlElement("Y")] // Correspond à la balise <Y>
        public int Y { get; set; }

        [XmlElement("Time", IsNullable = true)] // Correspond à la balise <Time> (nullable)
        public int? Time { get; set; }
        
        public Point Position => new Point(X, Y);
    }
}