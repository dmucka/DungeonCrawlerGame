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
            var level = new Level(1).SetEmptyLevel().Render();
            return level;
        }
    }
}
