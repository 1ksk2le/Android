using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MobileGame
{
    public static class TextureLoader
    {
        private static ContentManager contentManager;
        public static Texture2D TEX_Player { get; private set; }
        public static Texture2D TEX_Joystick { get; private set; }
        public static void LoadAllTextures(GraphicsDevice graphicsDevice, GameServiceContainer services)
        {
            contentManager = new ContentManager(services, "Content");

            TEX_Player = contentManager.Load<Texture2D>("Sprites/Player_Base");
            TEX_Joystick = contentManager.Load<Texture2D>("Sprites/Joystick");
        }
    }
}