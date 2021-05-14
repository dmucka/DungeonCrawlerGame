using DungeonCrawlerGame.Controls;
using DungeonCrawlerGame.Models;
using DungeonCrawlerGame.Services;
using Stylet;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace DungeonCrawlerGame.Pages
{
    public class GameViewModel : Screen
    {
        private readonly LevelService _levelService;

        public GameViewModel(LevelService levelService)
        {
            _levelService = levelService;
        }

        protected override void OnInitialActivate()
        {
            CurrentLevel = _levelService.GetLevel1();
        }

        protected override void OnActivate()
        {
            
        }

        public Level CurrentLevel { get; private set; }

        public void OpenPauseView() => (Parent as ShellViewModel).OpenPauseView();
        
    }
}
