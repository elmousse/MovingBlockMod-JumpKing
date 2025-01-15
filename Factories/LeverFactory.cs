using System;
using JumpKing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MovingBlockMod.Blocks;
using MovingBlockMod.Entities;
using MovingBlockMod.Utils;
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
            
            foreach (var zoneData in data.ActivationZones)
            {
                var block = new LeverBlock(
                    new Rectangle(zoneData.X, zoneData.Y, zoneData.Width, zoneData.Height),
                    lever);

                var textureOffset = new Point(zoneData.TextureOffsetX ?? 0, zoneData.TextureOffsetY ?? 0);
                
                var sourceTexture = Loader.LoadTexture(zoneData.TextureName, "textures");
                var textures = SplitTextureVertically(sourceTexture, Game1.graphics.GraphicsDevice);

                var zone = new LeverZone(
                    block,
                    lever,
                    zoneData.Screen - 1,
                    textureOffset,
                    textures
                );
                
                lever.AddZone(zone);
            }
        
            return lever;
        }
        
        private Texture2D[] SplitTextureVertically(Texture2D sourceTexture, GraphicsDevice graphicsDevice)
        {
            if (sourceTexture == null)
            {
                return new Texture2D[0];
            }
            
            var width = sourceTexture.Width;
            var height = sourceTexture.Height;
            
            if (height % 2 != 0)
            {
                return new Texture2D[0];
            }
            
            var halfHeight = height / 2;
            
            var sourceData = new Color[width * height];
            sourceTexture.GetData(sourceData);
            
            var topData = new Color[width * halfHeight];
            for (var y = 0; y < halfHeight; y++)
            {
                Array.Copy(sourceData, y * width, topData, y * width, width);
            }
            
            var bottomData = new Color[width * halfHeight];
            for (var y = 0; y < halfHeight; y++)
            {
                Array.Copy(sourceData, (y + halfHeight) * width, bottomData, y * width, width);
            }
            
            var topHalf = new Texture2D(graphicsDevice, width, halfHeight);
            var bottomHalf = new Texture2D(graphicsDevice, width, halfHeight);
            
            topHalf.SetData(topData);
            bottomHalf.SetData(bottomData);

            return new [] { topHalf, bottomHalf };
        }

    }
}