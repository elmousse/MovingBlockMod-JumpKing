

using JumpKing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MovingBlockMod.XmlData;

namespace MovingBlockMod.Factory
{
    public class MovingPlatformFactory
    {
        public MovingPlatform CreateFromXmlData(MovingPlatformXml data)
        {
            var hitboxTexture = Loader.LoadTexture(data.HitboxName, "hitboxes");
            if (hitboxTexture == null)
            {
                return null;
            }
            var hitbox = MovingPlatformHitbox.FromTexture(hitboxTexture);
            
            var texture = Loader.LoadTexture(data.TextureName, "textures")
                          ?? new Texture2D(Game1.graphics.GraphicsDevice, 0, 0);
            
            var waypoints = data.GetWaypointsFromData();
        
            if (waypoints.Count == 0)
                return null;
        
            var textureOffset = new Point(data.TextureOffsetX ?? 0, data.TextureOffsetY ?? 0);
            
            IPlatformActivation platformActivation;

            switch (data.SerializedMovingBehaviour)
            {
                case "onActiveOffInactive":
                    platformActivation = new PlatformActivationOnActiveOffInactive();
                    break;
                case "offAfterLoop":
                    platformActivation = new PlatformActivationOffAfterLoop(waypoints.Count);
                    break;
                case "offAfterOneWay":
                    if (data.PingPongMode)
                    {
                        platformActivation = new PlatformActivationOffAfterOneWay(waypoints.Count);
                    }
                    else
                    {
                        platformActivation = new PlatformActivationOffAfterLoop(waypoints.Count);
                    }
                    break;
                default:
                    platformActivation = new PlatformActivationAlwaysOn();
                    break;
            }
        
            var platform = new MovingPlatform(data.ScreenIndex, texture, waypoints, textureOffset, platformActivation);
        
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