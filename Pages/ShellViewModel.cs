using System.Windows;
using System.Windows.Input;
using Stylet;

namespace DungeonCrawlerGame.Pages
{
    public class ShellViewModel : Conductor<IScreen>.StackNavigation
    {
        public bool IsFullscreen
        {
            get; private set;
        }

        readonly private MainMenuViewModel _mainMenuViewModel;
        readonly private PauseViewModel _pauseViewModel;

        public ShellViewModel(MainMenuViewModel mainMenuViewModel, PauseViewModel pauseViewModel)
        {
            _mainMenuViewModel = mainMenuViewModel;
            _pauseViewModel = pauseViewModel;

            this.IsFullscreen = false;
            this.ActivateItem(_mainMenuViewModel);
        }

        public void ReturnToMainMenu()
        {
            while (this.ActiveItem is not MainMenuViewModel)
                this.GoBack();
        }

        public void ToggleFullscreen(Window window)
        {
            if (!this.IsFullscreen)
            {
                window.WindowStyle = WindowStyle.None;
                window.Topmost = true;
                window.WindowState = WindowState.Maximized;
                this.IsFullscreen = true;
            }
            else
            {
                window.WindowStyle = WindowStyle.SingleBorderWindow;
                window.Topmost = false;
                window.WindowState = WindowState.Normal;
                this.IsFullscreen = false;
            }
        }

        public void OpenPauseView()
        {
            this.ActivateItem(_pauseViewModel);
        }

        public void OnEscapePressed()
        {
            if (this.ActiveItem is GameViewModel)
            {
                this.OpenPauseView();
            }
        }

        protected override void OnInitialActivate()
        {
            base.OnInitialActivate();

            InputManager.Current.PostProcessInput += (sender, e) =>
            {
                if (e.StagingItem.Input is MouseButtonEventArgs args && args.LeftButton == MouseButtonState.Released)
                    FocusManager.SetFocusedElement((this.View as Window), null);
            };
        }
    }
}
