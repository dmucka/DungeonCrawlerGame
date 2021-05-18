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
using Newtonsoft.Json;
using System.Windows;

namespace DungeonCrawlerGame.Models
{
    public class LevelExitEventArgs : EventArgs
    {
        public LevelExitEventArgs(Level currentLevel, Level nextLevel)
        {
            CurrentLevel = currentLevel;
            NextLevel = nextLevel;
        }

        public Level CurrentLevel { get; set; }
        public Level NextLevel { get; set; }
    }

    [JsonObject]
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

        public delegate void LevelExitEventHandler(object sender, LevelExitEventArgs e);
        public event LevelExitEventHandler LevelExit;
        public event EventHandler GameOver;

        [Save] public DateTime SaveTimestamp { get; set; }
        [Save] public int Id { get; private set; }
        public int Height { get; private set; }
        public int Width { get; private set; }
        public Point SpawnPoint { get; private set; }

        public ObservableCollection<IRenderableElement> RenderQueue { get; private set; }
        public Tile[,] Map { get; }

        [Save]
        [JsonProperty(ItemTypeNameHandling = TypeNameHandling.Objects)]
        public List<BaseEntity> Entities { get; private set; }

        [Save]
        [JsonProperty(IsReference = true)]
        public PlayerEntity Player { get; private set; }

        public Func<PlayerEntity, Level> NextLevelFactory { get; private set; }

        public Level Render()
        {
            var temp = new ObservableCollection<IRenderableElement>();

            // render map tiles
            foreach (var tile in this)
            {
                double renderX = tile.X * tile.Height;
                double renderY = tile.Y * tile.Width;

                var tileSide = TileSideType.None;

                // clamp
                var prevX = Math.Max(0, tile.X - 1);
                var prevY = Math.Max(0, tile.Y - 1);
                var nextX = Math.Min(Height - 1, tile.X + 1);
                var nextY = Math.Min(Width - 1, tile.Y + 1);

                // isometric view and wall/doors connecting
                if (tile.Type == TileType.Wall)
                {
                    if (tile.X < Height / 2 && tile.Y < Width / 2)
                    {
                        if ((Map[tile.X, nextY].Type == TileType.Wall || Map[tile.X, nextY].Type == TileType.Door) && (Map[nextX, tile.Y].Type == TileType.Wall || Map[nextX, tile.Y].Type == TileType.Door))
                            tileSide = TileSideType.OuterCornerUpLeft;
                        else if (tile.X != 0 && tile.Y != 0 && tile.X != Height - 1 && tile.Y != Width - 1 && (Map[prevX, tile.Y].Type == TileType.Wall || Map[prevX, tile.Y].Type == TileType.Door) && (Map[tile.X, prevY].Type == TileType.Wall || Map[tile.X, prevY].Type == TileType.Door))
                            tileSide = TileSideType.InnerCornerUpLeft;
                    }
                    else if (tile.X < Height / 2 && tile.Y >= Width / 2)
                    {
                        if ((Map[tile.X, prevY].Type == TileType.Wall || Map[tile.X, prevY].Type == TileType.Door) && (Map[nextX, tile.Y].Type == TileType.Wall || Map[nextX, tile.Y].Type == TileType.Door))
                            tileSide = TileSideType.OuterCornerUpRight;
                        else if (tile.X != 0 && tile.Y != 0 && tile.X != Height - 1 && tile.Y != Width - 1 && (Map[prevX, tile.Y].Type == TileType.Wall || Map[prevX, tile.Y].Type == TileType.Door) && (Map[tile.X, nextY].Type == TileType.Wall || Map[tile.X, nextY].Type == TileType.Door))
                            tileSide = TileSideType.InnerCornerUpRight;
                    }
                    else if (tile.X >= Height / 2 && tile.Y < Width / 2)
                    {
                        if ((Map[prevX, tile.Y].Type == TileType.Wall || Map[prevX, tile.Y].Type == TileType.Door) && (Map[tile.X, nextY].Type == TileType.Wall || Map[tile.X, nextY].Type == TileType.Door))
                            tileSide = TileSideType.OuterCornerDownLeft;
                        else if (tile.X != 0 && tile.Y != 0 && tile.X != Height - 1 && tile.Y != Width - 1 && (Map[tile.X, prevY].Type == TileType.Wall || Map[tile.X, prevY].Type == TileType.Door) && (Map[nextX, tile.Y].Type == TileType.Wall || Map[nextX, tile.Y].Type == TileType.Door))
                            tileSide = TileSideType.InnerCornerDownLeft;
                    }
                    else if (tile.X >= Height / 2 && tile.Y >= Width / 2)
                    {
                        if ((Map[tile.X, prevY].Type == TileType.Wall || Map[tile.X, prevY].Type == TileType.Door) && (Map[prevX, tile.Y].Type == TileType.Wall || Map[prevX, tile.Y].Type == TileType.Door))
                            tileSide = TileSideType.OuterCornerDownRight;
                        else if (tile.X != 0 && tile.Y != 0 && tile.X != Height - 1 && tile.Y != Width - 1 && (Map[nextX, tile.Y].Type == TileType.Wall || Map[nextX, tile.Y].Type == TileType.Door) && (Map[tile.X, nextY].Type == TileType.Wall || Map[tile.X, nextY].Type == TileType.Door))
                            tileSide = TileSideType.InnerCornerDownRight;
                    }

                    if (tileSide == TileSideType.None)
                    {
                        if (tile.X < Height / 2 && (Map[nextX, tile.Y].Type != TileType.Wall || Map[prevX, tile.Y].Type != TileType.Wall) && (Map[nextX, tile.Y].Type != TileType.Door && Map[prevX, tile.Y].Type != TileType.Door))
                            tileSide = TileSideType.Up;
                        else if (tile.X >= Height / 2 && (Map[nextX, tile.Y].Type != TileType.Wall || Map[prevX, tile.Y].Type != TileType.Wall) && (Map[nextX, tile.Y].Type != TileType.Door && Map[prevX, tile.Y].Type != TileType.Door))
                            tileSide = TileSideType.Down;
                        else if (tile.Y < Width / 2 && (Map[tile.X, nextY].Type != TileType.Wall || Map[tile.X, prevY].Type != TileType.Wall))
                            tileSide = TileSideType.Left;
                        else if (tile.Y >= Width / 2 && (Map[tile.X, nextY].Type != TileType.Wall || Map[tile.X, prevY].Type != TileType.Wall))
                            tileSide = TileSideType.Right;
                    }
                }
                else if (tile.Type == TileType.Door)
                {
                    if (tile.X < Height / 2 && (Map[nextX, tile.Y].Type != TileType.Wall || Map[prevX, tile.Y].Type != TileType.Wall) && Map[nextX, tile.Y].Type != TileType.Door)
                        tileSide = TileSideType.Up;
                    else if (tile.X >= Height / 2 && (Map[nextX, tile.Y].Type != TileType.Wall || Map[prevX, tile.Y].Type != TileType.Wall) && Map[prevX, tile.Y].Type != TileType.Door)
                        tileSide = TileSideType.Down;
                    else if (tile.Y < Width / 2 && (Map[tile.X, nextY].Type != TileType.Wall || Map[tile.X, prevY].Type != TileType.Wall) && Map[tile.X, prevY].Type != TileType.Door)
                        tileSide = TileSideType.Left;
                    else if (tile.Y >= Width / 2 && (Map[tile.X, nextY].Type != TileType.Wall || Map[tile.X, prevY].Type != TileType.Wall) && Map[tile.X, nextY].Type != TileType.Door)
                        tileSide = TileSideType.Right;
                }


                temp.Add(new TileImage(tile.Id, renderX, renderY, tile.Height, tile.Width, tile.Type, tileSide));
            }

            // render player and enemies
            foreach (var entity in Entities)
            {
                double renderX = entity.X * 100 - 15;
                double renderY = entity.Y * 100 + 7;
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

        public void Tick()
        {
            var random = new Random();

            foreach (EnemyEntity entity in Entities.Where(x => x.Type != EntityType.Player).ToList())
            {
                if (entity.State != EntityState.Alive)
                    continue;

                // random entity movement
                var randomMove = false;

                do
                {
                    var movement = random.Next(-1, 5);
                    if (movement == -1)
                    {
                        randomMove = true;
                        break;
                    }

                    var randomSide = (SideType)movement;
                    randomMove = MoveEntity(entity, randomSide, 1);
                } while (!randomMove);

                // enemy attacks
                var attack = (int)(entity.AttackChance * 100) > (int)(random.NextDouble() * 100);
                if (attack)
                {
                    TryAttack(entity);
                }
            }
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
            double renderX = entity.X * 100 - 15;
            double renderY = entity.Y * 100 + 7;
            var renderedEntity = (EntityView)RenderQueue.First(x => x.Id == entity.Id);
            renderedEntity.Update(renderX, renderY, entity.Health);
        }

        public bool MovePlayer(SideType side, int units)
        {
            var movement = MoveEntity(Player, side, units);

            if (Map[Player.X, Player.Y].Type == TileType.Stairs)
            {
                var nextLevel = NextLevelFactory.Invoke(Player);
                LevelExit(this, new LevelExitEventArgs(this, nextLevel));
            }

            return movement;
        }

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
            {
                RemoveEntity(target);

                if (target.Type == EntityType.BossSlime)
                    GameOver(this, new EventArgs());
            }
            else
                UpdateEntity(target);

            return true;
        }

        public BaseEntity PredictAttack(BaseEntity attacker)
        {
            var range = 1;

            if (attacker is PlayerEntity player)
            {
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
                        range = 3;
                        break;
                }
            }

            var upperLeftX = Math.Clamp(attacker.X - range, 0, Height - 1);
            var upperLeftY = Math.Clamp(attacker.Y - range, 0, Width - 1);
            var lowerRightX = Math.Clamp(attacker.X + range, 0, Height - 1);
            var lowerRightY = Math.Clamp(attacker.Y + range, 0, Width - 1);

            // BFS algorithm to check if the target is visible

            var visitedMatrix = new bool[Height, Width];
            var queue = new Queue<(Point Cell, Point Move)>();

            visitedMatrix[attacker.X, attacker.Y] = true;

            foreach (var (X, Y) in new[] { (-1, 0), (-1, 1), (0, 1), (1, 1), (1, 0), (1, -1), (0, -1), (-1, -1) })
            {
                queue.Enqueue((new Point(attacker.X, attacker.Y), new Point(X, Y)));
            }

            while (queue.Any())
            {
                var (Cell, Move) = queue.Dequeue();

                // the target is visible
                var candidate = Entities.FirstOrDefault(x => x.X == Cell.X && x.Y == Cell.Y);
                if (candidate != null && candidate != attacker && (attacker is EnemyEntity && candidate is not EnemyEntity || attacker is PlayerEntity))
                    return candidate;

                var nextX = Move.X + Cell.X;
                var nextY = Move.Y + Cell.Y;

                if (nextX >= upperLeftX && nextY >= upperLeftY
                    && nextX <= lowerRightX && nextY <= lowerRightY
                    && !visitedMatrix[nextX, nextY]
                    && Map[nextX, nextY].Type != TileType.Wall
                    && Map[nextX, nextY].Type != TileType.Door)
                {
                    queue.Enqueue((new Point(nextX, nextY), Move));
                    visitedMatrix[nextX, nextY] = true;
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

        protected Level SetTileRing(int offset, TileType type)
        {
            foreach (var (X, Y) in new MatrixEnumerator(Height, Width))
            {
                if ((X == offset || X == Height - 1 - offset) && Y < Width - offset && Y >= offset ||
                    (Y == offset || Y == Width - 1 - offset) && X < Height - offset && X >= offset)
                    Map[X, Y] = new Tile(type, X, Y);
            }

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
        public Level SetWallRing(int offset) => SetTileRing(offset, TileType.Wall);

        #endregion

        public Level SetDoor(int x, int y) => SetTile(x, y, TileType.Door);

        public Level SetStairs(int x, int y) => SetTile(x, y, TileType.Stairs);

        public Level SetEmptyLevel()
        {
            foreach (var (X, Y) in new MatrixEnumerator(Height, Width))
                SetFloor(X, Y);

            return this;
        }

        public Level SetNextLevel(Func<PlayerEntity, Level> nextLevelFactory)
        {
            NextLevelFactory = nextLevelFactory;

            return this;
        }

        #region AddEnemy

        public Level AddEnemy(int x, int y, EntityType enemyType)
        {
            Entities.Add(new EnemyEntity(Entities.Count + 1, x, y, enemyType));
            return this;
        }

        public Level AddEnemy(Range xRange, Range yRange, EntityType enemyType)
        {
            foreach (var (X, Y) in new MatrixEnumerator(xRange, yRange))
                AddEnemy(X, Y, enemyType);

            return this;
        }

        public Level AddEnemy(int x, Range yRange, EntityType enemyType)
        {
            foreach (var (X, Y) in new MatrixEnumerator(x..x, yRange))
                AddEnemy(X, Y, enemyType);

            return this;
        }

        public Level AddEnemy(Range xRange, int y, EntityType enemyType)
        {
            foreach (var (X, Y) in new MatrixEnumerator(xRange, y..y))
                AddEnemy(X, Y, enemyType);

            return this;
        }

        #endregion

        public Level AddPlayer()
        {
            Player = new PlayerEntity(1, SpawnPoint.X, SpawnPoint.Y);
            Entities.Add(Player);
            return this;
        }

        public Level AddPlayer(PlayerEntity player)
        {
            if (player == null)
                return this;

            player.Teleport(SpawnPoint.X, SpawnPoint.Y);
            Player = player;
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
