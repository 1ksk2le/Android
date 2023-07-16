using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace MobileGame
{
    public class Inventory
    {
        public const int NumRows = 4;
        public const int NumColumns = 6;

        private Item[,] slots;

        public Inventory()
        {
            slots = new Item[NumRows, NumColumns];
        }

        public void AddItem(Item item)
        {
            // Find an empty slot and add the item
            for (int row = 0; row < NumRows; row++)
            {
                for (int col = 0; col < NumColumns; col++)
                {
                    if (slots[row, col] == null)
                    {
                        slots[row, col] = item;
                        return;
                    }
                }
            }
        }

        public void AddItemToSlot(Item item, int row, int col)
        {
            slots[row, col] = item;
        }

        public void RemoveItem(int row, int col)
        {
            slots[row, col] = null;
        }

        public Item GetItemAt(int row, int col)
        {
            return slots[row, col];
        }
    }
    public class InventoryRenderer
    {
        private Inventory inventory;
        private Texture2D slotTexture;
        private Texture2D slotOutlineTexture;
        private Vector2 position;

        public InventoryRenderer(Inventory inventory, Texture2D slotTexture, Texture2D slotOutlineTexture, Vector2 position)
        {
            if (inventory == null)
                throw new ArgumentNullException(nameof(inventory));

            this.inventory = inventory;
            this.slotTexture = slotTexture;
            this.slotOutlineTexture = slotOutlineTexture;
            this.position = position;
        }

        private float slotDim = 144f;
        public void Draw(SpriteBatch spriteBatch)
        {
            for (int row = 0; row < Inventory.NumRows; row++)
            {
                for (int col = 0; col < Inventory.NumColumns; col++)
                {
                    Vector2 slotPosition = position + new Vector2(col * (slotDim + 8), row * (slotDim + 8));

                    Item item = inventory.GetItemAt(row, col);
                    if (item != null)
                    {
                        //spriteBatch.Draw(slotOutlineTexture, slotPosition, Color.White);
                        spriteBatch.Draw(slotTexture, slotPosition, item.NameColor);
                        float scale = 90f / Math.Max(item.Texture.Width, item.Texture.Height);
                        int width = (int)(item.Texture.Width * scale);
                        int height = (int)(item.Texture.Height * scale);
                        Vector2 itemPosition = slotPosition + new Vector2((slotDim - width) / 2 + 42, (slotDim - height) / 2 - 4);
                        Rectangle destinationRectangle = new Rectangle((int)itemPosition.X, (int)itemPosition.Y, width, height);
                        spriteBatch.Draw(item.Texture, destinationRectangle, null, Color.White, MathHelper.ToRadians(45f), Vector2.Zero, SpriteEffects.None, 0f);
                        if (item.Modifier != 0)
                        {
                            spriteBatch.Draw(item.Texture, destinationRectangle, null, Color.Red, MathHelper.ToRadians(45f), Vector2.Zero, SpriteEffects.None, 0f);
                        }
                    }
                    else
                    {
                        // spriteBatch.Draw(slotOutlineTexture, slotPosition, Color.White);
                        spriteBatch.Draw(slotTexture, slotPosition, Color.White);
                    }
                }
            }
        }

    }
}
