using System;
using System.Collections.Generic;

namespace Mazen
{
    public class BinaryTreeMaze : Grid
    {
        public BinaryTreeMaze(ulong _rows, ulong _cols) : base(_rows, _cols)
        {
            Random rng = new Random();

            for (ulong i = 0; i < _rows; ++i)
            {
                for (ulong j = 0; j < _cols; ++j)
                {
                    List<Direction> possibleNeighbors = new List<Direction>();
                    if (i > 0)
                        possibleNeighbors.Add(Direction.North);
                    if (j < _cols - 1)
                        possibleNeighbors.Add(Direction.East);

                    if (possibleNeighbors.Count > 0)
                    {
                        int idx = rng.Next(possibleNeighbors.Count);
                        _grid[i, j].Link(possibleNeighbors[idx]);
                    }
                }
            }
        }
    }
}