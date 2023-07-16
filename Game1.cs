using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using static MobileGame.TextureLoader;

namespace MobileGame
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        private Player player;
        private Joystick joystick;
        private ProjectileManager projectile;
        private ItemManager item;
        private Inventory inventory;

        public static SpriteFont TestFont;

        public Game1()
        {
            IsFixedTimeStep = false;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            TextureLoader.LoadAllTextures(Services);

            projectile = new ProjectileManager();
            inventory = new Inventory();
            item = new ItemManager(player);
            player = new Player(this, projectile, item, inventory);
            joystick = new Joystick(this, player);
            Components.Add(joystick);
            Components.Add(player);



            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(spriteBatch);
            TestFont = Content.Load<SpriteFont>("Font_Test");
            projectile.LoadContent(Content);
            item.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            projectile.Update(gameTime);
            item.Update(gameTime);
            player.Update(gameTime);


            inventory.AddItemToSlot(new Item(0, 1, 1), 0, 0);
            inventory.AddItemToSlot(new Item(0, 1, 0), 3, 3);
            inventory.AddItemToSlot(new Item(0, 0, 0), 3, 4);
            //inventory.AddItem(new Item(1, 0, 0));

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGray);

            projectile.Draw(spriteBatch);
            player.Draw(gameTime);



            base.Draw(gameTime);

            spriteBatch.Begin();
            InventoryRenderer inventoryRenderer = new InventoryRenderer(inventory, TEX_Inventory_Slot, TEX_Inventory_Slot_Outline, new Vector2(200, 200));
            inventoryRenderer.Draw(spriteBatch);
            spriteBatch.DrawString(TestFont, "FPS: " + ((int)(1 / (float)gameTime.ElapsedGameTime.TotalSeconds)).ToString() + " FPS", new Vector2(50, 50), Color.Black, 0, Vector2.Zero, 3f, SpriteEffects.None, 1f);
            spriteBatch.End();
        }
    }
}