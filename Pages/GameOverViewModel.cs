using Stylet;

namespace DungeonCrawlerGame.Pages
{
    public class GameOverViewModel : Screen
    {
        public GameOverViewModel()
        {
        }

        public void ReturnToMainMenu() => (Parent as ShellViewModel).ReturnToMainMenu();

        public string Message { get; set; }
    }
}
