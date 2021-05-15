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

        public void NewGame()
        {
            CurrentLevel = _levelService.GetLevel1();
            CurrentLevel.LevelExit += CurrentLevel_LevelExit;
            CurrentLevel.GameOver += CurrentLevel_GameOver;
        }

        private void CurrentLevel_GameOver(object sender, System.EventArgs e)
        {
            (Parent as ShellViewModel).OpenGameOver();
        }

        private void CurrentLevel_LevelExit(object sender, LevelExitEventArgs e)
        {
            CurrentLevel.LevelExit -= CurrentLevel_GameOver;
            CurrentLevel.LevelExit -= CurrentLevel_LevelExit;

            CurrentLevel = e.NextLevel;

            CurrentLevel.LevelExit += CurrentLevel_LevelExit;
            CurrentLevel.GameOver += CurrentLevel_GameOver;
        }

        protected override void OnInitialActivate()
        {
            NewGame();
        }

        protected override void OnClose()
        {
            CurrentLevel.CleanUp();
            CurrentLevel = null;
        }

        public void OpenPauseView() => (Parent as ShellViewModel).OpenPauseView();

    }
}
