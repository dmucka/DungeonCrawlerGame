using DungeonCrawlerGame.Classes;
using DungeonCrawlerGame.Services;
using StyletIoC;

namespace DungeonCrawlerGame.Pages
{
    public class SettingsViewModel : ReturnableScreen
    {
        public SettingsViewModel()
        {
        }

        [Inject]
        public SettingsService Settings { get; set; }
    }
}
