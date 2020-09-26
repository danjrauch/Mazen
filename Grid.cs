using System;
using System.Collections.Generic;
using System.Drawing;

namespace Mazen
{
    public enum Direction
    {
        North,
        South,
        East,
        West
    }

    public class Grid : IEnumerable<Cell>
    {
        public static readonly Dictionary<Direction, (int, int)> Moves =
            new Dictionary<Direction, (int, int)>{
                { Direction.North, (-1, 0) },
                { Direction.South, (1, 0) },
                { Direction.East, (0, 1) },
                { Direction.West, (0, -1) },
            };

        public static readonly Dictionary<Direction, Direction> inverseMoves =
            new Dictionary<Direction, Direction>{
                { Direction.North, Direction.South },
                { Direction.South, Direction.North },
                { Direction.West, Direction.East },
                { Direction.East, Direction.West },
            };

        protected ulong _rows;
        protected ulong _cols;
        protected Cell[,] _grid;

        public Grid(ulong rows, ulong cols)
        {
            _rows = rows;
            _cols = cols;
            _grid = new Cell[rows, cols];
            for (ulong i = 0; i < rows; ++i)
                for (ulong j = 0; j < cols; ++j)
                    _grid[i, j] = new Cell(this, i, j);
        }

        public Cell this[ulong row, ulong col]
        {
            get
            {
                if (row < 0 || row >= _rows || col < 0 || col >= _cols)
                {
                    throw new IndexOutOfRangeException("Row and column position outside boundaries.");
                }
                return _grid[row, col];
            }
            set
            {
                if (!(row < 0 || row >= _rows || col < 0 || col >= _cols))
                {
                    _grid[row, col] = value;
                }
            }
        }

        public IEnumerator<Cell> GetEnumerator()
        {
            for (ulong i = 0; i < _rows; ++i)
            {
                for (ulong j = 0; j < _cols; ++j)
                {
                    yield return _grid[i, j];
                }
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            string output = "+";
            for (ulong i = 0; i < _cols; ++i)
            {
                output += "---+";
            }
            output += "\n";

            for (ulong i = 0; i < _rows; ++i)
            {
                string top = "|";
                string bottom = "+";

                for (ulong j = 0; j < _cols; ++j)
                {
                    Cell cell = _grid[i, j];

                    string body = "   ";
                    string east_boundary = cell.IsLinked(Direction.East) ? " " : "|";
                    top += body + east_boundary;

                    string south_boundary = cell.IsLinked(Direction.South) ? "   " : "---";
                    string corner = "+";
                    bottom += south_boundary + corner;
                }

                output += top + "\n";
                output += bottom + "\n";
            }

            return output;
        }

        public Bitmap ToPng(ulong cellSize = 10, bool includeBackgrounds = true)
        {
            ulong imgWidth = cellSize * _cols;
            ulong imgHeight = cellSize * _rows;

            Brush background = Brushes.White;
            Pen wall = Pens.Black;

            Bitmap mazeImage = new Bitmap((int)imgWidth, (int)imgHeight);

            using (var graphics = Graphics.FromImage(mazeImage))
            {
                graphics.FillRectangle(background, 0, 0, imgWidth, imgHeight);

                if (includeBackgrounds)
                    foreach (var cell in _grid)
                    {
                        ulong x1 = cell.Col * cellSize;
                        ulong y1 = cell.Row * cellSize;
                        ulong x2 = (cell.Col + 1) * cellSize;
                        ulong y2 = (cell.Row + 1) * cellSize;

                        Color color = Color.OldLace; //IndianRed for path color
                        Brush brush = new SolidBrush(color);
                        graphics.FillRectangle(brush, x1, y1, (x2 - x1), (y2 - y1));
                    }

                foreach (var cell in _grid)
                {
                    ulong x1 = cell.Col * cellSize;
                    ulong y1 = cell.Row * cellSize;
                    ulong x2 = (cell.Col + 1) * cellSize;
                    ulong y2 = (cell.Row + 1) * cellSize;

                    if (!cell.IsLinked(Direction.North))
                        graphics.DrawLine(wall, x1, y1, x2, y1);
                    if (!cell.IsLinked(Direction.West))
                        graphics.DrawLine(wall, x1, y1, x1, y2);
                    if (!cell.IsLinked(Direction.East))
                        graphics.DrawLine(wall, x2, y1, x2, y2);
                    if (!cell.IsLinked(Direction.South))
                        graphics.DrawLine(wall, x1, y2, x2, y2);
                }
            }

            return mazeImage;
        }
    }
}