namespace PathFindingAssignment
{
    // Node used in the search algorithms.
    // Stores the grid position, the cost so far and the heuristic value.
    public struct SearchNode
    {
        public GridPosition Pos;      // Where this node is in the grid
        public int CostFromStart;     // g-cost for Dijkstra / A*-style algorithms
        public int Heuristic;         // h-cost (e.g. Manhattan distance to goal)

        public SearchNode(GridPosition pos, int costFromStart, int heuristic)
        {
            Pos = pos;
            CostFromStart = costFromStart;
            Heuristic = heuristic;
        }
    }
}
