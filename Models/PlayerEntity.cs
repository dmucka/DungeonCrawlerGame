using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonCrawlerGame.Enums;

namespace DungeonCrawlerGame.Models
{
    public class PlayerEntity : BaseEntity
    {
        public PlayerEntity(int id, int x, int y) : base(id, x, y)
        {
            Type = EntityType.Player;
        }
    }
}
