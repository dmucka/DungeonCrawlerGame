using DungeonCrawlerGame.Interfaces;
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
        public BaseEntity()
        {
            Width = 80;
            Height = 80;
            Type = EntityType.None;
            Health = 100;
        }

        public BaseEntity(double x, double y, int id) : this()
        {
            X = x;
            Y = y;
            Id = id;
        }

        public double X { get; protected set; }
        public double Y { get; protected set; }
        public double Width { get; protected set; }
        public double Height { get; protected set; }

        public int Id { get; protected set; }
        public EntityType Type { get; protected set; }
        public int Health { get; protected set; }
    }
}
