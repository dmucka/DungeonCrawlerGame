using DungeonCrawlerGame.Controls;
using Stylet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using DungeonCrawlerGame.Enums;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using DungeonCrawlerGame.Classes;

namespace DungeonCrawlerGame.Models
{
    public class Level : PropertyChangedBase, IEnumerable<Tile>
    {
        public Level(int id)
        {
            Id = id;
            Height = 6;
            Width = 8;
            RenderQueue = new();
            Map = new Tile[Height, Width];
        }

        public int Id { get; private set; }
        public int Height { get; private set; }
        public int Width { get; private set; }
        public Point SpawnPoint { get; private set; }

        public ObservableCollection<RenderableImage> RenderQueue { get; }
        public Tile[,] Map { get; }

        public Level Render()
        {
            foreach (var tile in this)
            {
                double renderX = tile.X * tile.Height;
                double renderY = tile.Y * tile.Width;
                RenderQueue.Add(new TileImage(renderX, renderY, tile.Width, tile.Height, tile.Type));
            }

            return this;
        }

        public Level SetSpawnPoint(Point spawnPoint)
        {
            SpawnPoint = spawnPoint;
            return this;
        }

        public Level SetFloor(int x, int y)
        {
            Map[x, y] = new Tile(TileType.Floor, x , y);
            return this;
        }

        public Level SetWall(int x, int y)
        {
            Map[x, y] = new Tile(TileType.Wall, x, y);
            return this;
        }

        public Level SetDoor(int x, int y)
        {
            Map[x, y] = new Tile(TileType.Door, x, y);
            return this;
        }

        public Level SetStairs(int x, int y)
        {
            Map[x, y] = new Tile(TileType.Stairs, x, y);
            return this;
        }

        public Level SetEmptyLevel()
        {
            foreach (var (X, Y) in new MatrixEnumerator(Height, Width))
                SetFloor(X, Y);

            return this;
        }

        #region IEnumerable

        public IEnumerator<Tile> GetEnumerator()
        {
            foreach (var (X, Y) in new MatrixEnumerator(Height, Width))
                yield return Map[X, Y];
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        #endregion
    }
}
