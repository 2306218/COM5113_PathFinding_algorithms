using System;

namespace PathFindingAssignment
{
    // Factory class used to create the correct path-finder object
    // based on the user's menu choice.
    public static class PathFinderFactory
    {
        public static IPathFinder Create(string option)
        {
            switch (option)
            {
                case "1":
                    return new BfsPathFinder();
                case "2":
                    return new DfsPathFinder();
                case "3":
                    return new HillClimbingPathFinder();
                case "4":
                    return new BestFirstPathFinder();
                case "5":
                    return new DijkstraPathFinder();
                default:
                    throw new ArgumentException("Unknown algorithm option");
            }
        }

        // Prints the list of algorithms for the user
        public static void PrintMenu()
        {
            Console.WriteLine("Choose algorithm:");
            Console.WriteLine("1 - Breadth First Search");
            Console.WriteLine("2 - Depth First Search");
            Console.WriteLine("3 - Hill Climbing");
            Console.WriteLine("4 - Best First");
            Console.WriteLine("5 - Dijkstra");
        }
    }
}
