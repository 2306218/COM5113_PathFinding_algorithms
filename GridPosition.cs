using System;

namespace PathFindingAssignment
{
    // Simple struct to represent a cell in the grid.
    // Stores its row and column.
    public struct GridPosition
    {
        public int Row;
        public int Col;

        public GridPosition(int row, int col)
        {
            Row = row;
            Col = col;
        }

        // Converts the position to "row col" format,
        // which matches the format in the path output file.
        public override string ToString()
        {
            return Row + " " + Col;
        }

        // Equality based on row and column values.
        public override bool Equals(object obj)
        {
            if (!(obj is GridPosition)) return false;

            GridPosition other = (GridPosition)obj;
            return Row == other.Row && Col == other.Col;
        }

        // Simple hash code combining row and column.
        public override int GetHashCode()
        {
            return Row * 397 ^ Col;
        }
    }
}
