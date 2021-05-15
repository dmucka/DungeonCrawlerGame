﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DungeonCrawlerGame.Interfaces
{
    public interface IRenderable
    {
        int Id { get; }
        int X { get; }
        int Y { get; }
        int Width { get; }
        int Height { get; }
    }
}
