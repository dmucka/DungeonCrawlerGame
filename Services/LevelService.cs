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
                .SetNextLevel(GetLevel2)
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

        public Level GetLevel2(PlayerEntity player)
        {
            var level = new Level(2)
                .SetNextLevel(GetLevel3)
                .SetEmptyLevel()
                .SetSpawnPoint(1, 6)
                .SetWall(0..5, 4)
                .SetDoor(1, 4)
                .SetStairs(2, 2)
                .AddPlayer(player)
                .AddEnemy(3, 5, EntityType.Slime)
                .AddEnemy(4, 6, EntityType.Slime)
                .AddEnemy(2, 1, EntityType.Slime)
                .Render()
                .SetDebugText();

            return level;
        }

        public Level GetLevel3(PlayerEntity player)
        {
            var level = new Level(3)
                .SetNextLevel(GetBossLevel)
                .SetEmptyLevel()
                .SetSpawnPoint(4, 1)
                .SetWall(0..5, 4)
                .SetDoor(1, 4)
                .SetStairs(5, 5)
                .AddPlayer(player)
                .AddEnemy(0, 0, EntityType.Slime)
                .AddEnemy(3, 5, EntityType.Slime)
                .AddEnemy(4, 6, EntityType.Slime)
                .AddEnemy(2, 1, EntityType.Slime)
                .Render()
                .SetDebugText();

            return level;
        }

        public Level GetBossLevel(PlayerEntity player)
        {
            var level = new Level(4)
                .SetEmptyLevel()
                .SetWallRing(0)
                .SetSpawnPoint(1, 6)
                .SetWall(0..2, 4)
                .SetWall(2, 4..6)
                .SetDoor(1, 4)
                .AddPlayer(player)
                .AddEnemy(4, 2, EntityType.BossSlime)
                .Render()
                .SetDebugText();

            return level;
        }

        public Level GetDebugLevel()
        {
            var level = new Level(-1)
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

        public Level GetDebugRingLevel()
        {
            var level = new Level(-1)
                .SetEmptyLevel()
                .SetWallRing(1)
                .SetSpawnPoint(3, 5)
                .AddPlayer()
                .Render()
                .SetDebugText();

            return level;
        }
    }
}
