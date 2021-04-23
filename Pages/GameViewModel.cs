using Stylet;

namespace DungeonCrawlerGame.Pages
{
    public class GameViewModel : Screen
    {
        public GameViewModel()
        {
        }

        public void OpenPauseView() => (Parent as ShellViewModel).OpenPauseView();
    }
}
