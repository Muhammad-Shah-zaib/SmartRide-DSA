namespace SmartRide.src.Utilities.Carpool;

public class Carpool
{

    public static void InitializeCarpool(SmartRideDbContext context)
    {
        MapService mapService = new(context);

        using var dbContext = new SmartRideDbContext();
        // Initialize the CarpoolService
        var carpoolService = new CarpoolService(dbContext);

        // get routes
        var shortestPath = new ShortestPath<string>();

        var totalCost1 = 0.0;
        var totalCost2 = 0.0;
        var path1 = shortestPath.Dijkstra(mapService._graph, "BOLAN CHOWK", "SNAKESIAN", ref totalCost1);
        var path2 = shortestPath.Dijkstra(mapService._graph, "IQBAL CIRCLE", "SNAKESIAN", ref totalCost2);
        // Create sample carpools
        var newCarpools = new List<CarpoolRideDto>()
            {
                new CarpoolRideDto {
                    DriverId = 5,
                    Route = string.Join("->", path1),
                    MaxPassengers = 4,
                    CurrentPassengers = 1,
                    Status = "ACTIVE",
                    Src = "BOLAN CHOWK",
                    TotalCost= totalCost1,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new CarpoolRideDto
                {
                    DriverId = 6,
                    Route = string.Join("->", path2),
                    MaxPassengers = 4,
                    CurrentPassengers = 2,
                    Status = "ACTIVE",
                    TotalCost= totalCost2,
                    Src  = "IQBAL CHOWK",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
            };

        // Add each carpool using the CarpoolService
        foreach (var carpool in newCarpools)
        {
            carpoolService.AddNewCarpool(carpool);
        }
    }
}
