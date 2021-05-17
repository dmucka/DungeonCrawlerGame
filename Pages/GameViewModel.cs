using DungeonCrawlerGame.Controls;
using DungeonCrawlerGame.Models;
using DungeonCrawlerGame.Services;
using DungeonCrawlerGame.Enums;
using Stylet;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DungeonCrawlerGame.Pages
{
    public class GameViewModel : Screen
    {
        private readonly LevelService _levelService;

        public Level CurrentLevel { get; private set; }

        public GameViewModel(LevelService levelService)
        {
            _levelService = levelService;
        }

        public void LoadGame(Level level)
        {
            _levelService.CurrentLevel = level;
            CurrentLevel = _levelService.CurrentLevel;

            CurrentLevel.LevelExit += CurrentLevel_LevelExit;
            CurrentLevel.GameOver += CurrentLevel_GameOver;
        }

        public void NewGame()
        {
            LoadGame(_levelService.GetLevel1());
        }

        public void DebugGame()
        {
            LoadGame(_levelService.GetDebugLevel1().SetDebugText());
        }

        private void CurrentLevel_GameOver(object sender, System.EventArgs e)
        {
            (Parent as ShellViewModel).OpenGameOver("You won");
        }

        private void CurrentLevel_LevelExit(object sender, LevelExitEventArgs e)
        {
            CurrentLevel.LevelExit -= CurrentLevel_GameOver;
            CurrentLevel.LevelExit -= CurrentLevel_LevelExit;

            _levelService.CurrentLevel = e.NextLevel;
            CurrentLevel = e.NextLevel;

            CurrentLevel.LevelExit += CurrentLevel_LevelExit;
            CurrentLevel.GameOver += CurrentLevel_GameOver;
        }

        protected override void OnClose()
        {
            CurrentLevel.CleanUp();
            CurrentLevel = null;
        }

        public void OpenPauseView() => (Parent as ShellViewModel).OpenPauseView();

    }
}
