namespace SmartRide.src.Services
{
    public class UserRideHistoryService
    {
        private readonly SmartRideDbContext _dbContext;
        private readonly MapService _mapService;
        private readonly HashMap<int, DoublyLinkedList<UserRideHistoryDto>> _rideHistoryMap;

        public UserRideHistoryService(SmartRideDbContext dbContext)
        {
            _dbContext = dbContext;
            _rideHistoryMap = new HashMap<int, DoublyLinkedList<UserRideHistoryDto>>(100);
            _mapService = new(dbContext);

            // Load all user rides into the HashMap on initialization
            LoadRidesFromHistory();
        }

        // Loads all rides from the database into the HashMap
        private void LoadRidesFromHistory()
        {
            _rideHistoryMap.Clear();
            var rides = _dbContext.Completedrides.ToList();
            foreach (var ride in rides)
            {
                var rideDto = new UserRideHistoryDto
                {
                    Id = ride.Id,
                    UserId = ride.Userid,
                    RideStartLocation = ride.Source,
                    RideEndLocation = ride.Destination,
                    RideDate = ride.Ridetime,
                };
                ShortestPath<string> shortestPath = new();
                var totalCost = 0.0;
                shortestPath.Dijkstra(_mapService._graph, rideDto.RideStartLocation, rideDto.RideEndLocation, ref totalCost);

                rideDto.RideDistance = (decimal)totalCost;
                
                if (!_rideHistoryMap.ContainsKey(ride.Userid))
                {
                    _rideHistoryMap.Put(ride.Userid, new DoublyLinkedList<UserRideHistoryDto>());
                }

                _rideHistoryMap.Get(ride.Userid)!.AddLast(rideDto);
            }
        }

        // Adds a new ride to the database and the HashMap
        public void AddRide(UserRideHistoryDto rideDto)
        {
            var ride = new Userridehistory
            {
                Userid = rideDto.UserId,
                Ridestartlocation = rideDto.RideStartLocation,
                Rideendlocation = rideDto.RideEndLocation,
                Ridedistance = rideDto.RideDistance,
                Rideduration = rideDto.RideDuration,
                Ridedate = rideDto.RideDate,
                Ridecost = rideDto.RideCost
            };

            _dbContext.Userridehistories.Add(ride);
            _dbContext.SaveChanges();

            // Update the DTO with the new ID
            rideDto.Id = ride.Id;

            // Add to the HashMap
            if (!_rideHistoryMap.ContainsKey(ride.Userid))
            {
                _rideHistoryMap.Put(ride.Userid, new DoublyLinkedList<UserRideHistoryDto>());
            }

            _rideHistoryMap.Get(ride.Userid)!.AddLast(rideDto);
        }

        // Gets all rides for a specific user
        public IEnumerable<UserRideHistoryDto> GetUserRideHistory(int userId)
        {
            LoadRidesFromHistory();
            if (!_rideHistoryMap.ContainsKey(userId))
                throw new KeyNotFoundException($"No ride history found for user ID {userId}.");

            return _rideHistoryMap.Get(userId)!.ToList();
        }

        // Removes a ride from the database and the HashMap
        public void RemoveRide(int rideId)
        {
            var ride = _dbContext.Userridehistories.FirstOrDefault(r => r.Id == rideId);
            if (ride == null) throw new Exception($"Ride ID {rideId} not found!");

            // Remove from database
            _dbContext.Userridehistories.Remove(ride);
            _dbContext.SaveChanges();

            // Remove from HashMap
            if (_rideHistoryMap.ContainsKey(ride.Userid))
            {
                var rideList = _rideHistoryMap.Get(ride.Userid)!;
                var rideToRemove = rideList.ToList().FirstOrDefault(r => r.Id == rideId);
                if (rideToRemove != null)
                {
                    rideList.Remove(rideToRemove);

                    // Remove the key from HashMap if no rides remain for the user
                    if (rideList.Count == 0)
                    {
                        _rideHistoryMap.Remove(ride.Userid);
                    }
                }
            }
        }
    }
}
