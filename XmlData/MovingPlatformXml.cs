using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Graphics;

namespace MovingBlockMod.XmlData
{
    public class MovingPlatformXml
    {
        public int ScreenIndex { get; set; }
        [XmlElement("HitboxName")]
        public string HitboxName { get; set; }

        [XmlElement("TextureName")]
        public string TextureName { get; set; }

        [XmlElement("TotalTime")]
        public float TotalTime { get; set; }

        [XmlArray("Positions")]
        [XmlArrayItem("Position")]
        public List<PositionXml> Positions { get; set; } = new List<PositionXml>();
    }
}