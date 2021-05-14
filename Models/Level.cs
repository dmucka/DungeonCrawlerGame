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
using DungeonCrawlerGame.Interfaces;

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

        public ObservableCollection<IRenderable> RenderQueue { get; }
        public Tile[,] Map { get; }

        public Level Render()
        {
            foreach (var tile in this)
            {
                double renderX = tile.X * tile.Height;
                double renderY = tile.Y * tile.Width;
                RenderQueue.Add(new TileImage(renderX, renderY, tile.Height, tile.Width, tile.Type));
            }

            return this;
        }

        public Level SetSpawnPoint(int x, int y)
        {
            SpawnPoint = new Point(x, y);
            return this;
        }

        #region SetTile

        private Level SetTile(int x, int y, TileType type)
        {
            Map[x, y] = new Tile(type, x, y);
            return this;
        }

        private Level SetTile(Range xRange, Range yRange, TileType type)
        {
            foreach (var (X, Y) in new MatrixEnumerator(xRange, yRange))
                Map[X, Y] = new Tile(type, X, Y);

            return this;
        }

        private Level SetTile(int x, Range yRange, TileType type)
        {
            foreach (var (X, Y) in new MatrixEnumerator(x..x, yRange))
                Map[X, Y] = new Tile(type, X, Y);

            return this;
        }

        private Level SetTile(Range xRange, int y, TileType type)
        {
            foreach (var (X, Y) in new MatrixEnumerator(xRange, y..y))
                Map[X, Y] = new Tile(type, X, Y);

            return this;
        }

        #endregion

        #region SetFloor

        public Level SetFloor(int x, int y) => SetTile(x, y, TileType.Floor);
        public Level SetFloor(Range xRange, Range yRange) => SetTile(xRange, yRange, TileType.Floor);
        public Level SetFloor(int x, Range yRange) => SetTile(x, yRange, TileType.Floor);
        public Level SetFloor(Range xRange, int y) => SetTile(xRange, y, TileType.Floor);

        #endregion

        #region SetWall

        public Level SetWall(int x, int y) => SetTile(x, y, TileType.Wall);
        public Level SetWall(Range xRange, Range yRange) => SetTile(xRange, yRange, TileType.Wall);
        public Level SetWall(int x, Range yRange) => SetTile(x, yRange, TileType.Wall);
        public Level SetWall(Range xRange, int y) => SetTile(xRange, y, TileType.Wall);

        #endregion

        public Level SetDoor(int x, int y) => SetTile(x, y, TileType.Door);

        public Level SetStairs(int x, int y) => SetTile(x, y, TileType.Stairs);

        public Level SetEmptyLevel()
        {
            foreach (var (X, Y) in new MatrixEnumerator(Height, Width))
                SetFloor(X, Y);

            return this;
        }

        public Level SetDebugText()
        {
            foreach (var (X, Y) in new MatrixEnumerator(Height, Width))
            {
                double renderX = X * 100 + 25;
                double renderY = Y * 100 + 15;
                RenderQueue.Add(new RenderableText(renderX, renderY, 100, 100, $"{X},{Y}"));
            }

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
