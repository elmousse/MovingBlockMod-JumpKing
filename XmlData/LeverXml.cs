using System.Collections.Generic;
using System.Xml.Serialization;

namespace MovingBlockMod.XmlData
{
    public class LeverXml
    {
        [XmlElement("id")]
        public string LeverId { get; set; }
        
        [XmlArray("ActivationZone")]
        [XmlArrayItem("Rectangle")]
        public List<RectangleXml> Waypoints { get; set; } = new List<RectangleXml>();
    }
    
    public class RectangleXml
    {
        [XmlElement("screen")]
        public int Screen { get; set; }
        
        [XmlElement("X")]
        public int X { get; set; }
        
        [XmlElement("Y")]
        public int Y { get; set; }
        
        [XmlElement("width")]
        public int Width { get; set; }
        
        [XmlElement("height")]
        public int Height { get; set; }
    }
}