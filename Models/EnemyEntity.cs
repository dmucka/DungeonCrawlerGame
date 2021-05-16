using DungeonCrawlerGame.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonCrawlerGame.Models
{
    public class EnemyEntity : BaseEntity
    {
        [JsonConstructor]
        protected EnemyEntity() : base()
        {
        }

        public EnemyEntity(int id, int x, int y, EntityType enemyType) : base(id, x, y)
        {
            Type = enemyType;

            if (enemyType == EntityType.BossSlime)
                Health = 200;
        }

        public int DropExperience { get => CalculateDropExperience(); }
        public override int Attack { get => CalculateAttack(); }

        private int CalculateDropExperience()
        {
            switch (Type)
            {
                case EntityType.Slime:
                    return 3;
                default:
                    return 0;
            }
        }

        private int CalculateAttack()
        {
            switch (Type)
            {
                case EntityType.Slime:
                    return 5;
                case EntityType.BossSlime:
                    return 15;
                default:
                    return 0;
            }
        }
    }
}
