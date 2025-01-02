

using Microsoft.Xna.Framework;
using MovingBlockMod.XmlData;

namespace MovingBlockMod
{
    public class MovingPlatformFactory
    {
        public MovingPlatform CreateFromXmlData(MovingPlatformXml data)
        {
            var hitboxTexture = Loader.LoadTexture(data.HitboxName, "hitboxes");
            var hitbox = MovingPlatformHitbox.FromTexture(hitboxTexture);
        
            var texture = Loader.LoadTexture(data.TextureName, "textures");
            var waypoints = data.GetWaypointsFromData();
        
            if (waypoints.Count == 0)
                return null;
        
            var textureOffset = new Point(data.TextureOffsetX ?? 0, data.TextureOffsetY ?? 0);
        
            var platform = new MovingPlatform(data.ScreenIndex, texture, waypoints, textureOffset);
        
            for (var i = 0; i < hitbox.Height * hitbox.Width; i++)
            {
                var xPosition = (i % hitbox.Width) * 8;
                var yPosition = (i / hitbox.Width) * 8;
            
                if (hitbox.Hitbox[i].A == 0)
                {
                    continue;
                }
                platform.AddBlock(new MovingBlock(new Point(xPosition, yPosition), platform));
            }
        
            return platform;
        }
    }

}