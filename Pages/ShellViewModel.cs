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

        public ShellViewModel(MainMenuViewModel mainMenuViewModel, PauseViewModel pauseViewModel)
        {
            _mainMenuViewModel = mainMenuViewModel;
            _pauseViewModel = pauseViewModel;

            ActivateItem(_mainMenuViewModel);
        }

        [Inject]
        public SettingsService Settings { get; set; }

        public void ReturnToMainMenu()
        {
            while (ActiveItem is not MainMenuViewModel)
                GoBack();
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

        public void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (ActiveItem is not GameViewModel gameViewModel)
                return;

            if (e.Key == Settings.UpKey)
            {
                gameViewModel.CurrentLevel.MovePlayer(SideType.Up, 1);
            }
            else if (e.Key == Settings.DownKey)
            {
                gameViewModel.CurrentLevel.MovePlayer(SideType.Down, 1);
            }
            else if (e.Key == Settings.LeftKey)
            {
                gameViewModel.CurrentLevel.MovePlayer(SideType.Left, 1);
            }
            else if (e.Key == Settings.RightKey)
            {
                gameViewModel.CurrentLevel.MovePlayer(SideType.Right, 1);
            }
            else if (e.Key == Settings.AttackKey)
            {
                gameViewModel.CurrentLevel.TryAttack(gameViewModel.CurrentLevel.Player);
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
