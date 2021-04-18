using DungeonCrawlerGame.Classes;

namespace DungeonCrawlerGame.Pages
{
    public class PauseViewModel : ReturnableScreen
    {
        readonly private SaveGameViewModel _saveGameViewModel;

        public PauseViewModel(SaveGameViewModel saveGameViewModel)
        {
            _saveGameViewModel = saveGameViewModel;
        }

        public void ReturnToMainMenu()
        {
            (this.Parent as ShellViewModel).ReturnToMainMenu();
        }

        public void OpenSaveGameView()
        {
            (this.Parent as ShellViewModel).ActivateItem(_saveGameViewModel);
        }
    }
}
