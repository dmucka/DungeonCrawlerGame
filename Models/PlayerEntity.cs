using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonCrawlerGame.Classes;
using DungeonCrawlerGame.Enums;
using Newtonsoft.Json;

namespace DungeonCrawlerGame.Models
{
    public class PlayerEntity : BaseEntity
    {
        [JsonConstructor]
        protected PlayerEntity() : base()
        {
        }

        public PlayerEntity(int id, int x, int y) : base(id, x, y)
        {
            Type = EntityType.Player;
            Weapon = WeaponType.Sword;
            Level = 1;
            Experience = 0;
        }

        [Save] public WeaponType Weapon { get; private set; }
        [Save] public int Experience { get; private set; }
        [Save] public int Level { get; private set; }
        public override int Attack { get => CalculateAttack(); }

        public void EquipWeapon(WeaponType weapon)
        {
            Weapon = weapon;
            NotifyOfPropertyChange(nameof(Attack));
        }

        public void AddExperience(int exp)
        {
            var newExp = Experience + exp;
            if (newExp >= 10)
            {
                newExp = 0;
                Level += 1;
                Health = 100;
            }

            Experience = newExp;
        }

        private int CalculateAttack()
        {
            var attack = 0;

            switch (Weapon)
            {
                case WeaponType.None:
                    break;
                case WeaponType.Sword:
                    attack = 20;
                    break;
                case WeaponType.Staff:
                    attack = 15;
                    break;
                case WeaponType.Bow:
                    attack = 10;
                    break;
            }

            attack *= Level;

            return attack;
        }
    }
}
