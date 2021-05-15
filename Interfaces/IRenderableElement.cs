using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonCrawlerGame.Interfaces
{
    public interface IRenderableElement
    {
        int Id { get; }
        double X { get; }
        double Y { get; }
        double Width { get; }
        double Height { get; }

        void Update(double x, double y);
    }
}
