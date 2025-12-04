namespace PathFindingAssignment
{
    // Simple class to store the result of a path-finding run
    public class PathResult
    {
        // True if the algorithm found a route from start to goal
        public bool Success;

        // The sequence of grid positions that form the final path
        public GridPosition[] Path;

        // How many nodes/cells were visited during the search
        public int VisitedNodes;

        public PathResult(bool success, GridPosition[] path, int visitedNodes)
        {
            Success = success;
            Path = path;
            VisitedNodes = visitedNodes;
        }
    }

    // Interface that all path-finding algorithms implement
    public interface IPathFinder
    {
        // Name of the algorithm (e.g. "BFS", "Dijkstra")
        string Name { get; }

        // Run the algorithm on the supplied map and return the result
        PathResult FindPath(Map map);
    }
}
