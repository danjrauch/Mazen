using System;
using System.Collections.Generic;
using System.Dynamic;

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
        private Dictionary<string, dynamic> _properties;

        public Cell(Grid grid, ulong row, ulong col)
        {
            _grid = grid;
            _row = row;
            _col = col;
            _edges = new HashSet<Direction>();
            _properties = new Dictionary<string, dynamic>();
        }

        public Cell GetNeighbor(Direction dir)
        {
            Tuple<int, int> vec = Grid.Moves[dir];
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

        public void SetProperty(string key, dynamic val)
        {
            _properties[key] = val;
        }

        public bool HasProperty(string key)
        {
            return _properties.ContainsKey(key);
        }

        public dynamic GetProperty(string key)
        {
            if (_properties.ContainsKey(key))
            {
                return _properties[key];
            }
            return null;
        }

        public bool IsLinked(Direction dir)
        {
            return _edges.Contains(dir);
        }

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

        public override string ToString()
        {
            return $"[{_row.ToString()}, {_col.ToString()}]";
        }
    }
}