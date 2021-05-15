﻿using DungeonCrawlerGame.Interfaces;
using DungeonCrawlerGame.Enums;
using Stylet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonCrawlerGame.Models
{
    public abstract class BaseEntity : PropertyChangedBase, IRenderable
    {
        protected BaseEntity()
        {
            Width = 80;
            Height = 80;
            Type = EntityType.None;
            Health = 100;
        }

        public BaseEntity(int x, int y, int id) : this()
        {
            Id = id;
            X = x;
            Y = y;
        }

        public int X { get; protected set; }
        public int Y { get; protected set; }
        public int Width { get; protected set; }
        public int Height { get; protected set; }

        public int Id { get; protected set; }
        public EntityType Type { get; protected set; }
        public int Health { get; protected set; }

        public void Move(SideType side, int units)
        {
            switch (side)
            {
                case SideType.Left:
                    Y -= units;
                    break;
                case SideType.Right:
                    Y += units;
                    break;
                case SideType.Top:
                    X -= units;
                    break;
                case SideType.Down:
                    X += units;
                    break;
            }
        }
    }
}