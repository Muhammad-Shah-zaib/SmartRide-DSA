﻿namespace SmartRide.src.Services;

public class CarpoolService
{
    private readonly HashMap<int, CarpoolRideDto> _carpools;
    private readonly SmartRideDbContext _dbContext;

    public CarpoolService(SmartRideDbContext dbContext)
    {
        _dbContext = dbContext;
        _carpools = new HashMap<int, CarpoolRideDto>();

        LoadCarpoolsFromDb();
    }

    private void LoadCarpoolsFromDb()
    {
        var carpoolRecords = _dbContext.Carpoolrides.ToList();
        foreach (var record in carpoolRecords)
        {
            var carpoolRideDto = MapToDto(record);
            _carpools.Put(carpoolRideDto.CarpoolId, carpoolRideDto);
        }
    }

    public void AddNewCarpool(CarpoolRideDto newCarpool)
    {
        if (_carpools.ContainsKey(newCarpool.CarpoolId))
        {
            Console.WriteLine($"Carpool {newCarpool.CarpoolId} already exists.");
            return;
        }

        var newRecordPool = MapToEntity(newCarpool);
        _dbContext.Carpoolrides.Add(newRecordPool);
        _dbContext.SaveChanges();
        newCarpool.CarpoolId = newRecordPool.CarpoolId;

        _carpools.Put(newCarpool.CarpoolId, newCarpool);

        Console.WriteLine($"New carpool {newCarpool.CarpoolId} has been added.");
    }

    public (CarpoolRideDto carpool, List<string> route)? FindAndAssignNearestCarpool(string userSrc, Graph<string> graph)
    {
        if (!graph.ContainsNode(userSrc.ToUpper()))
        {
            Console.WriteLine("Invalid source location.");
            return null;
        }

        var userPriorityQueue = new PriorityQueue<(CarpoolRideDto carpool, List<string> route, double totalCost), double>();
        var shortestPathFinder = new ShortestPath<string>();

        foreach (var carpool in _carpools.Values().Where(c => c.Status == "ACTIVE"))
        {
            double totalCost = 0;
            try
            {
                if (carpool.Status != "ACTIVE") continue;

                try
                {
                    var path = shortestPathFinder.Dijkstra(graph, userSrc.ToUpper(), carpool.Src.ToUpper(), ref totalCost);

                    // Enqueue the tuple with the carpool, path (route), and totalCost
                    userPriorityQueue.Enqueue((carpool, path, totalCost), totalCost);
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Error calculating path for carpool: {ex.Message}");
                }
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error calculating path for carpool {carpool.CarpoolId}: {ex.Message}");
            }
        }

        while (userPriorityQueue.Count > 0)
        {
            var (nearestCarpool, route, totalCost) = userPriorityQueue.Dequeue();

            if (nearestCarpool.CurrentPassengers < nearestCarpool.MaxPassengers)
            {
                // Calculate estimated time
                var estimatedTime = 1.45 * totalCost;

                // Calculate cost per passenger
                var costPerPassenger = totalCost / (nearestCarpool.CurrentPassengers + 1);

                // Display carpool details
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("-------------------");
                Console.WriteLine($"\nNearest Carpool Found: Carpool ID {nearestCarpool.CarpoolId}");
                Console.WriteLine($"Route: {nearestCarpool.Route}");
                Console.WriteLine($"Total Cost: {totalCost:C2}");
                Console.WriteLine($"Estimated Time: {estimatedTime:F2} minutes");
                Console.WriteLine($"Current Passengers: {nearestCarpool.CurrentPassengers}/{nearestCarpool.MaxPassengers}");
                Console.WriteLine($"Your Cost: {costPerPassenger:C2}");
                Console.WriteLine("-------------------");
                Console.ResetColor();

                // Update carpool details
                nearestCarpool.CurrentPassengers++;
                nearestCarpool.UpdatedAt = DateTime.Now;

                if (nearestCarpool.CurrentPassengers == nearestCarpool.MaxPassengers)
                {
                    nearestCarpool.Status = "FULL";
                }

                _carpools.Put(nearestCarpool.CarpoolId, nearestCarpool);
                UpdateCarpoolInDb(MapToEntity(nearestCarpool));

                Console.WriteLine($"\nYou have successfully joined Carpool {nearestCarpool.CarpoolId}.");
                Console.WriteLine($"Your share of the cost is {costPerPassenger:C2}.");
                return (nearestCarpool, route);
            }
        }

        Console.WriteLine("No active carpools available.");
        return null;
    }

    public void EndCarpool(int carpoolId)
    {
        if (_carpools.ContainsKey(carpoolId))
        {
            var carpool = _carpools.Get(carpoolId);
            carpool.Status = "COMPLETED";
            carpool.UpdatedAt = DateTime.Now;

            _carpools.Put(carpoolId, carpool);
            UpdateCarpoolInDb(MapToEntity(carpool));

            Console.WriteLine($"Carpool {carpoolId} has been marked as completed.");
        }
        else
        {
            Console.WriteLine($"Carpool with ID {carpoolId} not found.");
        }
    }

    public List<string> GetAvailableDestinations()
    {
        var destinations = _carpools.Values()
            .Where(c => c.Status == "ACTIVE" && c.CurrentPassengers < c.MaxPassengers)
            .Select(c => c.Dest)
            .Distinct()
            .ToList();

        return destinations;
    }

    public List<CarpoolRideDto> GetAllCarpools()
    {
        var allCarpools = _carpools.Values().ToList();

        foreach (var carpool in allCarpools)
        {
            Console.WriteLine($"Carpool ID: {carpool.CarpoolId}, Driver ID: {carpool.DriverId}, Route: {carpool.Route}, " +
                              $"Current Passengers: {carpool.CurrentPassengers}/{carpool.MaxPassengers}, Status: {carpool.Status}" +
                              $"Src: {carpool.Src}, Dest: {carpool.Dest}");
        }

        return allCarpools;
    }

    private void UpdateCarpoolInDb(Carpoolride carpool)
    {
        var carpoolRecord = _dbContext.Carpoolrides.FirstOrDefault(c => c.CarpoolId == carpool.CarpoolId);
        if (carpoolRecord != null)
        {
            carpoolRecord.Status = carpool.Status;
            carpoolRecord.CurrentPassengers = carpool.CurrentPassengers;
            carpoolRecord.UpdatedAt = DateTime.Now;
            _dbContext.SaveChanges();
        }
    }

    private CarpoolRideDto MapToDto(Carpoolride entity) => new CarpoolRideDto
    {
        CarpoolId = entity.CarpoolId,
        DriverId = entity.DriverId,
        Route = entity.Route,
        MaxPassengers = entity.MaxPassengers,
        CurrentPassengers = (int)entity.CurrentPassengers,
        Status = entity.Status,
        TotalCost = entity.Totalcost,
        CreatedAt = (DateTime)entity.CreatedAt,
        UpdatedAt = (DateTime)entity.UpdatedAt,
        Src = entity.Src,
        Dest = entity.Dest
    };

    private Carpoolride MapToEntity(CarpoolRideDto dto) => new Carpoolride
    {
        DriverId = dto.DriverId,
        Route = dto.Route,
        MaxPassengers = dto.MaxPassengers,
        CurrentPassengers = dto.CurrentPassengers,
        Status = dto.Status,
        CreatedAt = dto.CreatedAt,
        Totalcost = dto.TotalCost,
        UpdatedAt = dto.UpdatedAt,
        Src = dto.Src,
        Dest = dto.Dest
    };
}
