using DungeonCrawlerGame.Classes;
using DungeonCrawlerGame.Services;
using StyletIoC;
using System.Windows.Input;

namespace DungeonCrawlerGame.Pages
{
    public class SettingsViewModel : ReturnableScreen
    {
        public SettingsViewModel()
        {
        }

        [Inject]
        public SettingsService Settings { get; set; }

        public void OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        protected override void OnDeactivate()
        {
            Settings.SaveSettings();
        }
    }
}
