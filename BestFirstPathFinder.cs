using System;

namespace PathFindingAssignment
{
    // Path-finding algorithm: Best-First Search
    // This algorithm always expands the node that is closest to the goal
    // according to the heuristic (Manhattan distance).
    public class BestFirstPathFinder : IPathFinder
    {
        public string Name => "Best First";

        public PathResult FindPath(Map map)
        {
            // Tracks which grid cells we have already visited
            bool[,] visited = new bool[map.Rows, map.Cols];

            // Stores each cell's parent so we can rebuild the path at the end
            GridPosition?[,] parent = new GridPosition?[map.Rows, map.Cols];

            // Priority queue ordered by heuristic only (lower h = higher priority)
            PriorityQueueAdapter<SearchNode> open =
                new PriorityQueueAdapter<SearchNode>((a, b) => a.Heuristic.CompareTo(b.Heuristic));

            // Create the starting node
            SearchNode startNode = new SearchNode(
                map.Start,
                0, // Best-First does not use actual cost
                Map.ManhattanDistance(map.Start, map.Goal));

            // Put the start into the open list and mark it visited
            open.Enqueue(startNode);
            visited[map.Start.Row, map.Start.Col] = true;

            int visitedCount = 0; // Used for output statistics

            // Movement directions: up, down, left, right
            int[] dr = { -1, 1, 0, 0 };
            int[] dc = { 0, 0, -1, 1 };

            // Main search loop
            while (!open.IsEmpty())
            {
                // Remove the node with the smallest heuristic value
                SearchNode node = open.Dequeue();
                GridPosition current = node.Pos;
                visitedCount++;

                // If we reached the goal, rebuild and return the path
                if (current.Equals(map.Goal))
                {
                    GridPosition[] path = ReconstructPath(map, parent);
                    return new PathResult(true, path, visitedCount);
                }

                // Explore the 4 neighbours of the current cell
                for (int i = 0; i < 4; i++)
                {
                    int nr = current.Row + dr[i];
                    int nc = current.Col + dc[i];

                    // Skip neighbours that are outside the map
                    if (!map.InBounds(nr, nc)) continue;

                    // Skip walls / blocked cells
                    if (!map.IsPassable(nr, nc)) continue;

                    // Skip cells we already visited
                    if (visited[nr, nc]) continue;

                    GridPosition next = new GridPosition(nr, nc);

                    // Heuristic = Manhattan distance to goal
                    int h = Map.ManhattanDistance(next, map.Goal);

                    // Best-First only uses heuristic, actual cost not considered
                    SearchNode neighbour = new SearchNode(next, 0, h);

                    open.Enqueue(neighbour);
                    visited[nr, nc] = true;

                    // Store how we reached this cell
                    parent[nr, nc] = current;
                }
            }

            // If the open list becomes empty, no path exists
            return new PathResult(false, null, visitedCount);
        }

        // Reconstructs the path by walking backwards from the goal to the start
        private GridPosition[] ReconstructPath(Map map, GridPosition?[,] parent)
        {
            LinkedList<GridPosition> list = new LinkedList<GridPosition>();
            GridPosition current = map.Goal;

            while (true)
            {
                // Add the current cell to the front of our list
                list.AddFirst(current);

                // Stop when we reach the starting position
                if (current.Equals(map.Start)) break;

                // Move to the parent of the current cell
                GridPosition? p = parent[current.Row, current.Col];
                if (!p.HasValue) break;
                current = p.Value;
            }

            // Convert our LinkedList into an array for the PathResult
            GridPosition[] result = new GridPosition[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                result[i] = list.GetAt(i);
            }

            return result;
        }
    }
}
