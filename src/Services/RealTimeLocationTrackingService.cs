namespace SmartRide.src.Services;

public class RealTimeLocationTrackingService
{
    private readonly MapService _mapService;
    private readonly ShortestPath<string> _shortestPath;

    public RealTimeLocationTrackingService(MapService mapService)
    {
        _mapService = mapService;
        _shortestPath = new ShortestPath<string>();
    }

    public (DriverDto nearestDriver, double cost, List<string> route) FindNearestDriver(List<DriverDto> drivers, string userLocation)
    {
        if (drivers == null || drivers.Count == 0)
            throw new ArgumentException("Driver list cannot be null or empty.");

        DriverDto? nearestDriver = null;
        double minCost = double.MaxValue;
        List<string>? bestRoute = null;

        foreach (var driver in drivers)
        {
            if (driver?.CurrentPosition == null)
                continue;

            double cost = 0;
            var route = _shortestPath.Dijsktra(
                adjacencyList: _mapService._graph,
                start: driver.CurrentPosition.Name,
                goal: userLocation,
                totalCost: ref cost
            );
            Console.WriteLine($"Driver: {driver.Name} - Cose: {cost}");
            if (route != null && cost < minCost)
            {
                minCost = cost;
                nearestDriver = driver;
                bestRoute = route;
            }
        }

        if (nearestDriver == null)
            throw new Exception("No drivers available for the specified location.");

        return (nearestDriver, minCost, bestRoute);
    }

    public (List<string> route, double cost) GetShortestRoute(string src, string dest)
    {
        if (string.IsNullOrEmpty(src) || string.IsNullOrEmpty(dest))
            throw new ArgumentException("Source and destination cannot be null or empty.");

        double cost = 0;
        var route = _shortestPath.Dijsktra(
            adjacencyList: _mapService._graph,
            start: src,
            goal: dest,
            totalCost: ref cost
        );

        if (route == null || !route.Any())
            throw new Exception("No route available between the specified locations.");

        return (route, cost);
    }
}
