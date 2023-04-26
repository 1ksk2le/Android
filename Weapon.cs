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

        private int oldModifier = -1;
        private bool isEquipped = false;

        public void Update(GameTime gameTime)
        {
            string ModifierName = "";
            string EnchantName = "";

            // Check if Modifier value has changed
            if (Modifier != oldModifier)
            {
                oldModifier = Modifier;
                isEquipped = false;
            }

            switch (Modifier)
            {
                case 1:
                    ModifierName = "Reforged";
                    if (!isEquipped)
                    {
                        MinDamage += 10;
                        MaxDamage += 10;
                        isEquipped = true;
                    }
                    break;
                case 2:
                    ModifierName = "Smelt";
                    if (!isEquipped)
                    {
                        MinDamage -= 5;
                        MaxDamage -= 5;
                        isEquipped = true;
                    }
                    break;
                default:
                    ModifierName = "";
                    break;
            }

            switch (Enchant)
            {
                case 1:
                    EnchantName = "Flames";
                    break;
                case 2:
                    EnchantName = "Frost";
                    break;
                default:
                    EnchantName = "";
                    break;
            }

            Name = ModifierName + " " + BaseName + " of " + EnchantName;
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
            weaponLoader.Add(1, (WEP_Test_Sword, 1, 15, 25, 500, 0, 0, 1f, "Magic Sword", "Sword", true, false));

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

        float animationTime = 0;
        float currentAngle = 0f;
        bool isAnimationForward = true;
    }
}

