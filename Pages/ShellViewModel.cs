using System;
using System.Windows;
using System.Windows.Controls;
using Stylet;

namespace DungeonCrawlerGame.Pages
{
    public class ShellViewModel : Conductor<IScreen>.StackNavigation
    {
        public bool IsFullscreen { get; private set; }

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

        public void OnEscapePressed()
        {
            if (this.ActiveItem is GameViewModel)
            {
                this.ActivateItem(_pauseViewModel);
            }
        }

#if DEBUG
        public override void ActivateItem(IScreen item)
        {
            base.ActivateItem(item);
        }
#endif
    }
}
