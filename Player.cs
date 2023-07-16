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
        private const int frameCount = 1;
        private static int currentFrame = 0;
        private ProjectileManager projectile;
        private ItemManager item;
        private Inventory inventory;
        private double shootTimer = 0;
        public PlayerDirection facedWay { get; set; } = PlayerDirection.Right;

        // Fields
        public Vector2 movementDirection;

        public Vector2 attackDirection;
        public Vector2 position;
        public Vector2 velocity;
        public float speed = 200f;
        public bool isAttacking;
        public int EquippedWeaponID;
        public int EquippedChestplateID;
        public int EquippedWeaponModifier;

        private const float baseScale = 2f;

        private Item EquippedWeapon => inventory.GetItemAt(0, 0);
        private Item EquippedChestplate => item.GetItem(EquippedChestplateID);

        private Texture2D weaponTexture;

        // Constructor
        public Player(Game game, ProjectileManager projectileManager, ItemManager item, Inventory inventory) : base(game)
        {
            this.item = item;
            this.inventory = inventory;
            this.projectile = projectileManager;
            attackDirection = Vector2.Zero;
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
            speed = EquippedChestplate.SpeedPenalty * 200f;
            Animate(gameTime);
            //Shoot(gameTime, EquippedWeapon.ProjectileDamage);


            base.Update(gameTime);

            EquippedChestplateID = 0;
            //position = new Vector2(100, 100);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = Game.Services.GetService<SpriteBatch>();

            Rectangle destinationRect = new Rectangle((int)position.X, (int)position.Y, TEX_Player.Width, TEX_Player.Height / frameCount);
            Rectangle animRect = new Rectangle(0, TEX_Player.Height / frameCount * currentFrame, TEX_Player.Width, TEX_Player.Height / frameCount);

            spriteBatch.Begin();

            if (!isAttacking)
            {
                float rot = (facedWay == PlayerDirection.Right) ? MathHelper.ToRadians(-135) : MathHelper.ToRadians(135);
                spriteBatch.Draw(EquippedWeapon.Texture, position + new Vector2(EquippedWeapon.Texture.Width, EquippedWeapon.Texture.Height / 2), null, Color.White, rot, new Vector2(TEX_Player.Width / 2, TEX_Player.Height / 2), baseScale, SpriteEffects.None, 0f);
            }
            spriteBatch.Draw(TEX_Player, position, null, Color.White, 0f, Vector2.Zero, 2f, (facedWay == PlayerDirection.Right) ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 1f);
            if (EquippedChestplateID == 100)
            {
                spriteBatch.Draw(TEX_Player, position, null, Color.Red, 0f, Vector2.Zero, 2f, (facedWay == PlayerDirection.Right) ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 1f);
            }

            if (isAttacking)
            {
                if (EquippedWeapon.Type == "Sword")
                {
                    AnimateSwords(spriteBatch, gameTime, EquippedWeapon);
                }
            }

            //spriteBatch.Draw(EquippedWeapon.Texture, position, null, Color.White, 0f, Vector2.Zero, 3f, SpriteEffects.None, 1f);

            spriteBatch.DrawString(TestFont, "[" + EquippedWeapon.Name + "]", position + new Vector2(0, 100), EquippedWeapon.NameColor, 0, Vector2.Zero, baseScale * 1.5f, SpriteEffects.None, 1f);
            spriteBatch.DrawString(TestFont, EquippedWeapon.TotalMinDamage.ToString() + "-" + EquippedWeapon.TotalMaxDamage.ToString() + " " + EquippedWeapon.DamageType.ToString() + " damage", position + new Vector2(0, 135), Color.Black, 0, Vector2.Zero, baseScale * 1.5f, SpriteEffects.None, 1f);
            spriteBatch.DrawString(TestFont, "Use Time: " + ((float)(EquippedWeapon.UseTime / 1000)).ToString() + " seconds", position + new Vector2(0, 170), Color.Black, 0, Vector2.Zero, baseScale * 1.5f, SpriteEffects.None, 1f);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private float animationTime = 0;
        private float currentAngle = 0f;

        internal void AnimateSwords(SpriteBatch spriteBatch, GameTime gameTime, Item equippedWeapon)
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
                    animationTime = 0;
                }

                spriteBatch.Draw(EquippedWeapon.Texture, position + new Vector2(TEX_Player.Width / 2 + equippedWeapon.Texture.Width / 2, 40), null, Color.White, currentAngle, origin, baseScale, SpriteEffects.None, 0f);
            }
        }

        internal void Shoot(GameTime gameTime, float damageModifier)
        {
            if (isAttacking && gameTime.TotalGameTime.TotalMilliseconds - shootTimer >= EquippedWeapon.UseTime && EquippedWeapon.CanShoot)
            {
                projectile.AddProjectile(EquippedWeapon.ShootType,
                    0,
                    (int)(EquippedWeapon.TotalMinDamage * damageModifier),
                    (int)(EquippedWeapon.TotalMaxDamage * damageModifier),
                    EquippedWeapon.ProjectileLifetime,
                    Vector2.One,
                    attackDirection,
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
                currentFrame = 1;
            if (facedWay == PlayerDirection.Right)
                currentFrame = 0;
        }
    }
}