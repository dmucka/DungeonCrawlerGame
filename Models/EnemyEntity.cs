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
                Health = 250;
        }

        public int DropExperience => CalculateDropExperience();
        public override int Attack => CalculateAttack();
        public double AttackChance => CalculateAttackChance();

        private int CalculateDropExperience()
        {
            switch (Type)
            {
                case EntityType.Slime:
                    return 3;
                case EntityType.BossSlime:
                    return 10;
            }

            return 0;
        }

        private int CalculateAttack()
        {
            switch (Type)
            {
                case EntityType.Slime:
                    return 5;
                case EntityType.BossSlime:
                    return 25;
            }

            return 0;
        }

        private double CalculateAttackChance()
        {
            switch (Type)
            {
                case EntityType.Slime:
                    return 0.25;
                case EntityType.BossSlime:
                    return 0.75;
            }

            return 0;
        }
    }
}
