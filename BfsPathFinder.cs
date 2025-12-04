using System;

namespace PathFindingAssignment
{
    // Breadth-First Search implementation.
    // Uses a queue and always explores all nodes at the current distance
    // before moving further away from the start.
    public class BfsPathFinder : IPathFinder
    {
        public string Name => "BFS";

        public PathResult FindPath(Map map)
        {
            // visited[r, c] = true once we have added this position to the queue
            bool[,] visited = new bool[map.Rows, map.Cols];

            // parent[r, c] stores the previous position on the path
            GridPosition?[,] parent = new GridPosition?[map.Rows, map.Cols];

            // Our own queue built on top of the LinkedList
            QueueAdapter<GridPosition> queue = new QueueAdapter<GridPosition>();

            // Start from the start position
            queue.Enqueue(map.Start);
            visited[map.Start.Row, map.Start.Col] = true;

            int visitedCount = 0;   // just for statistics

            // Directions: up, down, left, right
            int[] dr = { -1, 1, 0, 0 };   // N, S, W, E
            int[] dc = { 0, 0, -1, 1 };

            // Standard BFS loop
            while (!queue.IsEmpty())
            {
                GridPosition current = queue.Dequeue();
                visitedCount++;

                // If we reached the goal we can reconstruct the path
                if (current.Equals(map.Goal))
                {
                    GridPosition[] path = ReconstructPath(map, parent);
                    return new PathResult(true, path, visitedCount);
                }

                // Check all 4 neighbours of the current cell
                for (int i = 0; i < 4; i++)
                {
                    int nr = current.Row + dr[i];
                    int nc = current.Col + dc[i];

                    // Skip if out of bounds or a wall
                    if (!map.InBounds(nr, nc)) continue;
                    if (!map.IsPassable(nr, nc)) continue;

                    // Skip if we have already visited this cell
                    if (visited[nr, nc]) continue;

                    // Mark as visited and remember where we came from
                    visited[nr, nc] = true;
                    parent[nr, nc] = current;

                    // Add neighbour to the queue
                    queue.Enqueue(new GridPosition(nr, nc));
                }
            }

            // Queue emptied without reaching the goal → no path
            return new PathResult(false, null, visitedCount);
        }

        // Builds the path by walking backwards from the goal to the start
        private GridPosition[] ReconstructPath(Map map, GridPosition?[,] parent)
        {
            LinkedList<GridPosition> list = new LinkedList<GridPosition>();
            GridPosition current = map.Goal;

            while (true)
            {
                list.AddFirst(current); // add to front so path is start → goal

                if (current.Equals(map.Start))
                    break;

                GridPosition? p = parent[current.Row, current.Col];
                if (!p.HasValue) break;   // safety check

                current = p.Value;
            }

            // Convert our LinkedList into a simple array
            GridPosition[] result = new GridPosition[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                result[i] = list.GetAt(i);
            }

            return result;
        }
    }
}
