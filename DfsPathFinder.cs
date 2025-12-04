using System;

namespace PathFindingAssignment
{
    // Depth-First Search implementation.
    // Uses a stack and explores as deep as possible before backtracking.
    public class DfsPathFinder : IPathFinder
    {
        public string Name => "DFS";

        public PathResult FindPath(Map map)
        {
            bool[,] visited = new bool[map.Rows, map.Cols];
            GridPosition?[,] parent = new GridPosition?[map.Rows, map.Cols];

            // Our own stack implemented using the LinkedList
            StackAdapter<GridPosition> stack = new StackAdapter<GridPosition>();

            // Push the start position on the stack
            stack.Push(map.Start);

            int visitedCount = 0;

            int[] dr = { -1, 1, 0, 0 };
            int[] dc = { 0, 0, -1, 1 };

            while (!stack.IsEmpty())
            {
                GridPosition current = stack.Pop();

                // Because DFS can push the same cell multiple times,
                // we check and skip if we have already processed it.
                if (visited[current.Row, current.Col]) continue;

                visited[current.Row, current.Col] = true;
                visitedCount++;

                if (current.Equals(map.Goal))
                {
                    GridPosition[] path = ReconstructPath(map, parent);
                    return new PathResult(true, path, visitedCount);
                }

                // Explore neighbours
                for (int i = 0; i < 4; i++)
                {
                    int nr = current.Row + dr[i];
                    int nc = current.Col + dc[i];

                    if (!map.InBounds(nr, nc)) continue;
                    if (!map.IsPassable(nr, nc)) continue;
                    if (visited[nr, nc]) continue;

                    // Record that we came to (nr, nc) from current
                    parent[nr, nc] = current;

                    // Push neighbour on the stack (LIFO order)
                    stack.Push(new GridPosition(nr, nc));
                }
            }

            // Stack empty and goal not reached → no path
            return new PathResult(false, null, visitedCount);
        }

        // Same reconstruction as BFS, using the parent array
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
