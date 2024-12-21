namespace SmartRide.src.DataStructures;

public class Graph<T>
{
    private readonly HashMap<T, List<(T neighbor, double weight)>> _adjacencyList;

    public Graph(int initialCapacity = 100)
    {
        _adjacencyList = new HashMap<T, List<(T, double)>>(initialCapacity);
    }

    // Adds a node to the graph
    public void AddVertex(T node)
    {
        if (!_adjacencyList.ContainsKey(node))
        {
            _adjacencyList.Put(node, new List<(T, double)>());
        }
    }

    // Adds an edge between two nodes
    public void AddEdge(T from, T to, double weight, bool isTwoWay = false)
    {
        if (weight <= 0) throw new ArgumentException("Edge weight must be positive.");

        AddVertex(from);
        AddVertex(to);

        // Add the edge from 'from' to 'to'
        _adjacencyList.Get(from).Add((to, weight));

        // If it's a two-way road, add the reverse edge
        if (isTwoWay)
        {
            _adjacencyList.Get(to).Add((from, weight));
        }
    }

    // Gets the neighbors of a node
    public List<(T neighbor, double weight)> GetNeighbors(T node)
    {
        if (!_adjacencyList.ContainsKey(node))
            throw new KeyNotFoundException($"Node {node} does not exist in the graph.");

        return _adjacencyList.Get(node)!;
    }

    // Clears the entire graph
    public void Clear()
    {
        foreach (var key in _adjacencyList.Keys())
        {
            _adjacencyList.Get(key).Clear();
        }
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
