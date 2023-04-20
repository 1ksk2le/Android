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
            var viewport = GraphicsDevice.Viewport;

            this.player = player;
            isTouching = false;
            smallCirclePosition = new Vector2(0, 0);
            largeCirclePosition = new Vector2(300, viewport.Height - 300);
        }

        public override void Draw(GameTime gameTime)
        {
            var spriteBatch = Game.Services.GetService<SpriteBatch>();

            spriteBatch.Begin();

            if (isTouching)
            {
                spriteBatch.Draw(TEX_Joystick_Border, largeCirclePosition - new Vector2(TEX_Joystick_Border.Width / 2, TEX_Joystick_Border.Height / 2), Color.White);
                spriteBatch.Draw(TEX_Joystick, smallCirclePosition - new Vector2(TEX_Joystick.Width / 2, TEX_Joystick.Height / 2), Color.White);
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

                Vector2 direction = touchLocation.Position - largeCirclePosition;
                float distance = direction.Length();
                direction.Normalize();

                //EKRANDA DOKUNDUĞUMUZ YERİN EKRANDA OLUP OLMADIĞINA BAK
                if (touchLocation.State == TouchLocationState.Moved && distance > 0)
                {
                    //UFAK ÇEMBERİN BÜYÜK ÇEMBERDEN NE KADAR UZAKLAŞABİLECEĞİNE BAK
                    float maxDistance = TEX_Joystick_Border.Width / 2 - TEX_Joystick.Width / 2;

                    //UFAK ÇEMBERİN DİĞER ÇEMBER İÇİNDEKİ HAREKETİNİ LİMİTLE
                    if (distance > maxDistance)
                    {
                        Vector2 maxPosition = largeCirclePosition + direction * maxDistance;

                        smallCirclePosition = Vector2.Clamp(touchLocation.Position, maxPosition - new Vector2(TEX_Joystick.Width / 2), maxPosition + new Vector2(TEX_Joystick.Width / 2));
                    }
                    else
                    {
                        smallCirclePosition = touchLocation.Position;
                    }

                    // OYUNCUNUN HAREKET YÖNÜNÜNÜ HESAPLA
                    Vector2 movementDirection = smallCirclePosition - largeCirclePosition;
                    movementDirection.Normalize();

                    //POZİSYONU GÜNCELLE
                    player.position += movementDirection * player.speed / 100f;
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