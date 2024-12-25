using System.Collections.Concurrent;

namespace SmartRide.src.Services
{
    /// <summary>
    /// Provides carpool-related operations such as finding, adding, updating, and ending carpools.
    /// Manages in-memory and database synchronization of carpool data.
    /// </summary>
    public class CarpoolService
    {
        private readonly ConcurrentDictionary<int, CarpoolRideDto> _carpools; // Key: CarpoolId, Value: CarpoolRideDto
        private readonly PriorityQueue<CarpoolRideDto, int> _priorityQueue; // Priority based on proximity or other factors
        private readonly SmartRideDbContext _dbContext;

        /// <summary>
        /// Constructor initializes the carpool service with data from the database.
        /// </summary>
        /// <param name="dbContext">Database context for accessing carpool data.</param>
        public CarpoolService(SmartRideDbContext dbContext)
        {
            _dbContext = dbContext;
            _carpools = new ConcurrentDictionary<int, CarpoolRideDto>();
            _priorityQueue = new PriorityQueue<CarpoolRideDto, int>();

            LoadCarpoolsFromDb();
        }

        /// <summary>
        /// Loads all carpool records from the database into an in-memory dictionary.
        /// This allows for fast, thread-safe access to carpool data.
        /// </summary>
        private void LoadCarpoolsFromDb()
        {
            var carpoolRecords = _dbContext.Carpoolrides.ToList();
            foreach (var record in carpoolRecords)
            {
                var carpoolRideDto = MapToDto(record);
                _carpools[carpoolRideDto.CarpoolId] = carpoolRideDto;
            }
        }

        /// <summary>
        /// Adds a new carpool to the system. Updates both the in-memory collection and the database.
        /// </summary>
        /// <param name="newCarpool">Details of the carpool to be added.</param>
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
            // updating the newly assigned id to the carpool by the db
            newCarpool.CarpoolId = newRecordPool.CarpoolId;

            // adding carpool on the hashmap
            _carpools[newCarpool.CarpoolId] = newCarpool;

            Console.WriteLine($"New carpool {newCarpool.CarpoolId} has been added.");
        }

        /// <summary>
        /// Finds the nearest available carpool for a user based on their source location and assigns it.
        /// Returns both the selected carpool and the route taken to reach it.
        /// </summary>
        /// <param name="userSrc">Source location of the user.</param>
        /// <param name="graph">Graph representation of locations and paths.</param>
        /// <returns>Tuple containing the carpool details and the route, or null if no carpool is found.</returns>
        public (CarpoolRideDto? carpool, List<string> route)? FindAndAssignNearestCarpool(string userSrc, Graph<string> graph)
        {
            if (!graph.ContainsNode(userSrc.ToUpper()))
            {
                Console.WriteLine("Invalid source location.");
                return null;
            }

            var userPriorityQueue = new PriorityQueue<(CarpoolRideDto carpool, List<string> route), double>();
            var shortestPathFinder = new ShortestPath<string>();

            foreach (var carpool in _carpools.Values)
            {
                if (carpool.Status != "ACTIVE") continue;

                double totalCost = 0;
                try
                {
                    var path = shortestPathFinder.Dijsktra(graph, userSrc.ToUpper(), carpool.Src.ToUpper(), ref totalCost);
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

                if (nearestCarpool.Status == "ACTIVE" && nearestCarpool.CurrentPassengers < nearestCarpool.MaxPassengers)
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

        /// <summary>
        /// Marks a carpool as completed and updates its status in both memory and the database.
        /// </summary>
        /// <param name="carpoolId">The ID of the carpool to be ended.</param>
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

        /// <summary>
        /// Retrieves all carpools from the in-memory collection (hashmap).
        /// </summary>
        /// <returns>A list of all carpool details in the hashmap.</returns>
        public List<CarpoolRideDto> GetAllCarpools()
        {
            // Get all carpools from the _carpools hashmap
            var allCarpools = _carpools.Values.ToList();

            // Optionally print the details to the console (you can customize this as needed)
            foreach (var carpool in allCarpools)
            {
                Console.WriteLine($"Carpool ID: {carpool.CarpoolId}, Driver ID: {carpool.DriverId}, Route: {carpool.Route}, " +
                                  $"Current Passengers: {carpool.CurrentPassengers}/{carpool.MaxPassengers}, Status: {carpool.Status}" +
                                  $"Src: {carpool.Src}, Dest: {carpool.Dest}");
            }

            return allCarpools;
        }


        /// <summary>
        /// Updates the status of a carpool in the database.
        /// </summary>
        /// <param name="carpool">Carpool entity with updated details.</param>
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

        /// <summary>
        /// Maps a CarpoolRide entity from the database to a DTO object for in-memory operations.
        /// </summary>
        /// <param name="entity">CarpoolRide entity from the database.</param>
        /// <returns>Corresponding CarpoolRideDto object.</returns>
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

        /// <summary>
        /// Maps a CarpoolRideDto object to a CarpoolRide entity for database operations.
        /// </summary>
        /// <param name="dto">CarpoolRideDto object.</param>
        /// <returns>Corresponding CarpoolRide entity.</returns>
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
}
