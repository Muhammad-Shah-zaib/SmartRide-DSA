using SmartRide.src.DataStructures;

namespace SmartRide.src.Services;

public class RideHistoryService
{
    private readonly SmartRideDbContext _dbContext;
    private readonly HashMap<int, DoublyLinkedList<RideHistoryDto>> _rideHistoryMap;

    public RideHistoryService(SmartRideDbContext dbContext)
    {
        _dbContext = dbContext;
        _rideHistoryMap = new HashMap<int, DoublyLinkedList<RideHistoryDto>>(100);

        // Load ride history from the database into the HashMap
        LoadRideHistoryFromDb();
    }

    private void LoadRideHistoryFromDb()
    {
        var rides = _dbContext.Ridehistories.ToList();
        foreach (var ride in rides)
        {
            var rideDto = new RideHistoryDto()
            {
                Id = ride.Id,
                UserId = ride.Userid,
                Source = ride.Source,
                Destination = ride.Destination,
                RideDate = ride.Ridedate,
                Log = ride.Log ?? ""
            };

            if (!_rideHistoryMap.ContainsKey(ride.Userid))
                _rideHistoryMap.Put(ride.Userid, new DoublyLinkedList<RideHistoryDto>());

            // Add the ride to the user's doubly linked list
            _rideHistoryMap.Get(ride.Userid)!.AddLast(rideDto);
        }
    }

    public void AddRide(RideHistoryDto ride)
    {
        var newRide = new Ridehistory()
        {
            Userid = ride.UserId,
            Source = ride.Source,
            Destination = ride.Destination,
            Ridedate = ride.RideDate,
            Log = ride.Log
        };

        _dbContext.Ridehistories.Add(newRide);
        _dbContext.SaveChanges();

        // Assign the generated ID to the DTO
        ride.Id = newRide.Id;

        if (!_rideHistoryMap.ContainsKey(ride.UserId))
            _rideHistoryMap.Put(ride.UserId, new DoublyLinkedList<RideHistoryDto>());

        _rideHistoryMap.Get(ride.UserId)!.AddLast(ride);

        Console.WriteLine($"Ride added for User ID: {ride.UserId}, Ride ID: {ride.Id}");
    }

    public IEnumerable<RideHistoryDto> GetUserRideHistory(int userId)
    {
        if (!_rideHistoryMap.ContainsKey(userId))
            throw new KeyNotFoundException("No ride history found for the given user.");

        return _rideHistoryMap.Get(userId)!.ToList();
    }

    public void RemoveRide(int rideId)
    {
        var ride = _dbContext.Ridehistories.FirstOrDefault(r => r.Id == rideId)
            ?? throw new Exception("Ride not found.");
        _dbContext.Ridehistories.Remove(ride);
        _dbContext.SaveChanges();

        if (_rideHistoryMap.ContainsKey(ride.Userid))
        {
            var rideList = _rideHistoryMap.Get(ride.Userid)!;
            var rideToRemove = rideList.ToList().FirstOrDefault(r => r.Id == rideId);
            if (rideToRemove != null)
                rideList.Remove(rideToRemove);

            if (rideList.Count == 0)
                _rideHistoryMap.Remove(ride.Userid);
        }

        Console.WriteLine($"Ride ID: {rideId} removed successfully.");
    }
}
