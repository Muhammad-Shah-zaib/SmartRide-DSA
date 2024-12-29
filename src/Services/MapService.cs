namespace SmartRide.src.Services
{
    public class MapService
    {
        public readonly Graph<string> _graph;
        private readonly SmartRideDbContext _context;

        public MapService(SmartRideDbContext context)
        {
            _graph = new Graph<string>();
            _context = context;

            this.LoadGraphFromDatabase();
        }

        public void AddNode(string name, string type)
        {
            // Add node to the database
            var node = new Node { Name = name, Type = type };
            _context.Nodes.Add(node);
            _context.SaveChanges();

            // Add node to the graph
            _graph.AddVertex(name);
        }

        public void AddEdge(string source, string destination, double weight, bool isOneWay)
        {
            // Get source and destination nodes from the database
            var sourceNode = _context.Nodes.FirstOrDefault(n => n.Name == source);
            var destinationNode = _context.Nodes.FirstOrDefault(n => n.Name == destination);

            if (sourceNode == null || destinationNode == null)
                throw new Exception("Source or destination node not found!");

            // Add edge to the database
            var edge = new Edge
            {
                SourceId = sourceNode.Id,
                DestinationId = destinationNode.Id,
                Weight = (decimal)weight,
                IsOneWay = isOneWay
            };
            _context.Edges.Add(edge);
            _context.SaveChanges();

            // Add edge to the graph
            _graph.AddEdge(source, destination, weight);
            if (!isOneWay)
            {
                _graph.AddEdge(destination, source, weight);
            }
        }
        //print all the vertices
        public void PrintVertices()
        {
            var nodes = _graph.GetAllNodes().ToList();
            for (int i = 0; i < nodes.Count; i += 2)
            {
                string firstColumn = nodes[i].ToString();
                string secondColumn = (i + 1 < nodes.Count) ? nodes[i + 1].ToString() : string.Empty;
                Console.WriteLine($"{firstColumn,-15}{secondColumn}");
            }
        }
        public void LoadGraphFromDatabase()
        {
            // Clear current graph
            _graph.Clear();

            // Load nodes from the database
            var nodes = _context.Nodes.ToList();
            foreach (var node in nodes)
            {
                _graph.AddVertex(node.Name.ToUpper());
            }

            // Load edges from the database
            var edges = _context.Edges.ToList();
            foreach (var edge in edges)
            {
                Console.WriteLine($"{edge.Source.Name.ToUpper()} -> {edge.Destination.Name.ToUpper()} -- {edge.IsOneWay}");
                var source = edge.Source.Name.ToUpper();
                var destination = edge.Destination.Name.ToUpper();

                _graph.AddEdge(source, destination, (double)edge.Weight);
                if (!edge.IsOneWay)
                {
                    _graph.AddEdge(destination, source, (double)edge.Weight);
                }
            }
        }
    }
}
