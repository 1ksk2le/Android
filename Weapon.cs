using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using static MobileGame.TextureLoader;

namespace MobileGame
{
    public class Weapon
    {
        public Texture2D Texture { get; private set; }
        public int ID { get; private set; }
        public int ShootType { get; set; }
        public int MinDamage { get; set; }
        public int MaxDamage { get; set; }
        public int UseTime { get; set; }
        public int Modifier { get; set; }
        public int Enchant { get; set; }
        public float AnimationSpeed { get; set; }
        public string BaseName { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public bool CanShoot { get; set; }
        public bool Equipped { get; set; }

        public Weapon(Texture2D texture, int id)
        {
            ID = id;
            Texture = texture;
        }

        public float TotalMinDamage, TotalMaxDamage, AdditionalDamage, ModifierDamage, EnchantDamage;

        public void Update(GameTime gameTime)
        {
            string ModifierName = "";
            string EnchantName = "";

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
    }

    public class WeaponManager
    {
        private List<Weapon> weapons;
        private Dictionary<int, (Texture2D texture, int shootType, int minDmg, int maxDmg, int useTime, int modifier, int enchant, float animationSpeed, string name, string type, bool canShoot, bool equipped)> weaponLoader;

        public WeaponManager()
        {
            weapons = new List<Weapon>();
            weaponLoader = new Dictionary<int, (Texture2D, int, int, int, int, int, int, float, string, string, bool, bool)>();
        }

        public void NewWeapon(int id)
        {
            (Texture2D texture, int shootType, int minDmg, int maxDmg, int useTime, int modifier, int enchant, float animationSpeed, string name, string type, bool canShoot, bool equipped) itemData = weaponLoader[id];
            Weapon newItem = new Weapon(itemData.texture, id)
            {
                ShootType = itemData.shootType,
                MinDamage = itemData.minDmg,
                MaxDamage = itemData.maxDmg,
                UseTime = itemData.useTime,
                Modifier = itemData.modifier,
                Enchant = itemData.enchant,
                AnimationSpeed = itemData.animationSpeed,
                BaseName = itemData.name,
                Type = itemData.type,
                CanShoot = itemData.canShoot,
                Equipped = itemData.equipped
            };
            weapons.Add(newItem);
        }

        public void LoadContent(ContentManager content)
        {
            weaponLoader.Add(0, (WEP_Test_Wand, 0, 10, 20, 200, 0, 0, 0.4f, "Test Wand", "Staff", true, false));
            weaponLoader.Add(1, (WEP_Test_Sword, 1, 100, 200, 500, 0, 0, 1f, "Test Sword", "Sword", true, false));

            foreach (int id in weaponLoader.Keys)
            {
                NewWeapon(id);
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (Weapon item in weapons)
            {
                item.Update(gameTime);
            }
        }

        public Weapon GetItem(int id)
        {
            foreach (Weapon item in weapons)
            {
                if (item.ID == id)
                {
                    return item;
                }
            }

            throw new ArgumentException($"Bu {id} numarali id'ye sahip silah yok!", nameof(id));
        }

        private float animationTime = 0;
        private float currentAngle = 0f;
        private bool isAnimationForward = true;
    }
}