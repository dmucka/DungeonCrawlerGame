using DungeonCrawlerGame.Enums;
using DungeonCrawlerGame.Interfaces;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonCrawlerGame.Models
{
    public class Tile : PropertyChangedBase, IRenderable
    {
        protected Tile()
        {
            Width = 100;
            Height = 100;
        }

        public Tile(TileType type, int x, int y) : this()
        {
            Type = type;
            X = x;
            Y = y;
            Id = (x + 1) * 10 + (y + 1);
        }

        public int Id { get; private set; }
        public TileType Type { get; private set; }
        public int X { get; private set; }
        public int Y { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
    }
}
