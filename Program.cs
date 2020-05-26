using System;
using System.Collections;
        
namespace Mazen
{
    class Program
    {
        static void Main(string[] args)
        {
            ulong rows = 40;
            ulong cols = 40;

            // var grid = new Grid(rows, cols);

            // for(ulong i = 0; i < rows-1; ++i)
            // {
            //     grid[i, 0].Link(Grid.Direction.South);
            // }

            // for(ulong i = 0; i < cols-1; ++i)
            // {
            //     grid[rows-1, i].Link(Grid.Direction.East);
            // }

            // Console.WriteLine(grid.ToString());

            BinaryTreeMaze btm = new BinaryTreeMaze(rows, cols);

            Console.WriteLine(btm.ToString());
        }
    }
}
