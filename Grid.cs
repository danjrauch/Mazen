using System;
using System.Collections.Generic;

namespace Mazen
{
    public class Grid
    {
        public enum Direction
        {
            North,
            South,
            East,
            West
        }

        private static readonly Dictionary<Direction, Tuple<int, int>> Moves =
            new Dictionary<Direction, Tuple<int, int>>{
                { Direction.North, new Tuple<int, int>(-1, 0) },
                { Direction.South, new Tuple<int, int>(1, 0) },
                { Direction.East, new Tuple<int, int>(0, 1) },
                { Direction.West, new Tuple<int, int>(0, -1) },
            };

        private static readonly Dictionary<Direction, Direction> inverseMoves = 
            new Dictionary<Direction, Direction>{
                { Direction.North, Direction.South },
                { Direction.South, Direction.North },
                { Direction.West, Direction.East },
                { Direction.East, Direction.West },
            };

        protected ulong _rows;
        protected ulong _cols;
        protected Cell[,] _grid;

        public class Cell
        {
            private Grid _grid;
            private ulong _row;
            public ulong Row
            {
                get => _row;
            }
            private ulong _col;
            public ulong Col
            {
                get => _col;
            }
            private HashSet<Direction> _edges;

            public Cell(Grid grid, ulong row, ulong col)
            {
                _grid = grid;
                _row = row;
                _col = col;
                _edges = new HashSet<Direction>();
            }

            public bool HasEdge(Direction dir)
            {
                return _edges.Contains(dir);
            }

            public Cell GetNeighbor(Direction dir)
            {
                Tuple<int, int> vec = Moves[dir];
                ulong row = _row, col = _col;
                switch (vec.Item1)
                {
                    case -1:
                        row--;
                        break;
                    case 1:
                        row++;
                        break;
                }
                switch (vec.Item2)
                {
                    case -1:
                        col--;
                        break;
                    case 1:
                        col++;
                        break;
                }
                return _grid[row, col];
            }

            public void Link(Direction dir, bool bidi = true)
            {
                _edges.Add(dir);
                if (bidi)
                {
                    Cell neighbor = GetNeighbor(dir);
                    neighbor.Link(inverseMoves[dir], false);
                }
            }

            public void Unlink(Direction dir, bool bidi = true)
            {
                _edges.Remove(dir);
                if (bidi)
                {
                    Cell neighbor = GetNeighbor(dir);
                    neighbor.Unlink(inverseMoves[dir], false);
                }
            }

            public List<Cell> GetValidMoves()
            {
                List<Cell> validMoves = new List<Cell>();
                foreach (var dir in _edges)
                {
                    validMoves.Add(GetNeighbor(dir));
                }
                return validMoves;
            }
        }

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
                return _grid[row, col];
            }
            set
            {
                _grid[row, col] = value;
            }
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
                    string east_boundary = cell.HasEdge(Direction.East) ? " " : "|";
                    top += body + east_boundary;

                    string south_boundary = cell.HasEdge(Direction.South) ? "   " : "---";
                    string corner = "+";
                    bottom += south_boundary + corner;
                }

                output += top + "\n";
                output += bottom + "\n";
            }

            return output;
        }
    }
}