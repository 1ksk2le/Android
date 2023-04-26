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
        private Vector2 movementSmallPos;
        private Vector2 attackSmallPos;
        private Vector2 movementLargePos;
        private Vector2 attackLargePos;

        public Joystick(Game game, Player player) : base(game)
        {
            var viewport = GraphicsDevice.Viewport;

            this.player = player;
            movementSmallPos = new Vector2(0, 0);
            attackSmallPos = new Vector2(0, 0);
            movementLargePos = new Vector2(300, viewport.Height - 300);
            attackLargePos = new Vector2(viewport.Width - 300, viewport.Height - 300);
        }

        public override void Draw(GameTime gameTime)
        {
            var spriteBatch = Game.Services.GetService<SpriteBatch>();

            spriteBatch.Begin();

            TouchCollection touchCollection = TouchPanel.GetState();
            foreach (TouchLocation touchLocation in touchCollection)
            {
                if (touchLocation.Position.X < GraphicsDevice.Viewport.Width / 2)
                {
                    spriteBatch.Draw(TEX_Joystick_Border, movementLargePos - new Vector2(TEX_Joystick_Border.Width / 2, TEX_Joystick_Border.Height / 2), Color.LightPink);
                    spriteBatch.Draw(TEX_Joystick, movementSmallPos - new Vector2(TEX_Joystick.Width / 2, TEX_Joystick.Height / 2), Color.White);
                }
                if (touchLocation.Position.X >= GraphicsDevice.Viewport.Width / 2)
                {
                    spriteBatch.Draw(TEX_Joystick_Border, attackLargePos - new Vector2(TEX_Joystick_Border.Width / 2, TEX_Joystick_Border.Height / 2), Color.LightCyan);
                    spriteBatch.Draw(TEX_Joystick, attackSmallPos - new Vector2(TEX_Joystick.Width / 2, TEX_Joystick.Height / 2), Color.White);
                }
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public override void Update(GameTime gameTime)
        {
            TouchCollection touchCollection = TouchPanel.GetState();

            // Update the movement joystick
            foreach (TouchLocation touchLocation in touchCollection)
            {
                if (touchLocation.Position.X < GraphicsDevice.Viewport.Width / 2)
                {
                    Vector2 direction = touchLocation.Position - movementLargePos;
                    float distance = direction.Length();
                    direction.Normalize();

                    float maxDistance = TEX_Joystick_Border.Width / 2 - TEX_Joystick.Width / 2;
                    if (distance > maxDistance)
                    {
                        Vector2 maxPosition = movementLargePos + direction * maxDistance;
                        movementSmallPos = Vector2.Clamp(touchLocation.Position, maxPosition - new Vector2(TEX_Joystick.Width / 2), maxPosition + new Vector2(TEX_Joystick.Width / 2));
                    }
                    else
                    {
                        movementSmallPos = touchLocation.Position;
                    }

                    Vector2 movementDirection = movementSmallPos - movementLargePos;
                    movementDirection.Normalize();
                    player.direction = movementDirection;
                    player.position += movementDirection * player.speed / 100f;

                    if (distance > 0 && !player.isAttacking)
                    {
                        float angle = MathHelper.ToDegrees((float)Math.Atan2(direction.Y, direction.X));
                        angle += 180;  // Add 180 degrees
                        if (angle >= 90 && angle <= 270)
                        {
                            player.facedWay = PlayerDirection.Right;
                        }
                        else
                        {
                            player.facedWay = PlayerDirection.Left;
                        }
                    }
                }
                if (touchLocation.Position.X >= GraphicsDevice.Viewport.Width / 2)
                {
                    Vector2 direction = touchLocation.Position - attackLargePos;
                    float distance = direction.Length();
                    direction.Normalize();

                    float maxDistance = TEX_Joystick_Border.Width / 2 - TEX_Joystick.Width / 2;
                    if (distance > maxDistance)
                    {
                        Vector2 maxPosition = attackLargePos + direction * maxDistance;
                        attackSmallPos = Vector2.Clamp(touchLocation.Position, maxPosition - new Vector2(TEX_Joystick.Width / 2), maxPosition + new Vector2(TEX_Joystick.Width / 2));
                    }
                    else
                    {
                        attackSmallPos = touchLocation.Position;
                    }

                    Vector2 attackingDirection = attackSmallPos - attackLargePos;
                    attackingDirection.Normalize();
                    player.attackDireciton = attackingDirection;
                    player.isAttacking = true;

                    if (distance > 0 && player.isAttacking)
                    {
                        float angle = MathHelper.ToDegrees((float)Math.Atan2(direction.Y, direction.X));
                        angle += 180;  // Add 180 degrees
                        if (angle >= 90 && angle <= 270)
                        {
                            player.facedWay = PlayerDirection.Right;
                        }
                        else
                        {
                            player.facedWay = PlayerDirection.Left;
                        }
                    }
                    if (touchLocation.State == TouchLocationState.Released)
                    {
                        player.isAttacking = false;
                    }
                }
            }

            if (touchCollection.Count == 0)
            {
                player.isAttacking = false;
            }

            base.Update(gameTime);
        }
    }
}