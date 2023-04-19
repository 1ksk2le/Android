using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;

namespace MobileGame
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private Player player;
        private Joystick joystick;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            TextureLoader.LoadAllTextures(Services);
            player = new Player(this);
            joystick = new Joystick(this, player);
            Components.Add(joystick);
            Components.Add(player);

            base.Initialize();

            ForceLandscapeOrientation();
        }

        private void ForceLandscapeOrientation()
        {
            graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
            graphics.ApplyChanges();

            // Wait for the device to rotate to a landscape orientation
            while (GraphicsDevice.PresentationParameters.DisplayOrientation != DisplayOrientation.LandscapeLeft && GraphicsDevice.PresentationParameters.DisplayOrientation != DisplayOrientation.LandscapeRight)
            {
                Thread.Sleep(100);
            }
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(spriteBatch);


        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            player.Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            player.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}