using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using static MobileGame.TextureLoader;

namespace MobileGame
{
    public class Item
    {
        public Texture2D Texture { get; private set; }
        public int ID { get; private set; }
        public int ShootType { get; set; }
        public int MinDamage { get; set; }
        public int MaxDamage { get; set; }
        public int UseTime { get; set; }
        public string Name { get; set; }
        public bool CanShoot { get; set; }

        public Item(Texture2D texture, int id)
        {
            ID = id;
            Texture = texture;
        }

        public void Update(GameTime gameTime)
        {
            if (ID == 1) { MaxDamage = 999; } //ESYALARIN STATLARINI BURADAN DİNAMİK OLARAK DEĞİŞTİREBİLİYORUZ
        }
    }

    public class ItemManager
    {
        private List<Item> items;
        private Dictionary<int, (Texture2D texture, int shootType, int minDmg, int maxDmg, int useTime, string name, bool canShoot)> itemLoader;

        public ItemManager()
        {
            items = new List<Item>();
            itemLoader = new Dictionary<int, (Texture2D, int, int, int, int, string, bool)>();
        }

        public void NewItem(int id)
        {
            (Texture2D texture, int shootType, int minDmg, int maxDmg, int useTime, string name, bool canShoot) itemData = itemLoader[id];
            Item newItem = new Item(itemData.texture, id)
            {
                ShootType = itemData.shootType,
                MinDamage = itemData.minDmg,
                MaxDamage = itemData.maxDmg,
                UseTime = itemData.useTime,
                Name = itemData.name,
                CanShoot = itemData.canShoot
            };
            items.Add(newItem);
        }


        public void LoadContent(ContentManager content)
        {
            itemLoader.Add(0, (WEP_Test_Wand, 0, 10, 20, 200, "Test Wand", true));
            itemLoader.Add(1, (WEP_Test_Sword, 0, 10, 20, 200, "Test Sword", false));

            foreach (int id in itemLoader.Keys)
            {
                NewItem(id);
            }
        }
        public void Update(GameTime gameTime)
        {
            foreach (Item item in items)
            {
                item.Update(gameTime);
            }
        }

        public Item GetItem(int id)
        {
            foreach (Item item in items)
            {
                if (item.ID == id)
                {
                    return item;
                }
            }

            throw new ArgumentException($"Bu {id} numarali id'ye sahip esya yok!", nameof(id));
        }

    }

}
