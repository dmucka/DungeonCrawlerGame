using DungeonCrawlerGame.Models;
using DungeonCrawlerGame.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonCrawlerGame.Services
{
    public class LevelService
    {
        public LevelService()
        {
        }

        public Level GetLevel1()
        {
            var level = new Level(1)
                .SetEmptyLevel()
                .SetSpawnPoint(0, 0)
                .SetWall(0..5, 4)
                .SetDoor(4, 4)
                .SetStairs(1, 6)
                .AddPlayer()
                .AddEnemy(4, 0, EntityType.Slime)
                .Render()
                .SetDebugText();
            return level;
        }
    }
}
