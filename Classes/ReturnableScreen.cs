using DungeonCrawlerGame.Interfaces;
using Stylet;

namespace DungeonCrawlerGame.Classes
{
    public class ReturnableScreen : Screen, IReturnableScreen
    {
        public void ReturnView()
        {
            this.RequestClose();
        }
    }
}
