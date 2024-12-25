namespace SmartRide.src.Utilities.GraphAlgos;


public class ShortestPath<T> where T : IComparable<T>
{
    public List<T> Dijsktra(Graph<T> adjacencyList, T start, T goal, ref double totalCost)
    {
        var distances = new HashMap<T, double>();
        var Nodes = new HashMap<T, T>();
        var priorityQueue = new PriorityQueues<T>();
        var path = new List<T>();

        // Initialize the total cost to 0
        totalCost = 0;

        // Checking if the start or goal nodes exist in the graph
        if (!adjacencyList.ContainsNode(start) || !adjacencyList.ContainsNode(goal))
            throw new ArgumentException("Start or goal node is not present in the graph.");

        // Initialize distances with a large value, except for the start node
        foreach (var vertex in adjacencyList.GetAllNodes())
        {
            distances.Put(vertex, double.MaxValue);
        }
        distances.Put(start, 0);

        // Enqueue the start node with distance 0
        priorityQueue.Enqueue(start, 0);

        // Process the nodes until the priority queue is empty
        while (priorityQueue.Count > 0)
        {
            var (currentNode, currentDistance) = priorityQueue.Dequeue();

            // If the goal node is reached, we can break and trace the path
            if (EqualityComparer<T>.Default.Equals(currentNode, goal))
            {
                path = makepath(Nodes, start, goal);

                // Calculate total cost by summing up the weights of the edges in the path
                totalCost = currentDistance; // Total cost is the distance of the goal node
                break;
            }

            // Visit each neighbor of the current node
            foreach (var neighbor in adjacencyList.GetNeighbors(currentNode))
            {
                var node = neighbor.Item1;
                var weight = neighbor.Item2;
                var new_distance = currentDistance + weight;

                // If a shorter path to the neighbor is found, update its distance
                if (new_distance < distances.Get(node))
                {
                    distances.Put(node, new_distance);
                    Nodes.Put(node, currentNode);
                    priorityQueue.Enqueue(node, new_distance);
                }
            }
        }

        return path;
    }


    private List<T> makepath(HashMap<T,T> visited,T start,T goal)
    {   

        var path = new List<T>();
        var current = goal;
        
        while (!EqualityComparer<T>.Default.Equals(current, start))
        {
            path.Add(current);
            current = visited.Get(current);

        }

        path.Add(start); // Add the start node at the end
        path.Reverse(); // Reverse to get the path from start to goal
        return path;
    }
}
