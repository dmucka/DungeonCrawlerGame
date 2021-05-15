using DungeonCrawlerGame.Controls;
using Stylet;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DungeonCrawlerGame.Enums;
using System.Collections;
using DungeonCrawlerGame.Classes;
using DungeonCrawlerGame.Interfaces;
using System.Windows.Media;
using Point = System.Drawing.Point;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DungeonCrawlerGame.Models
{
    public class Level : PropertyChangedBase, IEnumerable<Tile>
    {
        public Level(int id)
        {
            Id = id;
            Height = 6;
            Width = 8;
            RenderQueue = null;
            Map = new Tile[Height, Width];
            Entities = new();
            SpawnPoint = new Point(0, 0);
        }

        public int Id { get; private set; }
        public int Height { get; private set; }
        public int Width { get; private set; }
        public Point SpawnPoint { get; private set; }

        public ObservableCollection<IRenderableElement> RenderQueue { get; private set; }
        public Tile[,] Map { get; }
        public List<BaseEntity> Entities { get; }
        public PlayerEntity Player { get; private set; }


        public Level Render()
        {
            var temp = new ObservableCollection<IRenderableElement>();

            // render map tiles
            foreach (var tile in this)
            {
                double renderX = tile.X * tile.Height;
                double renderY = tile.Y * tile.Width;
                temp.Add(new TileImage(tile.Id, renderX, renderY, tile.Height, tile.Width, tile.Type));
            }

            // render player and enemies
            foreach (var entity in Entities)
            {
                double renderX = entity.X * 100 + (entity.Height / 8);
                double renderY = entity.Y * 100 + (entity.Width / 8);
                temp.Add(new EntityImage(entity.Id, renderX, renderY, entity.Height, entity.Width, entity.Type));
            }

            RenderQueue = temp;

            return this;
        }

        public void Update(BaseEntity entity)
        {
            double renderX = entity.X * 100 + (entity.Height / 8);
            double renderY = entity.Y * 100 + (entity.Width / 8);
            var renderedEntity = RenderQueue.First(x => x.Id == entity.Id);
            renderedEntity.Update(renderX, renderY);
        }

        public void CleanUp()
        {
            RenderQueue.Clear();
            RenderQueue = null;
        }

        public Level AddEnemy(int x, int y, EntityType enemyType)
        {
            Entities.Add(new EnemyEntity(x, y, Entities.Count + 1, enemyType));
            return this;
        }

        public Level AddPlayer()
        {
            Player = new PlayerEntity(SpawnPoint.X, SpawnPoint.Y, Entities.Count + 1);
            Entities.Add(Player);
            return this;
        }

        public Level SetSpawnPoint(int x, int y)
        {
            SpawnPoint = new Point(x, y);
            return this;
        }

        public bool MovePlayer(SideType side, int units) => MoveEntity(Player, side, units);

        public bool MoveEntity(BaseEntity entity, SideType side, int units)
        {
            if (!PredictMove(entity.X, entity.Y, side, units))
                return false;

            entity.Move(side, units);
            Update(entity);

            return true;
        }

        public bool PredictMove(int x, int y, SideType side, int units)
        {
            switch (side)
            {
                case SideType.Left:
                    y -= units;
                    break;
                case SideType.Right:
                    y += units;
                    break;
                case SideType.Top:
                    x -= units;
                    break;
                case SideType.Down:
                    x += units;
                    break;
            }

            if (x < 0 || y < 0 || x >= Height || y >= Width)
                return false;

            if (Map[x, y].Type == TileType.Wall)
                return false;

            foreach (var entity in Entities)
            {
                if (entity.X == x && entity.Y == y)
                    return false;
            }

            return true;
        }

        #region SetTile

        protected Level SetTile(int x, int y, TileType type)
        {
            Map[x, y] = new Tile(type, x, y);
            return this;
        }

        protected Level SetTile(Range xRange, Range yRange, TileType type)
        {
            foreach (var (X, Y) in new MatrixEnumerator(xRange, yRange))
                Map[X, Y] = new Tile(type, X, Y);

            return this;
        }

        protected Level SetTile(int x, Range yRange, TileType type)
        {
            foreach (var (X, Y) in new MatrixEnumerator(x..x, yRange))
                Map[X, Y] = new Tile(type, X, Y);

            return this;
        }

        protected Level SetTile(Range xRange, int y, TileType type)
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
                RenderQueue.Add(new RenderableText(RenderQueue.Count + 1, renderX, renderY, 100, 100, $"{X},{Y}", new SolidColorBrush(Color.FromArgb(127, 255, 255, 255))));
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
