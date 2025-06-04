using System.Collections.Generic;
using JumpKing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MovingBlockMod.Blocks;
using MovingBlockMod.Entities;
using MovingBlockMod.Entities.MovingPlatformComponents;
using MovingBlockMod.Utils;
using MovingBlockMod.XmlData;

namespace MovingBlockMod.Factories
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

            var screens = new List<int>();
            foreach (var w in data.Waypoints)
            {
                var screen = (w.ScreenOffset ?? 0) + data.ScreenIndex;
                if (!screens.Contains(screen))
                {
                    screens.Add(screen);
                }
                if (w.Y + hitbox.Height * 8 > 360)
                {
                    screens.Add(screen - 1);
                }
            }
        
            var platform = new MovingPlatform(screens.ToArray(), texture, waypoints, textureOffset, platformActivation, hitbox.Height * 8);
        
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
            
            // Add critical areas
            foreach (var criticalArea in data.CriticalAreas)
            {
                var collider = criticalArea.Area;
                var screen = criticalArea.ScreenIndex;
                platform.AddCriticalArea(new CriticalAreaBlock(collider, platform, screen));
            }
        
            return platform;
        }
    }

}