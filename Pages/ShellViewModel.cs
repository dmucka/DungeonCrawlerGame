using DungeonCrawlerGame.Enums;
using DungeonCrawlerGame.Services;
using Stylet;
using StyletIoC;
using System.Windows;
using System.Windows.Input;

namespace DungeonCrawlerGame.Pages
{
    public class ShellViewModel : Conductor<IScreen>.StackNavigation
    {
        private readonly MainMenuViewModel _mainMenuViewModel;
        private readonly PauseViewModel _pauseViewModel;
        private readonly GameOverViewModel _gameOverViewModel;

        public ShellViewModel(MainMenuViewModel mainMenuViewModel, PauseViewModel pauseViewModel, GameOverViewModel gameOverViewModel)
        {
            _mainMenuViewModel = mainMenuViewModel;
            _pauseViewModel = pauseViewModel;
            _gameOverViewModel = gameOverViewModel;

            ActivateItem(_mainMenuViewModel);
        }

        [Inject]
        public SettingsService Settings { get; set; }

        public void ReturnToMainMenu()
        {
            while (ActiveItem is not MainMenuViewModel)
                GoBack();
        }

        public void OpenGameOver(string message)
        {
            _gameOverViewModel.Message = message;
            ActivateItem(_gameOverViewModel);
        }

        public void OpenPauseView() => ActivateItem(_pauseViewModel);

        public void OnEscapePressed()
        {
            if (ActiveItem is GameViewModel)
            {
                OpenPauseView();
            }
            else if (ActiveItem is PauseViewModel)
            {
                GoBack();
            }
        }

        public void OnF5Pressed()
        {
            if (ActiveItem is MainMenuViewModel mainMenu)
            {
                mainMenu.OpenDebugGame();
            }
        }

        public void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (ActiveItem is not GameViewModel gameViewModel)
                return;

            var level = gameViewModel.CurrentLevel;

            if (e.Key == Settings.UpKey)
            {
                level.MovePlayer(SideType.Up, 1);
                level.Tick();
            }
            else if (e.Key == Settings.DownKey)
            {
                level.MovePlayer(SideType.Down, 1);
                level.Tick();
            }
            else if (e.Key == Settings.LeftKey)
            {
                level.MovePlayer(SideType.Left, 1);
                level.Tick();
            }
            else if (e.Key == Settings.RightKey)
            {
                level.MovePlayer(SideType.Right, 1);
                level.Tick();
            }
            else if (e.Key == Settings.AttackKey)
            {
                level.TryAttack(gameViewModel.CurrentLevel.Player);
                level.Tick();
            }

            // game over
            if (gameViewModel.CurrentLevel.Player.State == EntityState.Dead)
            {
                OpenGameOver("You died");
            }
        }

        protected override void OnInitialActivate()
        {
            base.OnInitialActivate();

            // unfocus any button on mouse click anywhere on window
            InputManager.Current.PostProcessInput += (sender, e) =>
            {
                if (e.StagingItem.Input is MouseButtonEventArgs args && args.LeftButton == MouseButtonState.Released)
                    FocusManager.SetFocusedElement((View as Window), null);
            };

            if (Settings.IsFullscreen)
                Settings.EnableFullscreen();
        }
    }
}
