using System;
using System.Collections.Generic;

namespace Mazen
{
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

        public Cell GetNeighbor(Direction dir)
        {
            (int, int) vec = Grid.Moves[dir];
            ulong row = vec.Item1 == -1 ? _row - 1 : vec.Item1 == 1 ? _row + 1 : _row;
            ulong col = vec.Item2 == -1 ? _col - 1 : vec.Item2 == 1 ? _col + 1 : _col;
            return _grid[row, col];
        }

        public bool IsLinked(Direction dir) => _edges.Contains(dir);

        public void Link(Direction dir, bool bidi = true)
        {
            _edges.Add(dir);
            if (bidi)
            {
                Cell neighbor = GetNeighbor(dir);
                neighbor.Link(Grid.inverseMoves[dir], false);
            }
        }

        public void Unlink(Direction dir, bool bidi = true)
        {
            _edges.Remove(dir);
            if (bidi)
            {
                Cell neighbor = GetNeighbor(dir);
                neighbor.Unlink(Grid.inverseMoves[dir], false);
            }
        }

        public List<Cell> GetNeighbors()
        {
            List<Cell> validMoves = new List<Cell>();
            foreach (var dir in _edges)
            {
                validMoves.Add(GetNeighbor(dir));
            }
            return validMoves;
        }

        public override string ToString() => $"[{_row.ToString()}, {_col.ToString()}]";
    }
}