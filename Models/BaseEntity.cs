using DungeonCrawlerGame.Interfaces;
using DungeonCrawlerGame.Enums;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonCrawlerGame.Models
{
    public abstract class BaseEntity : PropertyChangedBase, IRenderable
    {
        protected BaseEntity()
        {
            Width = 80;
            Height = 80;
            Type = EntityType.None;
            Health = 100;
            State = EntityState.Alive;
        }

        public BaseEntity(int id, int x, int y) : this()
        {
            Id = id;
            X = x;
            Y = y;
        }

        public int X { get; protected set; }
        public int Y { get; protected set; }
        public int Width { get; protected set; }
        public int Height { get; protected set; }

        public int Id { get; protected set; }
        public EntityState State { get; protected set; }
        public EntityType Type { get; protected set; }
        public int Health { get; protected set; }
        public abstract int Attack { get; }

        public void Teleport(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Move(SideType side, int units)
        {
            switch (side)
            {
                case SideType.Left:
                    Y -= units;
                    break;
                case SideType.Right:
                    Y += units;
                    break;
                case SideType.Up:
                    X -= units;
                    break;
                case SideType.Down:
                    X += units;
                    break;
            }
        }

        public void AttackTarget(BaseEntity target)
        {
            target.OnReceiveDamage(Attack);

            if (target.State == EntityState.Dead && this is PlayerEntity player && target is EnemyEntity enemy)
            {
                player.AddExperience(enemy.DropExperience);

                var random = new Random();
                if (random.Next(0, 6) == 0)
                {
                    var randomWeapon = (WeaponType)random.Next(1, 4);
                    player.EquipWeapon(randomWeapon);
                }
            }
        }

        public void OnReceiveDamage(int damage)
        {
            var newHealth = Health - damage;
            if (newHealth <= 0)
                State = EntityState.Dead;

            Health = newHealth;
        }
    }
}
