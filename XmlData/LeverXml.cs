using System.Collections.Generic;
using System.Xml.Serialization;
using MovingBlockMod.Entities.LeverComponents;

namespace MovingBlockMod.XmlData
{
    [XmlRoot("Levers")]
    public class LeversXml
    {
        [XmlElement("Lever")]
        public List<LeverXml> Levers { get; set; } = new List<LeverXml>();
    }
    public class LeverXml
    {
        [XmlElement("id")]
        public string LeverId { get; set; }
        
        [XmlElement("startingState")]
        public bool StartingState { get; set; }
        
        [XmlElement("activationType")]
        public string SerializedActivationType { get; set; }
        public IActivationType ActivationType
        {
            get
            {
                switch (SerializedActivationType)
                {
                    case "over" : return new OverActivation();
                    case "switch" : return new SwitchActivation();
                    case "signal" : return new SignalActivation();
                    default: return null;
                }
            }
        }

        [XmlArray("ActivationZones")]
        [XmlArrayItem("Zone")]
        public List<ZoneXml> ActivationZones { get; set; } = new List<ZoneXml>();
    }
    
    public class ZoneXml
    {
        [XmlElement("screen")]
        public int Screen { get; set; }
        
        [XmlElement("textureName")]
        public string TextureName { get; set; }
        
        [XmlElement("textureOffsetX")]
        public int? TextureOffsetX { get; set; }
        
        [XmlElement("textureOffsetY")]
        public int? TextureOffsetY { get; set; }
        
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