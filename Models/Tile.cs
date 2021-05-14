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
        public Tile()
        {
            Width = 100;
            Height = 100;
        }

        public Tile(TileType type) : this()
        {
            Type = type;
        }

        public Tile(TileType type, double x, double y) : this(type)
        {
            X = x;
            Y = y;
        }

        public TileType Type { get; private set; }
        public double X { get; private set; }
        public double Y { get; private set; }
        public double Width { get; private set; }
        public double Height { get; private set; }
    }
}
