using DungeonCrawlerGame.Models;
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
                .SetWall(0..3, 4..4)
                .SetDoor(4, 4)
                .SetWall(5, 4)
                .SetStairs(1, 6)
                .Render()
                .SetDebugText();
            return level;
        }
    }
}
