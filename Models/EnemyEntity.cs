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
        public EnemyEntity(int x, int y, int id, EntityType enemyType) : base(x, y, id)
        {
            Type = enemyType;
        }
    }
}
