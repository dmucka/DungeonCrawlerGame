using DungeonCrawlerGame.Models;
using DungeonCrawlerGame.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DungeonCrawlerGame.Classes;

namespace DungeonCrawlerGame.Services
{
    public class LevelService
    {
        public LevelService()
        {
            Levels = new();
            Levels.Add(GetLevel1);
            Levels.Add(GetLevel2);
            Levels.Add(GetLevel3);
            Levels.Add(GetBossLevel);
        }

        #region State Persistence

        private const string _saveFolder = "Saves";
        private readonly LocalStorage _saveProvider = new(_saveFolder);

        public Level LoadLevel(int slot)
        {
            var saveFile = $"slot{slot}.json";
            var levelId = _saveProvider.GetSavedPropertyValue<int>(saveFile, "Id");
            var newLevel = Levels[levelId - 1](null);
            _saveProvider.Load(saveFile, newLevel);
            newLevel.Render();

            return newLevel;
        }

        public void SaveLevel(Level level, int slot)
        {
            level.SaveTimestamp = DateTime.Now;
            _saveProvider.Save($"slot{slot}.json", level);
        }

        public bool SaveFileExists(int slot)
        {
            var saveFile = $"slot{slot}.json";
            return _saveProvider.StorageExists(saveFile);
        }

        public DateTime GetSaveTimestamp(int slot) => _saveProvider.GetSavedPropertyValue<DateTime>($"slot{slot}.json", "SaveTimestamp");

        #endregion

        public Level CurrentLevel { get; set; }

        public List<Func<PlayerEntity, Level>> Levels { get; }

        public Level GetLevel1(PlayerEntity player = null)
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
                .Render();

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
                .Render();

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
                .Render();

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
                .Render();

            return level;
        }

        public Level GetDebugLevel()
        {
            var level = new Level(1000)
                .SetEmptyLevel()
                .SetWallRing(0)
                .SetSpawnPoint(3, 5)
                .SetWall(0..5, 4)
                .SetDoor(4, 4)
                .SetStairs(1, 6)
                .AddPlayer()
                .AddEnemy(1..3, 1..3, EntityType.Slime)
                .Render();

            return level;
        }

        public Level GetDebugRingLevel()
        {
            var level = new Level(1001)
                .SetEmptyLevel()
                .SetWallRing(1)
                .SetSpawnPoint(3, 5)
                .AddPlayer()
                .Render();

            return level;
        }
    }
}
