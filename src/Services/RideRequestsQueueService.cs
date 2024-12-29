namespace SmartRide.src.Services;

public class RideRequestsQueueService
{
    private readonly SmartRideDbContext _dbContext;
    private readonly Queue<RideRequestDto> _rideRequests;

    public RideRequestsQueueService(SmartRideDbContext dbContext)
    {
        _dbContext = dbContext;
        _rideRequests = new Queue<RideRequestDto>();
    }

    public void AddRideRequest(RideRequestDto request)
    {
        _rideRequests.Enqueue(request);
    }

    public void RemoveRideRequestIfInQueue( int userId )
    {
        if (_rideRequests.Count > 0)
        {
            // Peek at the first request in the queue
            var firstRequest = _rideRequests.Peek();

            // Check if it matches the user's request
            if (firstRequest.UserId == userId)
            {
                _rideRequests.Dequeue(); // Remove the request if it matches
            }
        }
    }

    public int GetPendingRequestsCount()
    {
        return _rideRequests.Count;
    }
}
