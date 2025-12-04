using System;
using System.IO;

namespace PathFindingAssignment
{
    // Main entry point of the application.
    // Handles user input, runs algorithms and saves outputs.
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("=== Path Finding Assignment ===");
            Console.WriteLine("Enter map file name (e.g. simpleMap.txt):");
            string fileName = Console.ReadLine();

            // Keep asking until a valid, existing file name is entered
            while (string.IsNullOrWhiteSpace(fileName) || !File.Exists(fileName))
            {
                Console.WriteLine("File not found. Please enter again:");
                fileName = Console.ReadLine();
            }

            // Load the map from the file
            Map map = new Map(fileName);

            bool again = true;
            while (again)
            {
                // Show the list of available algorithms
                PathFinderFactory.PrintMenu();
                string choice = Console.ReadLine();

                try
                {
                    // Create the chosen path finder and run it
                    IPathFinder finder = PathFinderFactory.Create(choice);
                    Console.WriteLine("Running " + finder.Name + " ...");

                    PathResult result = finder.FindPath(map);

                    if (result.Success)
                    {
                        Console.WriteLine("Path found. Length = " + result.Path.Length);
                        Console.WriteLine("Visited nodes = " + result.VisitedNodes);

                        // Print each position in the path
                        foreach (GridPosition pos in result.Path)
                        {
                            Console.WriteLine(pos);
                        }

                        // Show an ASCII view of the map and the path
                        map.PrintToConsole(result.Path);

                        // Save the path to a text file e.g. test1Map_Path_BFS.txt
                        string outFile = map.Name + "_Path_" + finder.Name.Replace(" ", "") + ".txt";
                        WritePathToFile(outFile, result);
                        Console.WriteLine("Path written to " + outFile);
                    }
                    else
                    {
                        Console.WriteLine("No path found.");
                    }
                }
                catch (Exception ex)
                {
                    // Basic error handling, just print the message
                    Console.WriteLine("Error: " + ex.Message);
                }

                Console.WriteLine("Run another algorithm on same map? (y/n)");
                string answer = Console.ReadLine();
                again = answer != null && answer.ToLower().StartsWith("y");
            }

            Console.WriteLine("For simple graphical view press G, otherwise press Enter to exit.");
            string gChoice = Console.ReadLine();
            if (gChoice != null && gChoice.ToLower() == "g")
            {
                // Run the Windows Forms viewer to show a graphical version of the map
                System.Windows.Forms.Application.EnableVisualStyles();
                System.Windows.Forms.Application.Run(new SimpleForm(map));
            }
        }

        // Writes the path (list of grid positions) into a text file
        private static void WritePathToFile(string fileName, PathResult result)
        {
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                foreach (GridPosition pos in result.Path)
                {
                    writer.WriteLine(pos.ToString());
                }
            }
        }
    }
}
