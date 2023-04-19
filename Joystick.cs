using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Text;
using static MobileGame.TextureLoader;

namespace MobileGame
{
    public class Joystick : DrawableGameComponent
    {
        public Vector2 position;
        private Player player;

        public Joystick(Game game, Player player) : base(game)
        {
            // Get the size of the screen
            var viewport = GraphicsDevice.Viewport;

            this.player = player;
            // Set the position of the joystick to the bottom left corner of the screen
            position = new Vector2(0, viewport.Height) + new Vector2(0, -100);
        }
        public override void Draw(GameTime gameTime)
        {
            // Get the SpriteBatch object from the Game instance
            var spriteBatch = Game.Services.GetService<SpriteBatch>();

            // Draw the texture for the joystick at the position you set earlier
            spriteBatch.Begin();
            spriteBatch.Draw(TEX_Joystick, position, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
        public override void Update(GameTime gameTime)
        {
            var touchState = TouchPanel.GetState();

            if (touchState.Count > 0)
            {
                var touchPosition = touchState[0].Position;
                var joystickVector = touchPosition - position;
                var joystickAngle = (float)Math.Atan2(joystickVector.Y, joystickVector.X);
                var joystickMagnitude = Math.Min(joystickVector.Length(), 50) / 50;

                // Calculate the player's new position based on the angle and magnitude of the joystick
                var movementVector = new Vector2((float)Math.Cos(joystickAngle), (float)Math.Sin(joystickAngle));
                player.position += movementVector * joystickMagnitude * player.speed;
            }

            base.Update(gameTime);
        }

    }

}
