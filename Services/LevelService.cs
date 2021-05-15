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
                .AddEnemy(4, 6, EntityType.Slime)
                .Render()
                .SetDebugText();

            return level;
        }

        public Level GetLevel2()
        {
            var level = new Level(2)
                .SetEmptyLevel()
                .SetSpawnPoint(1, 6)
                .SetWall(0..5, 4)
                .SetDoor(1, 4)
                .SetStairs(2, 2)
                .AddPlayer()
                .AddEnemy(3, 5, EntityType.Slime)
                .AddEnemy(4, 6, EntityType.Slime)
                .AddEnemy(2, 1, EntityType.Slime)
                .Render()
                .SetDebugText();

            return level;
        }

        public Level GetDebugLevel()
        {
            var level = new Level(1337)
                .SetEmptyLevel()
                .SetWallRing(0)
                .SetSpawnPoint(3, 5)
                .SetWall(0..5, 4)
                .SetDoor(4, 4)
                .SetStairs(1, 6)
                .AddPlayer()
                .AddEnemy(1..3, 1..3, EntityType.Slime)
                .Render()
                .SetDebugText();

            return level;
        }
    }
}
