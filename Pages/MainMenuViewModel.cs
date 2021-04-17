using Stylet;
using System.Windows;

namespace DungeonCrawlerGame.Pages
{
    public class MainMenuViewModel : Screen
    {
        readonly private AboutViewModel _aboutViewModel;
        readonly private LoadGameViewModel _loadGameViewModel;
        readonly private SettingsViewModel _settingsViewModel;
        readonly private GameViewModel _gameViewModel;

        public MainMenuViewModel(AboutViewModel aboutViewModel, LoadGameViewModel loadGameViewModel, SettingsViewModel settingsViewModel, GameViewModel gameViewModel)
        {
            _aboutViewModel = aboutViewModel;
            _loadGameViewModel = loadGameViewModel;
            _settingsViewModel = settingsViewModel;
            _gameViewModel = gameViewModel;
        }

        public void OpenAboutView()
        {
            (this.Parent as ShellViewModel).ActivateItem(_aboutViewModel);
        }

        public void OpenLoadGameView()
        {
            (this.Parent as ShellViewModel).ActivateItem(_loadGameViewModel);
        }

        public void OpenNewGameView()
        {
            (this.Parent as ShellViewModel).ActivateItem(_gameViewModel);
        }

        public void OpenSettingsView()
        {
            (this.Parent as ShellViewModel).ActivateItem(_settingsViewModel);
        }

        public void Exit()
        {
            Application.Current.Shutdown();
        }
    }
}
