using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using static MobileGame.TextureLoader;

namespace MobileGame
{
    public class Projectile
    {
        public int ID { get; private set; }
        public int AI { get; private set; }
        public Texture2D Texture { get; private set; }
        public int MinDamage { get; set; }
        public int MaxDamage { get; set; }
        public int LifeTime { get; set; }
        public Vector2 Velocity { get; set; }
        public Vector2 Direction { get; set; }
        public Vector2 Position { get; set; }
        public bool IsFriendly { get; set; }
        public bool IsEnemy { get; set; }
        public float Scale { get; set; }
        public float Rotation { get; set; }
        public float Speed { get; set; }
        public Color Color { get; set; }
        public TimeSpan lifeTime;
        public TimeSpan elapsedTime;

        public Projectile(int id, int ai, Texture2D texture, int minDamage, int maxDamage, int lifeTime, Vector2 velocity, Vector2 direction, Vector2 position, bool isFriendly, bool isEnemy, float scale, float rotation, float speed, Color color)
        {
            ID = id;
            AI = ai;
            Texture = texture;
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            LifeTime = lifeTime;
            Velocity = velocity;
            Direction = direction;
            Position = position;
            IsEnemy = isEnemy;
            IsFriendly = isFriendly;
            Scale = scale;
            Rotation = rotation;
            Speed = speed;
            Color = color;

            this.lifeTime = TimeSpan.FromSeconds(lifeTime);
            this.elapsedTime = TimeSpan.Zero;
        }

        public void Update(GameTime gameTime)
        {
            // Update the position of the projectile based on its velocity
            Position += Velocity * Direction * Speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            elapsedTime += gameTime.ElapsedGameTime;

            // Check if the projectile has exceeded its lifetime
        }
    }

    public class ProjectileManager
    {
        private List<Projectile> projectiles;
        private Dictionary<int, Texture2D> projectileTextures;

        public ProjectileManager()
        {
            projectiles = new List<Projectile>();
            projectileTextures = new Dictionary<int, Texture2D>();
        }

        public void AddProjectile(int id, int ai, int minDamage, int maxDamage, int lifeTime, Vector2 velocity, Vector2 direction, Vector2 position, bool friendly, bool enemy, float scale, float rotation, float speed, Color color)
        {
            // Get the texture for the given ID
            Texture2D texture = projectileTextures[id];

            // Create a new projectile and add it to the list
            Projectile projectile = new Projectile(id, ai, texture, minDamage, maxDamage, lifeTime, velocity, direction, position, friendly, enemy, scale, rotation, speed, color);
            projectiles.Add(projectile);
        }

        public void LoadContent(ContentManager content)
        {
            projectileTextures.Add(0, TEX_Joystick); // ID 1 represents basicProjectileTexture
            projectileTextures.Add(1, TEX_Joystick); // ID 2 represents specialProjectileTexture
        }

        public void Update(GameTime gameTime)
        {
            // Update all the projectiles
            foreach (Projectile projectile in projectiles)
            {
                projectile.Update(gameTime);
            }
            for (int i = projectiles.Count - 1; i >= 0; i--)
            {
                Projectile projectile = projectiles[i];
                projectile.Update(gameTime);
                if (projectile.elapsedTime >= projectile.lifeTime)
                {
                    // Remove the projectile
                    projectiles.RemoveAt(i);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();
            foreach (Projectile projectile in projectiles)
            {
                spriteBatch.Draw(projectile.Texture, projectile.Position, null, projectile.Color, projectile.Rotation, Vector2.Zero, projectile.Scale, SpriteEffects.None, 1f);
            }
            spriteBatch.End();
        }
    }
}