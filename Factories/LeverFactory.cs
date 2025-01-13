using Microsoft.Xna.Framework;
using MovingBlockMod.Blocks;
using MovingBlockMod.Entities;
using MovingBlockMod.XmlData;

namespace MovingBlockMod.Factories
{
    public class LeverFactory
    {
        public Lever CreateFromXmlData(LeverXml data)
        {
            if (data.ActivationType == null || data.LeverId == null)
            {
                return null;
            }
            
            var lever = new Lever(data.ActivationType, data.LeverId, data.StartingState);
            
            foreach (var zone in data.ActivationZones)
            {
                var block = new LeverBlock(
                    new Rectangle(zone.X, zone.Y, zone.Width, zone.Height),
                    lever,
                    zone.Screen - 1);
                lever.AddBlock(block);
            }
        
            return lever;
        }
    }
}