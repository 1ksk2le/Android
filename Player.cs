using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static MobileGame.TextureLoader;

namespace MobileGame
{
    public enum PlayerDirection
    {
        Up,
        Down,
        Left,
        Right
    }

    public class Player : DrawableGameComponent
    {
        // Fields
        public Vector2 position;

        public Vector2 velocity;
        public float speed = 200f;

        private const int frameCount = 4;
        private static int currentFrame = 0;
        public PlayerDirection direction { get; set; } = PlayerDirection.Down;

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

            Animate(gameTime);

            base.Update(gameTime);
        }

        // Draw method
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = Game.Services.GetService<SpriteBatch>();

            Rectangle destinationRect = new Rectangle((int)position.X, (int)position.Y, TEX_Player.Width, TEX_Player.Height / frameCount);
            Rectangle animRect = new Rectangle(0, TEX_Player.Height / frameCount * currentFrame, TEX_Player.Width, TEX_Player.Height / frameCount);

            spriteBatch.Begin();
            spriteBatch.Draw(TEX_Player, position, animRect, Color.White, 0f, Vector2.Zero, 2.5f, SpriteEffects.None, 1f);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        internal void Animate(GameTime gameTime)
        {
            if (direction == PlayerDirection.Left)
                currentFrame = 3;
            if (direction == PlayerDirection.Right)
                currentFrame = 2;
            if (direction == PlayerDirection.Up)
                currentFrame = 1;
            if (direction == PlayerDirection.Down)
                currentFrame = 0;
        }
    }
}