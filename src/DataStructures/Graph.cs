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

    public IEnumerable<T> GetAllNodes()
    {
        return _adjacencyList.Keys();
    }

    // Clears the entire graph
    public void Clear()
    {
        foreach (var key in _adjacencyList.Keys())
        {
            _adjacencyList.Get(key).Clear();
        }
    }

    public bool ContainsNode(T node)
    {
        return _adjacencyList.ContainsKey(node);
        
    }

    //weight of edge between two nodes
    public double GetEdgeWeight(T from, T to)
    {
        if (_adjacencyList.ContainsKey(from))
        {
            var neighbors = _adjacencyList.Get(from);
            foreach (var (neighbor, weight) in neighbors)
            {
                if (neighbor.Equals(to))
                    return weight;
            }
        }

        // Return a default value (or throw an exception) if no edge exists
        throw new ArgumentException($"No edge exists between {from} and {to}");
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
