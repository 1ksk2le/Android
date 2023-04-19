using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using static MobileGame.TextureLoader;

namespace MobileGame
{
    public class Joystick : DrawableGameComponent
    {
        public Vector2 position;
        private Player player;
        private Vector2 smallCirclePosition;
        private Vector2 largeCirclePosition;
        private bool isTouching;

        public Joystick(Game game, Player player) : base(game)
        {
            // Get the size of the screen
            var viewport = GraphicsDevice.Viewport;

            this.player = player;
            isTouching = false;
            smallCirclePosition = new Vector2(0, 0);
            largeCirclePosition = new Vector2(viewport.Width - 200, viewport.Height - 200);
        }

        public override void Draw(GameTime gameTime)
        {
            // Get the SpriteBatch object from the Game instance
            var spriteBatch = Game.Services.GetService<SpriteBatch>();

            // Draw the texture for the joystick at the position you set earlier
            spriteBatch.Begin();
            spriteBatch.Draw(TEX_Joystick_Border, largeCirclePosition - new Vector2(TEX_Joystick_Border.Width / 2, TEX_Joystick_Border.Height / 2), Color.White);
            if (isTouching)
            {
                spriteBatch.Draw(TEX_Joystick, smallCirclePosition - new Vector2(TEX_Joystick.Width / 2, TEX_Joystick.Height / 2), Color.White);
            }
            else
            {
                spriteBatch.Draw(TEX_Joystick, largeCirclePosition - new Vector2(TEX_Joystick.Width / 2, TEX_Joystick.Height / 2), Color.White);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            TouchCollection touchCollection = TouchPanel.GetState();

            if (touchCollection.Count > 0)
            {
                isTouching = true;
                TouchLocation touchLocation = touchCollection[0];

                // Calculate the direction vector and distance from the center of the joystick
                Vector2 direction = touchLocation.Position - largeCirclePosition;
                float distance = direction.Length();
                direction.Normalize();

                // Check if the touch location is within the screen boundaries
                if (touchLocation.State == TouchLocationState.Moved && distance > 0)
                {
                    // Calculate the maximum distance the small circle can move from the center of the large circle
                    float maxDistance = TEX_Joystick_Border.Width / 2 - TEX_Joystick.Width / 2;

                    // Limit the movement of the small circle within the boundaries of the larger circle
                    if (distance > maxDistance)
                    {
                        Vector2 maxPosition = largeCirclePosition + direction * maxDistance;

                        smallCirclePosition = Vector2.Clamp(touchLocation.Position, maxPosition - new Vector2(TEX_Joystick.Width / 2), maxPosition + new Vector2(TEX_Joystick.Width / 2));
                    }
                    else
                    {
                        smallCirclePosition = touchLocation.Position;
                    }

                    // Calculate the movement direction of the player
                    Vector2 movementDirection = smallCirclePosition - largeCirclePosition;
                    movementDirection.Normalize();

                    // Update the player's position based on the movement direction and predetermined speed
                    player.position += movementDirection * player.speed;
                }

                if (distance > 0)
                {
                    float angle = (float)Math.Atan2(-direction.Y, direction.X);

                    if (angle >= -MathHelper.PiOver4 && angle < MathHelper.PiOver4)
                    {
                        player.direction = PlayerDirection.Right;
                    }
                    else if (angle >= MathHelper.PiOver4 && angle < 3 * MathHelper.PiOver4)
                    {
                        player.direction = PlayerDirection.Up;
                    }
                    else if (angle >= 3 * MathHelper.PiOver4 || angle < -3 * MathHelper.PiOver4)
                    {
                        player.direction = PlayerDirection.Left;
                    }
                    else
                    {
                        player.direction = PlayerDirection.Down;
                    }
                }
            }
            else
            {
                isTouching = false;
            }

            base.Update(gameTime);
        }

    }
}