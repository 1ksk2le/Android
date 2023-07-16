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
        #region WEAPON STATS
        public int ShootType { get; set; }
        public int MinDamage { get; set; }
        public int MaxDamage { get; set; }
        public int ArmorPiercing { get; set; }
        public int ShieldPiercing { get; set; }
        public int ProjectileLifetime { get; set; }
        public float AnimationSpeed { get; set; }
        public float ProjectileDamage { get; set; }
        public float ProjectileVelocity { get; set; }
        public float UseTime { get; set; }
        public string DamageType { get; set; }
        public bool CanShoot { get; set; }
        #endregion

        #region ARMOR STATS
        public int ArmorValue { get; set; }
        public int ShieldValue { get; set; }
        public float SpeedPenalty { get; set; }
        #endregion

        #region GLOBAL STATS
        public Texture2D Texture { get; private set; }
        public Color NameColor { get; set; }
        public string BaseName { get; set; }
        public string ModifierName { get; set; }
        public string EnchantName { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Modifier { get; set; }
        public int Enchant { get; set; }
        public int Value { get; set; }
        public int Rarity { get; set; }
        public int ID { get; private set; }
        #endregion

        public Item(int id, int modifier, int enchant)
        {
            ID = id;
            Texture = WEP_Test_Sword;
            BaseName = "";
            ModifierName = "";
            EnchantName = "";
            Type = "";
            DamageType = "";
            MinDamage = 0;
            MaxDamage = 0;
            UseTime = 0;
            AnimationSpeed = UseTime * 2 / 1000;
            CanShoot = false;
            ShootType = 0;
            ProjectileLifetime = 0;
            ProjectileDamage = 0f;
            ProjectileVelocity = 0f;
            Enchant = enchant;
            Modifier = modifier;
            Value = 0;
            Rarity = 0;
            ArmorPiercing = 0;
            ShieldPiercing = 0;
            ArmorValue = 0;
            ShieldValue = 0;
            SpeedPenalty = 1f;
            NameColor = Color.White;

            SetDefaultValues(id);
        }

        public float TotalMinDamage, TotalMaxDamage, AdditionalDamage, ModifierDamage, EnchantDamage;

        public void Update(GameTime gameTime)
        {
            AdditionalDamage = ModifierDamage + EnchantDamage;
            if (AdditionalDamage != 0f)
            {
                TotalMinDamage = MinDamage * AdditionalDamage;
                TotalMaxDamage = MaxDamage * AdditionalDamage;
            }
            else
            {
                TotalMinDamage = MinDamage;
                TotalMaxDamage = MaxDamage;
            }


        }

        public void SetDefaultValues(int id)
        {
            WeaponStats(id);
            ChestplateStats(id);

            switch (Rarity)
            {
                case 0:
                    NameColor = Color.White;
                    break;
                case 1:
                    NameColor = Color.LightBlue;
                    break;
                case 2:
                    NameColor = Color.LightGreen;
                    break;
                case 3:
                    NameColor = Color.DarkBlue;
                    break;
                case 4:
                    NameColor = Color.DarkGreen;
                    break;
                case 5:
                    NameColor = Color.Orange;
                    break;
                default:
                    NameColor = Color.Gray;
                    break;
            }

            switch (Modifier)
            {
                case 0:
                    ModifierName = "";
                    ModifierDamage = 0f;
                    break;

                case 1:
                    ModifierName = "Reforged";
                    ModifierDamage = 1.1f;
                    break;

                case 2:
                    ModifierName = "Broken";
                    ModifierDamage = 0.95f;
                    break;

                default:
                    ModifierName = "";
                    break;
            }

            switch (Enchant)
            {
                case 0:
                    EnchantName = "";
                    break;

                case 1:
                    EnchantName = " of Flames";
                    break;

                case 2:
                    EnchantName = " of Frost";
                    break;

                default:
                    EnchantName = "";
                    break;
            }

            if (Modifier > 0 && Enchant > 0)
            {
                Name = ModifierName + " " + BaseName + EnchantName;
            }
            else if (Modifier > 0 && Enchant <= 0)
            {
                Name = ModifierName + " " + BaseName;
            }
            else if (Enchant > 0 && Modifier <= 0)
            {
                Name = BaseName + EnchantName;
            }
            else if (Enchant <= 0 && Modifier <= 0)
            {
                Name = BaseName;
            }
        }

        public void WeaponStats(int id)
        {
            switch (id)
            {
                case 0:
                    BaseName = "Test Wand";
                    Type = "Wand";
                    DamageType = "Magic";
                    Texture = WEP_Test_Wand;
                    MinDamage = 10;
                    MaxDamage = 20;
                    UseTime = 200;
                    AnimationSpeed = UseTime * 2 / 1000;
                    CanShoot = true;
                    ShootType = 1;
                    ProjectileLifetime = 4;
                    ProjectileDamage = 1f;
                    ProjectileVelocity = 400f;
                    Value = 1000;
                    Rarity = 5;
                    break;

                case 1:
                    BaseName = "Test Sword";
                    Type = "Sword";
                    DamageType = "Melee";
                    Texture = WEP_Test_Sword;
                    MinDamage = 10;
                    MaxDamage = 20;
                    UseTime = 500;
                    AnimationSpeed = UseTime * 2 / 1000;
                    CanShoot = true;
                    ShootType = 1;
                    ProjectileLifetime = 3;
                    ProjectileDamage = 1.2f;
                    ProjectileVelocity = 500f;
                    Value = 1000;
                    Rarity = 3;
                    break;
                default:
                    break;
            }
        }

        public void ChestplateStats(int id)
        {
            switch (id)
            {
                case 100:
                    BaseName = "Test Armor";
                    Type = "Chestplate";
                    Value = 100;
                    Rarity = 2;
                    ArmorValue = 100;
                    ShieldValue = 100;
                    SpeedPenalty = 0.5f;
                    break;
                default:
                    break;
            }
        }
    }

    public class ItemManager
    {
        private List<Item> items;
        private Dictionary<int, Texture2D> itemLoader;
        private Player player;

        public ItemManager(Player player)
        {
            this.player = player;
            items = new List<Item>();
            itemLoader = new Dictionary<int, Texture2D>();
        }

        public void NewItem(int id, int modifier, int enchant)
        {
            Item newItem = new Item(id, modifier, enchant);
            items.Add(newItem);
        }

        public void LoadContent(ContentManager content)
        {
            itemLoader.Add(0, (WEP_Test_Wand));
            itemLoader.Add(1, (WEP_Test_Sword));
            itemLoader.Add(100, (WEP_Test_Sword));

            foreach (int id in itemLoader.Keys)
            {
                NewItem(id, 0, 0);
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