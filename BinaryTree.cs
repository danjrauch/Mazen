using System;
using System.Collections.Generic;

namespace Mazen
{
    public class BinaryTreeMaze : Grid
    {
        public BinaryTreeMaze(ulong _rows, ulong _cols) : base(_rows, _cols)
        {
            Random rng = new Random();

            foreach (var cell in _grid)
            {
                List<Direction> possibleNeighbors = new List<Direction>();
                if (cell.Row > 0)
                    possibleNeighbors.Add(Direction.North);
                if (cell.Col < _cols - 1)
                    possibleNeighbors.Add(Direction.East);

                if (possibleNeighbors.Count > 0)
                {
                    int idx = rng.Next(possibleNeighbors.Count);
                    cell.Link(possibleNeighbors[idx]);
                }
            }
        }
    }
}