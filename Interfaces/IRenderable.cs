using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DungeonCrawlerGame.Interfaces
{
    public interface IRenderable
    {
        double X { get; }
        double Y { get; }
        double Width { get; }
        double Height { get; }
    }
}
