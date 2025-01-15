using EntityComponent;
using JumpKing;
using JumpKing.Level;
using JumpKing.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MovingBlockMod.Blocks;

namespace MovingBlockMod.Entities
{
    public class LeverZone : Entity
    {
        public int Screen { get; private set; }
        public LeverBlock Block { get; private set; }
        private Point TextureOffset { get; set; }
        private Sprite[] Sprites { get; set; }
        private Lever ParentLever { get; set; }
        
        public LeverZone(
            LeverBlock block,
            Lever parentLever,
            int screen,
            Point textureOffset,
            Texture2D[] textures)
        {
            Block = block;
            ParentLever = parentLever;
            Screen = screen;
            TextureOffset = textureOffset;
            
            Sprites = new Sprite[textures.Length];
            for (var i = 0; i < textures.Length; i++)
            {
                Sprites[i] = Sprite.CreateSprite(textures[i]);
            }
        }

        public override void Draw()
        {
            if (EntityManager.instance.Find<PlayerEntity>() == null || LevelManager.CurrentScreen.GetIndex0() != Screen)
            {
                return;
            }
            
            if (Sprites.Length != 2)
            {
                return;
            }
            var i = ParentLever.GetVisualState() ? 0 : 1;
            Sprites[i].Draw(Camera.TransformVector2((Block.GetRect().Location - TextureOffset).ToVector2()));
        }
    }
}