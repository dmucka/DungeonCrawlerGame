using Stylet;

namespace DungeonCrawlerGame.Pages
{
    public class GameViewModel : Screen
    {
        public GameViewModel()
        {
        }

        public void OpenPauseView()
        {
            (this.Parent as ShellViewModel).OpenPauseView();
        }
    }
}
