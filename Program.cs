global using SmartRide.Models;
global using SmartRide.src.Dtos;
global using SmartRide.src.Services;
global using SmartRide.src.DataStructures;
global using Microsoft.EntityFrameworkCore;
using SmartRide.src.Utilities.GraphAlgos;

namespace SmartRide
{
    class Program
    {
        static void Main(string[] args)
        {
            var dbContext = new SmartRideDbContext();

            // Create a graph for testing
            var graph = new Graph<string>();

            // Add nodes to the graph
            graph.AddVertex("A");
            graph.AddVertex("B");
            graph.AddVertex("C");
            graph.AddVertex("D");
            graph.AddVertex("E");

            // Add edges (with weights)
            graph.AddEdge("A", "B", 1);  // A -> B with weight 1
            graph.AddEdge("A", "C", 4);  // A -> C with weight 4
            graph.AddEdge("B", "C", 2);  // B -> C with weight 2
            graph.AddEdge("B", "D", 5);  // B -> D with weight 5
            graph.AddEdge("C", "E", 1);  // C -> E with weight 1
            graph.AddEdge("D", "E", 2);  // D -> E with weight 2

            // Instantiate the ShortestPath algorithm
            var shortestPath = new ShortestPath<string>();

            // Define start and goal nodes
            string start = "A";
            string goal = "E";

            // Find the shortest path
            try
            {
                var path = shortestPath.Dijsktra(graph, start, goal);

                // Output the path if one exists
                if (path.Count > 0)
                {
                    Console.WriteLine($"The shortest path from {start} to {goal} is:");
                    Console.WriteLine(string.Join(" -> ", path));
                }
                else
                {
                    Console.WriteLine($"No path found from {start} to {goal}.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
