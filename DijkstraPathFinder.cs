using System;

namespace PathFindingAssignment
{
    // Dijkstra's algorithm.
    // Finds the lowest-cost path from start to goal, taking terrain costs into account.
    public class DijkstraPathFinder : IPathFinder
    {
        public string Name => "Dijkstra";

        public PathResult FindPath(Map map)
        {
            // dist[r, c] stores the best cost we have found so far to reach (r, c)
            int[,] dist = new int[map.Rows, map.Cols];

            // parent[r, c] lets us rebuild the path at the end
            GridPosition?[,] parent = new GridPosition?[map.Rows, map.Cols];

            // visited[r, c] = true once we have found the final cheapest cost for that cell
            bool[,] visited = new bool[map.Rows, map.Cols];

            const int INF = int.MaxValue / 4;

            // Initialise all distances to "infinity"
            for (int r = 0; r < map.Rows; r++)
            {
                for (int c = 0; c < map.Cols; c++)
                {
                    dist[r, c] = INF;
                }
            }

            // Distance to the start is zero
            dist[map.Start.Row, map.Start.Col] = 0;

            // Priority queue ordered by cost from start
            PriorityQueueAdapter<SearchNode> open =
                new PriorityQueueAdapter<SearchNode>((a, b) => a.CostFromStart.CompareTo(b.CostFromStart));

            // Add start node to open list
            open.Enqueue(new SearchNode(map.Start, 0, 0));

            int visitedCount = 0;

            int[] dr = { -1, 1, 0, 0 };
            int[] dc = { 0, 0, -1, 1 };

            while (!open.IsEmpty())
            {
                SearchNode node = open.Dequeue();
                GridPosition current = node.Pos;

                // Skip if we have already processed this cell
                if (visited[current.Row, current.Col]) continue;

                visited[current.Row, current.Col] = true;
                visitedCount++;

                // If this is the goal, we can reconstruct the path
                if (current.Equals(map.Goal))
                {
                    GridPosition[] path = ReconstructPath(map, parent);
                    return new PathResult(true, path, visitedCount);
                }

                // Relax all neighbours
                for (int i = 0; i < 4; i++)
                {
                    int nr = current.Row + dr[i];
                    int nc = current.Col + dc[i];

                    if (!map.InBounds(nr, nc)) continue;
                    if (!map.IsPassable(nr, nc)) continue;

                    // Cost to move into this neighbour (based on terrain)
                    int cost = map.GetTerrainCost(nr, nc);

                    int newDist = dist[current.Row, current.Col] + cost;

                    // If we found a cheaper path to (nr, nc), update it
                    if (newDist < dist[nr, nc])
                    {
                        dist[nr, nc] = newDist;
                        parent[nr, nc] = current;

                        // Push updated neighbour into the priority queue
                        open.Enqueue(new SearchNode(new GridPosition(nr, nc), newDist, 0));
                    }
                }
            }

            // No path found from start to goal
            return new PathResult(false, null, visitedCount);
        }

        // Rebuilds the path from goal back to start using the parent array
        private GridPosition[] ReconstructPath(Map map, GridPosition?[,] parent)
        {
            LinkedList<GridPosition> list = new LinkedList<GridPosition>();
            GridPosition current = map.Goal;

            while (true)
            {
                list.AddFirst(current);

                if (current.Equals(map.Start))
                    break;

                GridPosition? p = parent[current.Row, current.Col];
                if (!p.HasValue) break;

                current = p.Value;
            }

            GridPosition[] result = new GridPosition[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                result[i] = list.GetAt(i);
            }

            return result;
        }
    }
}
