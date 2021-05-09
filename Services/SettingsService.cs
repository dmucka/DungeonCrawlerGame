using Stylet;
using System.Windows;
using System.Windows.Input;

namespace DungeonCrawlerGame.Services
{
    public class SettingsService : PropertyChangedBase
    {
        public SettingsService()
        {
            IsFullscreen = false;
            UpKey = Key.W;
            DownKey = Key.S;
            LeftKey = Key.A;
            RightKey = Key.D;
            AttackKey = Key.Space;
        }

        public bool IsFullscreen { get; set; }
        public Key UpKey { get; set; }
        public Key DownKey { get; set; }
        public Key LeftKey { get; set; }
        public Key RightKey { get; set; }
        public Key AttackKey { get; set; }

        /// <summary>
        /// Toggles fullscreen mode.
        /// </summary>
        public void ToggleFullscreen()
        {
            var window = Application.Current.MainWindow;

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

        public void SaveSettings()
        {

        }

        public void LoadSettings()
        {

        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
        }
    }
}
