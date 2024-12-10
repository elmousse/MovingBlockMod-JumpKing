using System.Collections.Generic;
using System.Xml.Serialization;

namespace MovingBlockMod.XmlData
{
    [XmlRoot("MovingPlatforms")]
    public class MovingPlatformsXml
    {
        [XmlElement("MovingPlatform")]
        public List<MovingPlatformXml> Platforms { get; set; } = new List<MovingPlatformXml>();
    }
}