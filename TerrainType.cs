namespace PathFindingAssignment
{
    // Types of terrain on the map.
    // The integer values are also used as movement costs.
    public enum TerrainType
    {
        Wall = 0,   // Cannot move through this cell
        Open = 1,   // Normal ground
        Wood = 2,   // Slightly more expensive
        Water = 3   // Most expensive terrain (still passable)
    }
}
