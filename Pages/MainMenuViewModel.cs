using Stylet;
using System.Windows;

namespace DungeonCrawlerGame.Pages
{
    public class MainMenuViewModel : Screen
    {
        private readonly AboutViewModel _aboutViewModel;
        private readonly LoadGameViewModel _loadGameViewModel;
        private readonly SettingsViewModel _settingsViewModel;
        private readonly GameViewModel _gameViewModel;

        public MainMenuViewModel(AboutViewModel aboutViewModel, LoadGameViewModel loadGameViewModel, SettingsViewModel settingsViewModel, GameViewModel gameViewModel)
        {
            _aboutViewModel = aboutViewModel;
            _loadGameViewModel = loadGameViewModel;
            _settingsViewModel = settingsViewModel;
            _gameViewModel = gameViewModel;
        }

        public void OpenAboutView() => (Parent as ShellViewModel).ActivateItem(_aboutViewModel);

        public void OpenLoadGameView()
        {
            _loadGameViewModel.GameViewModel = _gameViewModel;
            (Parent as ShellViewModel).ActivateItem(_loadGameViewModel);
        }

        public void OpenNewGameView()
        {
            _gameViewModel.NewGame();
            (Parent as ShellViewModel).ActivateItem(_gameViewModel);
        }

        public void OpenDebugGame()
        {
            _gameViewModel.DebugGame();
            (Parent as ShellViewModel).ActivateItem(_gameViewModel);
        }

        public void OpenSettingsView() => (Parent as ShellViewModel).ActivateItem(_settingsViewModel);

        public void Exit() => Application.Current.Shutdown();
    }
}
