using System.Collections.Concurrent;

namespace SmartRide.src.Services;

public class CarpoolService
{
    private readonly ConcurrentDictionary<int, CarpoolRideDto> _carpools; // Key: CarpoolId, Value: CarpoolRideDto
    private readonly PriorityQueue<CarpoolRideDto, int> _priorityQueue; // Priority based on proximity or other factors
    private readonly SmartRideDbContext _dbContext;

    public CarpoolService(SmartRideDbContext dbContext)
    {
        _dbContext = dbContext;
        _carpools = new ConcurrentDictionary<int, CarpoolRideDto>();
        _priorityQueue = new PriorityQueue<CarpoolRideDto, int>();

        LoadCarpoolsFromDb();
    }

    private void LoadCarpoolsFromDb()
    {
        var carpoolRecords = _dbContext.Carpoolrides.ToList();
        foreach (var record in carpoolRecords)
        {
            var carpoolRideDto = MapToDto(record);
            _carpools[carpoolRideDto.CarpoolId] = carpoolRideDto;
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

        _carpools[newCarpool.CarpoolId] = newCarpool;

        Console.WriteLine($"New carpool {newCarpool.CarpoolId} has been added.");
    }

    public (CarpoolRideDto carpool, List<string> route)? FindAndAssignNearestCarpool(string userSrc, Graph<string> graph)
    {
        if (!graph.ContainsNode(userSrc.ToUpper()))
        {
            Console.WriteLine("Invalid source location.");
            return null;
        }

        var userPriorityQueue = new PriorityQueue<(CarpoolRideDto carpool, List<string> route), double>();
        var shortestPathFinder = new ShortestPath<string>();

        foreach (var carpool in _carpools.Values.Where(c => c.Status == "ACTIVE"))
        {
            double totalCost = 0;
            try
            {
                var path = shortestPathFinder.Dijkstra(graph, userSrc.ToUpper(), carpool.Src.ToUpper(), ref totalCost);
                userPriorityQueue.Enqueue((carpool, path), totalCost);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Error calculating path for carpool {carpool.CarpoolId}: {ex.Message}");
            }
        }

        while (userPriorityQueue.Count > 0)
        {
            var (nearestCarpool, route) = userPriorityQueue.Dequeue();

            if (nearestCarpool.CurrentPassengers < nearestCarpool.MaxPassengers)
            {
                Console.WriteLine($"Nearest carpool found: Carpool {nearestCarpool.CarpoolId}. Do you want to join? (yes/no)");
                var response = Console.ReadLine()?.ToLower();

                if (response == "yes")
                {
                    nearestCarpool.CurrentPassengers++;
                    nearestCarpool.UpdatedAt = DateTime.Now;

                    if (nearestCarpool.CurrentPassengers == nearestCarpool.MaxPassengers)
                    {
                        nearestCarpool.Status = "FULL";
                    }

                    _carpools[nearestCarpool.CarpoolId] = nearestCarpool;
                    UpdateCarpoolInDb(MapToEntity(nearestCarpool));
                    return (nearestCarpool, route);
                }
            }
        }

        Console.WriteLine("No active carpools available.");
        return null;
    }

    public void EndCarpool(int carpoolId)
    {
        if (_carpools.TryGetValue(carpoolId, out var carpool))
        {
            carpool.Status = "COMPLETED";
            carpool.UpdatedAt = DateTime.Now;

            _carpools[carpoolId] = carpool;
            UpdateCarpoolInDb(MapToEntity(carpool));

            Console.WriteLine($"Carpool {carpoolId} has been marked as completed.");
        }
        else
        {
            Console.WriteLine($"Carpool with ID {carpoolId} not found.");
        }
    }

    public List<CarpoolRideDto> GetAllCarpools()
    {
        var allCarpools = _carpools.Values.ToList();

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
        UpdatedAt = dto.UpdatedAt,
        Src = dto.Src,
        Dest = dto.Dest
    };
}
