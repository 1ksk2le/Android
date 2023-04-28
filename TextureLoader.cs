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
        public static Texture2D TEX_Joystick_Border { get; private set; }
        public static Texture2D WEP_Test_Bow { get; private set; }
        public static Texture2D WEP_Test_Sword { get; private set; }
        public static Texture2D WEP_Test_Wand { get; private set; }

        public static void LoadAllTextures(GameServiceContainer services)
        {
            contentManager = new ContentManager(services, "Content");

            TEX_Player = contentManager.Load<Texture2D>("Sprites/Player_Base");
            TEX_Joystick = contentManager.Load<Texture2D>("Sprites/Joystick");
            TEX_Joystick_Border = contentManager.Load<Texture2D>("Sprites/Joystick_Border");

            WEP_Test_Bow = contentManager.Load<Texture2D>("Sprites/Weapons/Weapon_Test_Bow");
            WEP_Test_Sword = contentManager.Load<Texture2D>("Sprites/Weapons/Weapon_Test_Sword");
            WEP_Test_Wand = contentManager.Load<Texture2D>("Sprites/Weapons/Weapon_Test_Wand");
        }
    }
}