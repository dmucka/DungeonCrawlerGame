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
        public PlayerEntity(int x, int y, int id) : base(x, y, id)
        {
            Type = EntityType.Player;
        }
    }
}
