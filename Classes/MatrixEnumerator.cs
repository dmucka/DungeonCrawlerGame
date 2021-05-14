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
        public int Rows { get; }
        public int Columns { get; }

        public MatrixEnumerator(int rows, int columns)
        {
            Rows = rows;
            Columns = columns;
        }

        public IEnumerator<(int X, int Y)> GetEnumerator()
        {
            for (int x = 0; x < Rows; x++)
            {
                for (int y = 0; y < Columns; y++)
                {
                    yield return (x, y);
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
