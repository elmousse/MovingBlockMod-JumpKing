using System.Collections.Generic;
using System;
using System.IO;
using System.Xml.Serialization;
using JumpKing;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MovingBlockMod.XmlData;


namespace MovingBlockMod
{
    public class MovingPlatformLoader
    {
        public static List<MovingPlatformXml> GetXmlData(string modPath)
        {
            var movingPlatformsDto = new MovingPlatformsXml();
            
            for (var i = 0; i < 169; i++)
            {
                var xmlPath = modPath + (i + 1) + ".xml";
                
                if (!File.Exists(xmlPath))
                {
                    continue;
                }
                
                using (var fileStream = new FileStream(xmlPath, FileMode.Open))
                {
                    
                    try
                    {
                        var serializer = new XmlSerializer(typeof(MovingPlatformsXml));
                        var dto = (MovingPlatformsXml)serializer.Deserialize(fileStream);
                        foreach (var platform in dto.Platforms)
                        {
                            platform.ScreenIndex = i;
                        }
                        movingPlatformsDto.Platforms.AddRange(dto.Platforms);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error loading moving platform xml: " + ex.Message);
                    }
                }
            }
            return movingPlatformsDto.Platforms;
        }
        
        public static Texture2D LoadTexture(string fileName, string type)
        {
            var contentManager = Game1.instance.contentManager;
            var sep = Path.DirectorySeparatorChar;
            var texturePath = $"{contentManager.root}{sep}moving_platforms{sep}{type}{sep}{fileName}";
            if (!File.Exists($"{texturePath}.xnb"))
            {
                return null;
            }
            return contentManager.Load<Texture2D>(texturePath);
        }
    }
}