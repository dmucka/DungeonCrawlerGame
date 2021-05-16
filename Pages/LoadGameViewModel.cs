using DungeonCrawlerGame.Classes;
using DungeonCrawlerGame.Services;
using LambdaConverters;
using System.Collections.Generic;
using System.Windows.Data;

namespace DungeonCrawlerGame.Pages
{
    public static class LoadGameConverters
    {
        public static readonly IValueConverter SaveFileEmptyConverter = ValueConverter.Create<string, bool>(e => e.Value != "Empty");
    }

    public class LoadGameViewModel : ReturnableScreen
    {
        private readonly LevelService _levelService;

        public LoadGameViewModel(LevelService levelService)
        {
            _levelService = levelService;

            SaveFileStatus = new();
        }

        public List<string> SaveFileStatus { get; }
        public GameViewModel GameViewModel { get; set; }

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

        public void LoadGame(string slotString)
        {
            var slot = int.Parse(slotString);
            var loadedLevel = _levelService.LoadLevel(slot);
            GameViewModel.LoadGame(loadedLevel);
            (Parent as ShellViewModel).ActivateItem(GameViewModel);
            ReturnView();
        }
    }
}
