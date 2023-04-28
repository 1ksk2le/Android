using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MobileGame
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private Player player;
        private Joystick joystick;
        private ProjectileManager projectile;
        private WeaponManager weapon;

        public static SpriteFont TestFont;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            TextureLoader.LoadAllTextures(Services);

            projectile = new ProjectileManager();
            weapon = new WeaponManager();
            player = new Player(this, projectile, weapon);
            joystick = new Joystick(this, player);
            Components.Add(joystick);
            Components.Add(player);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(spriteBatch);
            TestFont = Content.Load<SpriteFont>("Font_Test");
            projectile.LoadContent(Content);
            weapon.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            projectile.Update(gameTime);
            weapon.Update(gameTime);
            player.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGray);

            projectile.Draw(spriteBatch);
            player.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}