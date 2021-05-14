using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonCrawlerGame.Classes
{
    public class MatrixEnumerator : IEnumerable<(int X, int Y)>
    {
        public int StartX { get; }
        public int StartY { get; }
        public int Rows { get; }
        public int Columns { get; }

        public MatrixEnumerator(int rows, int columns)
        {
            StartX = 0;
            StartY = 0;
            Rows = rows;
            Columns = columns;
        }

        public MatrixEnumerator(Range xRange, Range yRange)
        {
            StartX = xRange.Start.Value;
            StartY = yRange.Start.Value;
            Rows = xRange.End.Value + 1;
            Columns = yRange.End.Value + 1;
        }

        public IEnumerator<(int X, int Y)> GetEnumerator()
        {
            for (int x = StartX; x < Rows; x++)
            {
                for (int y = StartY; y < Columns; y++)
                {
                    yield return (x, y);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
