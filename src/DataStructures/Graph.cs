namespace SmartRide.src.DataStructures;

public class Graph
{
    private readonly HashMap<int, List<(int neighbor, double weight)>> _adjacencyList;

    public Graph(int initialCapacity = 100)
    {
        _adjacencyList = new HashMap<int, List<(int, double)>>(initialCapacity);
    }

    // Adds a node to the graph
    public void AddNode(int node)
    {
        if (!_adjacencyList.ContainsKey(node))
        {
            _adjacencyList.Put(node, new List<(int, double)>());
        }
    }

    // Adds an edge between two nodes
    public void AddEdge(int from, int to, double weight, bool isTwoWay = false)
    {
        if (weight <= 0) throw new ArgumentException("Edge weight must be positive.");

        AddNode(from);
        AddNode(to);

        // Add the edge from 'from' to 'to'
        _adjacencyList.Get(from).Add((to, weight));

        // If it's a two-way road, add the reverse edge
        if (isTwoWay)
        {
            _adjacencyList.Get(to).Add((from, weight));
        }
    }

    // Gets the neighbors of a node
    public List<(int neighbor, double weight)> GetNeighbors(int node)
    {
        if (!_adjacencyList.ContainsKey(node))
            throw new KeyNotFoundException($"Node {node} does not exist in the graph.");

        // the _adjacencyList.Get(node) cannot be empty here
        return _adjacencyList.Get(node)!;
    }

    // Prints the adjacency list representation of the graph
    public void PrintGraph()
    {
        foreach (var (Key, Value) in _adjacencyList.ToList())
        {
            var neighbors = string.Join(", ", Value.Select(n => $"{n.neighbor} ({n.weight} KM)"));
            Console.WriteLine($"{Key}: {neighbors}");
        }
    }
}

