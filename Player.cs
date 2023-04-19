using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static MobileGame.TextureLoader;

namespace MobileGame
{
    public class Player : DrawableGameComponent
    {
        // Fields
        public Vector2 position;
        public Vector2 velocity;
        public float speed = 0.3f;

        const int frameCount = 4;
        static int currentFrame = 0;

        // Constructor
        public Player(Game game) : base(game)
        {
        }

        // Load content
        protected override void LoadContent()
        {
            position = new Vector2(Game.GraphicsDevice.Viewport.Width / 2, Game.GraphicsDevice.Viewport.Height / 2);
        }

        // Update method
        public override void Update(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            Movement(gameTime, keyboardState);
            Animate(gameTime, keyboardState);
            

            base.Update(gameTime);
        }

        // Draw method
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = Game.Services.GetService<SpriteBatch>();

            Rectangle destinationRect = new Rectangle((int)position.X, (int)position.Y, TEX_Player.Width, TEX_Player.Height / frameCount);
            Rectangle animRect = new Rectangle(0, TEX_Player.Height / frameCount * currentFrame, TEX_Player.Width, TEX_Player.Height / frameCount);

            spriteBatch.Begin();
            spriteBatch.Draw(TEX_Player, destinationRect, animRect, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        internal void Movement(GameTime gameTime, KeyboardState key)
        {
            velocity = Vector2.Zero;
            if (key.IsKeyDown(Keys.Left))
                velocity.X -= speed;
            if (key.IsKeyDown(Keys.Right))
                velocity.X += speed;
            if (key.IsKeyDown(Keys.Up))
                velocity.Y -= speed;
            if (key.IsKeyDown(Keys.Down))
                velocity.Y += speed;

            position += velocity;
        }

        internal void Animate(GameTime gameTime, KeyboardState key)
        {
            if (key.IsKeyDown(Keys.Left))
                currentFrame = 3;
            if (key.IsKeyDown(Keys.Right))
                currentFrame = 2;
            if (key.IsKeyDown(Keys.Up))
                currentFrame = 1;
            if (key.IsKeyDown(Keys.Down))
                currentFrame = 0;
        }
    }

}
