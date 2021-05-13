using DungeonCrawlerGame.Classes;
using Stylet;
using System.Windows;
using System.Windows.Input;

namespace DungeonCrawlerGame.Services
{
    public class SettingsService : PropertyChangedBase
    {
        private const string _settingsFolder = "Settings";
        private const string _defaultFile = "default.json";
        private const string _settingsFile = "user.json";
        private readonly LocalStorage _settingsProvider = new(_settingsFolder);

        public SettingsService()
        {
            IsFullscreen = false;
            UpKey = Key.W;
            DownKey = Key.S;
            LeftKey = Key.A;
            RightKey = Key.D;
            AttackKey = Key.Space;

            _settingsProvider.Save(_defaultFile, this);
        }

        [Save]
        public bool IsFullscreen { get; set; }

        [Save]
        public Key UpKey { get; set; }

        [Save]
        public Key DownKey { get; set; }

        [Save]
        public Key LeftKey { get; set; }

        [Save]
        public Key RightKey { get; set; }

        [Save]
        public Key AttackKey { get; set; }

        /// <summary>
        /// Toggles fullscreen mode.
        /// </summary>
        public void ToggleFullscreen()
        {
            if (IsFullscreen)
                DisableFullscreen();
            else
                EnableFullscreen();
        }

        /// <summary>
        /// Enable fullscreen mode.
        /// </summary>
        public void EnableFullscreen()
        {
            var window = Application.Current.MainWindow;

            window.WindowStyle = WindowStyle.None;
            window.Topmost = true;
            window.WindowState = WindowState.Maximized;
            IsFullscreen = true;
        }

        /// <summary>
        /// Disable fullscreen mode.
        /// </summary>
        public void DisableFullscreen()
        {
            var window = Application.Current.MainWindow;

            window.WindowStyle = WindowStyle.SingleBorderWindow;
            window.Topmost = false;
            window.WindowState = WindowState.Normal;
            IsFullscreen = false;
        }

        /// <summary>
        /// Save settings to file.
        /// </summary>
        public void SaveSettings()
        {
            _settingsProvider.Save(_settingsFile, this);
        }

        /// <summary>
        /// Load settings from file.
        /// </summary>
        public void LoadSettings()
        {
            if (_settingsProvider.StorageExists(_settingsFile))
                _settingsProvider.Load(_settingsFile, this);
        }
    }
}
