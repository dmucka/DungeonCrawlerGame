using Stylet;
using System.Windows;
using System.Windows.Input;

namespace DungeonCrawlerGame.Pages
{
    public class ShellViewModel : Conductor<IScreen>.StackNavigation
    {
        public bool IsFullscreen { get; private set; }

        private readonly MainMenuViewModel _mainMenuViewModel;
        private readonly PauseViewModel _pauseViewModel;

        public ShellViewModel(MainMenuViewModel mainMenuViewModel, PauseViewModel pauseViewModel)
        {
            _mainMenuViewModel = mainMenuViewModel;
            _pauseViewModel = pauseViewModel;

            IsFullscreen = false;
            ActivateItem(_mainMenuViewModel);
        }

        public void ReturnToMainMenu()
        {
            while (ActiveItem is not MainMenuViewModel)
                GoBack();
        }

        public void ToggleFullscreen(Window window)
        {
            if (!IsFullscreen)
            {
                window.WindowStyle = WindowStyle.None;
                window.Topmost = true;
                window.WindowState = WindowState.Maximized;
                IsFullscreen = true;
            }
            else
            {
                window.WindowStyle = WindowStyle.SingleBorderWindow;
                window.Topmost = false;
                window.WindowState = WindowState.Normal;
                IsFullscreen = false;
            }
        }

        public void OpenPauseView() => ActivateItem(_pauseViewModel);

        public void OnEscapePressed()
        {
            if (ActiveItem is GameViewModel)
            {
                OpenPauseView();
            }
        }

        protected override void OnInitialActivate()
        {
            base.OnInitialActivate();

            InputManager.Current.PostProcessInput += (sender, e) =>
            {
                if (e.StagingItem.Input is MouseButtonEventArgs args && args.LeftButton == MouseButtonState.Released)
                    FocusManager.SetFocusedElement((View as Window), null);
            };
        }
    }
}
