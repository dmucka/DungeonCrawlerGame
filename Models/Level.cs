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
                double renderX = entity.X * 100 - 10;
                double renderY = entity.Y * 100 + (entity.Width / 8);
                temp.Add(new EntityView(entity.Id, renderX, renderY, entity.Height, entity.Width, entity.Type, entity.Health));
            }

            RenderQueue = temp;

            return this;
        }

        public void CleanUp()
        {
            RenderQueue.Clear();
            RenderQueue = null;
        }

        #region Entity Editor

        public void RemoveEntity(BaseEntity entity)
        {
            var renderedEntity = RenderQueue.First(x => x.Id == entity.Id);
            RenderQueue.Remove(renderedEntity);
            Entities.Remove(entity);
        }

        public void UpdateEntity(BaseEntity entity)
        {
            double renderX = entity.X * 100 - 10;
            double renderY = entity.Y * 100 + (entity.Width / 8);
            var renderedEntity = (EntityView)RenderQueue.First(x => x.Id == entity.Id);
            renderedEntity.Update(renderX, renderY, entity.Health);
        }

        public bool MovePlayer(SideType side, int units) => MoveEntity(Player, side, units);

        public bool MoveEntity(BaseEntity entity, SideType side, int units)
        {
            if (!PredictMove(entity.X, entity.Y, side, units))
                return false;

            entity.Move(side, units);
            UpdateEntity(entity);

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
                case SideType.Up:
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

        public bool TryAttack(BaseEntity attacker)
        {
            var target = PredictAttack(attacker);
            if (target == null)
                return false;

            attacker.AttackTarget(target);

            if (target.State == EntityState.Dead)
                RemoveEntity(target);
            else
                UpdateEntity(target);

            return true;
        }

        public BaseEntity PredictAttack(BaseEntity attacker)
        {
            if (attacker is PlayerEntity player)
            {
                var range = 0;
                switch (player.Weapon)
                {
                    case WeaponType.None:
                        range = 0;
                        break;
                    case WeaponType.Sword:
                        range = 1;
                        break;
                    case WeaponType.Staff:
                        range = 2;
                        break;
                    case WeaponType.Bow:
                        range = 4;
                        break;
                }

                var upperLeftX = Math.Clamp(player.X - range, 0, Height);
                var upperLeftY = Math.Clamp(player.Y - range, 0, Width);
                var lowerRightX = Math.Clamp(player.X + range, 0, Height);
                var lowerRightY = Math.Clamp(player.Y + range, 0, Width);

                foreach (var (X, Y) in new MatrixEnumerator(upperLeftX..lowerRightX, upperLeftY..lowerRightY))
                {
                    var candidate = Entities.FirstOrDefault(x => x.X == X && x.Y == Y);
                    if (candidate != null && candidate != attacker)
                        return candidate;
                }
            }

            return null;
        }

        #endregion

        #region Map Editor

        public Level SetSpawnPoint(int x, int y)
        {
            SpawnPoint = new Point(x, y);
            return this;
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

        public Level AddEnemy(int x, int y, EntityType enemyType)
        {
            Entities.Add(new EnemyEntity(Entities.Count + 1, x, y, enemyType));
            return this;
        }

        public Level AddPlayer()
        {
            Player = new PlayerEntity(Entities.Count + 1, SpawnPoint.X, SpawnPoint.Y);
            Entities.Add(Player);
            return this;
        }

        #endregion

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
