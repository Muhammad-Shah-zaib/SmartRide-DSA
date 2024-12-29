namespace SmartRide.src.Services
{
    public class RideRequestMatching
    {
        public (DriverDto driver, List<string> path, double totalCost) RequestMatching(Graph<string> map, string userSrc, string userDest, List<DriverDto> drivers)
        {
            if (drivers == null || drivers.Count == 0)
                throw new InvalidOperationException("No drivers available.");

            var shortestPath = new ShortestPath<string>();
            var nearestDrivers = new PriorityQueues<DriverDto>();
            double driverToUserCost = 0.0;

            // Calculate distance from each driver to the user source
            foreach (var driver in drivers)
            {
                try
                {
                    var pathToUser = shortestPath.Dijkstra(map, driver.CurrentPosition.Name, userSrc, ref driverToUserCost);
                    if (pathToUser.Count > 0)
                    {
                        // Add driver to the priority queue
                        nearestDrivers.Enqueue(driver, driverToUserCost);
                    }
                }
                catch (Exception ex)
                {
                    // Log or handle unreachable drivers (e.g., disconnected graph)
                    Console.WriteLine($"Driver {driver.Name} is unreachable. Reason: {ex.Message}");
                }
            }

            // Select the nearest driver
            if (nearestDrivers.Count == 0)
                throw new InvalidOperationException("No drivers can reach your location.");

            var (nearestDriver, _) = nearestDrivers.Dequeue();

            // Calculate the shortest path and total cost from user source to destination
            double userTripCost = 0.0;
            var userTripPath = shortestPath.Dijkstra(map, userSrc, userDest, ref userTripCost);

            if (userTripPath == null || userTripPath.Count == 0)
                throw new InvalidOperationException("No path exists between the source and destination.");

            return (nearestDriver, userTripPath, userTripCost);
        }
    }
}
