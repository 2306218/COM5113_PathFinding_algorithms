using System;

namespace PathFindingAssignment
{
    // Hill Climbing algorithm.
    // Always moves to the neighbour that appears closest to the goal (heuristic).
    // Does not backtrack, so it can easily get stuck in a local minimum.
    public class HillClimbingPathFinder : IPathFinder
    {
        public string Name => "Hill Climbing";

        public PathResult FindPath(Map map)
        {
            bool[,] visited = new bool[map.Rows, map.Cols];

            // We keep the path as we go (no parent array here)
            LinkedList<GridPosition> pathList = new LinkedList<GridPosition>();

            GridPosition current = map.Start;
            visited[current.Row, current.Col] = true;
            pathList.AddLast(current);

            int visitedCount = 0;

            int[] dr = { -1, 1, 0, 0 };
            int[] dc = { 0, 0, -1, 1 };

            while (true)
            {
                visitedCount++;

                // If we reached the goal, turn the LinkedList into an array and return
                if (current.Equals(map.Goal))
                {
                    GridPosition[] result = new GridPosition[pathList.Count];

                    for (int i = 0; i < pathList.Count; i++)
                        result[i] = pathList.GetAt(i);

                    return new PathResult(true, result, visitedCount);
                }

                // Current heuristic value (distance from current to goal)
                int currentH = Map.ManhattanDistance(current, map.Goal);
                int bestH = currentH;
                GridPosition? bestNeighbour = null;

                // Look at the 4 neighbours and pick the one with the smallest heuristic
                for (int i = 0; i < 4; i++)
                {
                    int nr = current.Row + dr[i];
                    int nc = current.Col + dc[i];

                    if (!map.InBounds(nr, nc)) continue;
                    if (!map.IsPassable(nr, nc)) continue;
                    if (visited[nr, nc]) continue;

                    GridPosition next = new GridPosition(nr, nc);
                    int h = Map.ManhattanDistance(next, map.Goal);

                    // Only accept neighbours that improve the heuristic
                    if (h < bestH)
                    {
                        bestH = h;
                        bestNeighbour = next;
                    }
                }

                // If we couldn't find any better neighbour, we are stuck
                if (!bestNeighbour.HasValue)
                {
                    return new PathResult(false, null, visitedCount);
                }

                // Move to the best neighbour
                current = bestNeighbour.Value;
                visited[current.Row, current.Col] = true;
                pathList.AddLast(current);
            }
        }
    }
}
