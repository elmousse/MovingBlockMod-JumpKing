using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MovingBlockMod.Factories
{
    public class MovingPlatformHitbox
    {
        public Color[] Hitbox { get; }
        public int Width { get; }
        public int Height { get; }
        
        public MovingPlatformHitbox(Color[] hitbox, int width, int height)
        {
            Hitbox = hitbox;
            Width = width;
            Height = height;
        }
        
        public static MovingPlatformHitbox FromTexture(Texture2D texture)
        {
            var colorArray = new Color[texture.Width * texture.Height];
            texture.GetData(colorArray);
            return new MovingPlatformHitbox(colorArray, texture.Width, texture.Height);
        }
    }
}