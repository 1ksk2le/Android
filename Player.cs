using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using static MobileGame.Game1;
using static MobileGame.TextureLoader;

namespace MobileGame
{
    public enum PlayerDirection
    {
        Left,
        Right
    }

    public class Player : DrawableGameComponent
    {
        private const int frameCount = 4;
        private static int currentFrame = 0;
        private ProjectileManager projectile;
        private WeaponManager item;
        private double shootTimer = 0;
        public PlayerDirection facedWay { get; set; } = PlayerDirection.Right;
        // Fields
        public Vector2 direction;
        public Vector2 attackDireciton;
        public Vector2 position;
        public Vector2 velocity;
        public float speed = 200f;
        public bool isAttacking;
        public int EquippedWeaponID;
        public int EquippedWeaponModifier;

        private Weapon EquippedWeapon => item.GetItem(EquippedWeaponID);

        private Texture2D weaponTexture;
        // Constructor
        public Player(Game game, ProjectileManager projectileManager, WeaponManager item) : base(game)
        {
            this.item = item;
            this.projectile = projectileManager;
            attackDireciton = Vector2.Zero;
            isAttacking = false;
        }

        // Load content
        protected override void LoadContent()
        {
            position = new Vector2(Game.GraphicsDevice.Viewport.Width / 2, Game.GraphicsDevice.Viewport.Height / 2);
        }

        // Update method
        public override void Update(GameTime gameTime)
        {

            Animate(gameTime);
            Shoot(gameTime, 1f);



            base.Update(gameTime);

            EquippedWeaponID = 1;
            EquippedWeapon.Modifier = 1;
            EquippedWeapon.Enchant = 1;
            //position = new Vector2(100, 100);
        }

        // Draw method
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = Game.Services.GetService<SpriteBatch>();

            Rectangle destinationRect = new Rectangle((int)position.X, (int)position.Y, TEX_Player.Width, TEX_Player.Height / frameCount);
            Rectangle animRect = new Rectangle(0, TEX_Player.Height / frameCount * currentFrame, TEX_Player.Width, TEX_Player.Height / frameCount);

            spriteBatch.Begin();

            spriteBatch.Draw(TEX_Player, position, animRect, Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 1f);
            if (EquippedWeapon.Type == "Sword")
            {
                AnimateSwords(spriteBatch, gameTime, EquippedWeapon);
            }
            //spriteBatch.Draw(EquippedWeapon.Texture, position, null, Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);


            spriteBatch.DrawString(TestFont, EquippedWeapon.Name, position + new Vector2(0, 200), Color.Black, 0, Vector2.Zero, 2f, SpriteEffects.None, 1f);
            spriteBatch.DrawString(TestFont, EquippedWeapon.MinDamage.ToString(), position + new Vector2(0, 220), Color.Black, 0, Vector2.Zero, 2f, SpriteEffects.None, 1f);
            spriteBatch.DrawString(TestFont, EquippedWeapon.MaxDamage.ToString(), position + new Vector2(0, 240), Color.Black, 0, Vector2.Zero, 2f, SpriteEffects.None, 1f);
            spriteBatch.DrawString(TestFont, EquippedWeapon.UseTime.ToString(), position + new Vector2(0, 260), Color.Black, 0, Vector2.Zero, 2f, SpriteEffects.None, 1f);
            spriteBatch.End();


            base.Draw(gameTime);
        }

        float animationTime = 0;
        float currentAngle = 0f;
        bool switchAnims = false;
        internal void AnimateSwords(SpriteBatch spriteBatch, GameTime gameTime, Weapon equippedWeapon)
        {

            Vector2 origin = new Vector2(EquippedWeapon.Texture.Width / 2, EquippedWeapon.Texture.Height);
            float startingAngle = (facedWay == PlayerDirection.Right) ? MathHelper.ToRadians(-45) : MathHelper.ToRadians(45);
            float endingAngle = (facedWay == PlayerDirection.Right) ? MathHelper.ToRadians(135) : MathHelper.ToRadians(-135);
            float holdingAngle = (facedWay == PlayerDirection.Right) ? MathHelper.ToRadians(150) : MathHelper.ToRadians(-150);
            if (isAttacking)
            {
                animationTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

                float t = MathHelper.Clamp(animationTime / equippedWeapon.AnimationSpeed, 0, 1);
                currentAngle = MathHelper.Lerp(startingAngle, endingAngle, t);

                if (t == 1)
                {
                    switchAnims = false;
                    animationTime = 0;
                }

                spriteBatch.Draw(EquippedWeapon.Texture, position + new Vector2(TEX_Player.Width / 2 + equippedWeapon.Texture.Width / 2, 40), null, Color.White, currentAngle, origin, 2f, SpriteEffects.None, 0f);
            }
            else
            {
                spriteBatch.Draw(EquippedWeapon.Texture, position + new Vector2(TEX_Player.Width / 2 + equippedWeapon.Texture.Width / 2, 40), null, Color.White, startingAngle + holdingAngle, origin, 2f, SpriteEffects.None, 0f);
            }
        }


        internal void Shoot(GameTime gameTime, float damageModifier)
        {
            if (isAttacking && gameTime.TotalGameTime.TotalMilliseconds - shootTimer >= EquippedWeapon.UseTime && EquippedWeapon.CanShoot)
            {
                projectile.AddProjectile(EquippedWeapon.ShootType,
                    0,
                    (int)(EquippedWeapon.MinDamage * damageModifier),
                    (int)(EquippedWeapon.MaxDamage * damageModifier),
                    2,
                    Vector2.One,
                    attackDireciton,
                    position + new Vector2(TEX_Player.Width / 2, TEX_Player.Height / 2 / frameCount),
                    true,
                    false,
                    0.5f,
                    0f,
                    500f,
                    Color.White);

                shootTimer = gameTime.TotalGameTime.TotalMilliseconds;
            }
        }
        internal void Animate(GameTime gameTime)
        {
            if (facedWay == PlayerDirection.Left)
                currentFrame = 3;
            if (facedWay == PlayerDirection.Right)
                currentFrame = 2;
        }
    }
}