using DungeonCrawlerGame.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonCrawlerGame.Models
{
    public class EnemyEntity : BaseEntity
    {
        public EnemyEntity(int id, int x, int y, EntityType enemyType) : base(id, x, y)
        {
            Type = enemyType;
        }
    }
}
