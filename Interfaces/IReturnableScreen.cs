using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stylet;

namespace DungeonCrawlerGame.Interfaces
{
    public interface IReturnableScreen : IScreen
    {
        public void ReturnView();
    }
}
