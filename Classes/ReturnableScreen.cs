using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
