using System;
using System.Collections;
using System.Collections.Generic;

namespace Mazen
{
    class BFS
    {
        public static void CalculateShortestPath(Cell source, Cell dest)
        {
            Queue<Cell> q = new Queue<Cell>();

            string distLabel = $"{source.ToString()}";
            string pathLabel = $"{source.ToString()} - {dest.ToString()}";

            source.SetProperty(distLabel, 0);
            q.Enqueue(source);

            bool found = false;

            while (q.Count != 0)
            {
                Cell c = q.Dequeue();
                if(c == dest)
                {
                    found = true;
                    break;
                }
                foreach (var cell in c.GetNeighbors())
                {
                    if(!cell.HasProperty(distLabel))
                    {
                        cell.SetProperty(distLabel, c.GetProperty(distLabel) + 1);
                        q.Enqueue(cell);
                    }
                }
            }
            
            if (found)
            {
                q.Enqueue(dest);

                while (q.Count != 0)
                {
                    Cell c = q.Dequeue();
                    c.SetProperty(pathLabel, true);
                    if (c == source)
                    {
                        break;
                    } 
                    foreach (var cell in c.GetNeighbors())
                    {
                        if (c.GetProperty(distLabel) == cell.GetProperty(distLabel) + 1)
                        {
                            q.Enqueue(cell);
                            break;
                        }
                    }
                }
            }
        }
    }
}