using System;
using System.Collections;
using System.IO;
using System.Drawing;
        
namespace Mazen
{
    class Program
    {
        static void Main(string[] args)
        {
            ulong rows = 100;
            ulong cols = 100;

            BinaryTreeMaze btm = new BinaryTreeMaze(rows, cols);

            //Console.WriteLine(btm.ToString());
            if (!Directory.Exists("renderings"))
            {
                Directory.CreateDirectory("renderings");
            }
            // btm.ToPng().Save("renderings/maze.png");
            Bitmap mazeImage = btm.ToPng();
            
            Cell source = btm[0, 0];
            Cell dest = btm[rows-1, 0];

            BFS.CalculateShortestPath(source, dest);

            // TODO Refactor this to a OverlayPath method \/\/\/\/\/

            using (var graphics = Graphics.FromImage(mazeImage))
            {
                ulong cellSize = 10;
                Pen wall = Pens.Black;

                foreach (var cell in btm)
                {
                    ulong x1 = cell.Col * cellSize;
                    ulong y1 = cell.Row * cellSize;
                    ulong x2 = (cell.Col + 1) * cellSize;
                    ulong y2 = (cell.Row + 1) * cellSize;

                    Color color = !cell.HasProperty($"{source.ToString()} - {dest.ToString()}") ? Color.OldLace : Color.IndianRed;
                    Brush brush = new SolidBrush(color);
                    graphics.FillRectangle(brush, x1, y1, (x2 - x1), (y2 - y1));
                }

                foreach (var cell in btm)
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

            // /\/\/\/\/\/\/\/\/\/\/\

            mazeImage.Save("renderings/maze.png");
        }
    }
}
