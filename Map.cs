using System;
using System.IO;

namespace PathFindingAssignment
{
    // Represents a grid-based terrain map loaded from a text file.
    public class Map
    {
        public string Name { get; private set; }
        public int Rows { get; private set; }
        public int Cols { get; private set; }

        public GridPosition Start { get; private set; }
        public GridPosition Goal { get; private set; }

        // 2D array storing the terrain type at each cell
        public TerrainType[,] Grid { get; private set; }

        public Map(string filePath)
        {
            LoadFromFile(filePath);
        }

        // Reads the map file and fills in all the fields above
        private void LoadFromFile(string filePath)
        {
            Name = Path.GetFileNameWithoutExtension(filePath);
            string[] lines = File.ReadAllLines(filePath);

            if (lines.Length < 4)
                throw new Exception("Map file is too short.");

            // First line: rows and columns
            string[] sizeTokens = lines[0].Split(' ', '\t');
            Rows = int.Parse(sizeTokens[0]);
            Cols = int.Parse(sizeTokens[1]);

            // Second line: start row and column
            string[] startTokens = lines[1].Split(' ', '\t');
            Start = new GridPosition(int.Parse(startTokens[0]), int.Parse(startTokens[1]));

            // Third line: goal row and column
            string[] goalTokens = lines[2].Split(' ', '\t');
            Goal = new GridPosition(int.Parse(goalTokens[0]), int.Parse(goalTokens[1]));

            Grid = new TerrainType[Rows, Cols];

            int expectedLines = 3 + Rows;
            if (lines.Length < expectedLines)
                throw new Exception("Not enough rows in map file.");

            // Remaining lines store the terrain values
            for (int r = 0; r < Rows; r++)
            {
                string[] rowTokens = lines[3 + r].Split(' ', '\t');
                if (rowTokens.Length < Cols)
                    throw new Exception("Not enough columns on row " + r);

                for (int c = 0; c < Cols; c++)
                {
                    int value = int.Parse(rowTokens[c]);
                    Grid[r, c] = (TerrainType)value;
                }
            }
        }

        // True if (row, col) is inside the bounds of the grid
        public bool InBounds(int row, int col)
        {
            return row >= 0 && row < Rows && col >= 0 && col < Cols;
        }

        // True if the cell can be walked on (i.e. not a wall)
        public bool IsPassable(int row, int col)
        {
            return Grid[row, col] != TerrainType.Wall;
        }

        // Returns the movement cost for the given cell
        public int GetTerrainCost(int row, int col)
        {
            return (int)Grid[row, col];
        }

        // Prints a simple ASCII view of the map to the console.
        // If a path is supplied, it is drawn with '*' characters.
        public void PrintToConsole(GridPosition[] path)
        {
            Console.WriteLine("Map: " + Name);

            char[,] visual = new char[Rows, Cols];

            // First draw the base terrain symbols
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Cols; c++)
                {
                    switch (Grid[r, c])
                    {
                        case TerrainType.Open:
                            visual[r, c] = '.';
                            break;
                        case TerrainType.Wood:
                            visual[r, c] = 'W';
                            break;
                        case TerrainType.Water:
                            visual[r, c] = '~';
                            break;
                        case TerrainType.Wall:
                            visual[r, c] = '#';
                            break;
                        default:
                            visual[r, c] = '?';
                            break;
                    }
                }
            }

            // Overlay the path, if we have one
            if (path != null)
            {
                foreach (GridPosition pos in path)
                {
                    visual[pos.Row, pos.Col] = '*';
                }
            }

            // Mark start and goal explicitly
            visual[Start.Row, Start.Col] = 'S';
            visual[Goal.Row, Goal.Col] = 'G';

            // Finally print to the console
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Cols; c++)
                {
                    Console.Write(visual[r, c]);
                }
                Console.WriteLine();
            }
        }

        // Manhattan distance heuristic between two positions
        public static int ManhattanDistance(GridPosition a, GridPosition b)
        {
            int dr = Math.Abs(a.Row - b.Row);
            int dc = Math.Abs(a.Col - b.Col);
            return dr + dc;
        }
    }
}
