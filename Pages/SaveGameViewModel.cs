using DungeonCrawlerGame.Classes;
using DungeonCrawlerGame.Services;
using System.Collections.Generic;

namespace DungeonCrawlerGame.Pages
{
    public class SaveGameViewModel : ReturnableScreen
    {
        private readonly LevelService _levelService;

        public SaveGameViewModel(LevelService levelService)
        {
            _levelService = levelService;

            SaveFileStatus = new();
        }

        public List<string> SaveFileStatus { get; }

        protected override void OnActivate()
        {
            SaveFileStatus.Clear();

            for (int slot = 1; slot <= 3; slot++)
            {
                if (!_levelService.SaveFileExists(slot))
                {
                    SaveFileStatus.Add("Empty");
                    continue;
                }

                SaveFileStatus.Add(_levelService.GetSaveTimestamp(slot).ToString());
            }
        }

        public void SaveGame(string slotString)
        {
            var slot = int.Parse(slotString);

            _levelService.SaveLevel(_levelService.CurrentLevel, slot);
            ReturnView();
        }
    }
}
