using DungeonCrawlerGame.Classes;

namespace DungeonCrawlerGame.Pages
{
    public class PauseViewModel : ReturnableScreen
    {
        private readonly SaveGameViewModel _saveGameViewModel;

        public PauseViewModel(SaveGameViewModel saveGameViewModel)
        {
            _saveGameViewModel = saveGameViewModel;
        }

        public void ReturnToMainMenu() => (Parent as ShellViewModel).ReturnToMainMenu();

        public void OpenSaveGameView() => (Parent as ShellViewModel).ActivateItem(_saveGameViewModel);
    }
}
