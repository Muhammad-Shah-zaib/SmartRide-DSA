using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartRide.src.DataStructures;
using SmartRide.src.Dtos;
using SmartRide.Models;
namespace SmartRide.src.Services
{
    public class RideRequestsQueueService
    {
        private readonly SmartRideDbContext _dbContext;
        private readonly Queue<RideRequestDto> _rideRequests;
        public RideRequestsQueueService(SmartRideDbContext dbContext) { 
            _dbContext = dbContext;
            _rideRequests = new Queue<RideRequestDto>();
        }

        public void AddRideRequest(RideRequestDto request)
        {   
            _rideRequests.Enqueue(request);
        }
        public void RemoveRideRequest() {
            if(_rideRequests.Count > 0)
            {
                 var ride = _rideRequests.Dequeue();
                var completedRide = new RideRequest
                {
                    UserId = ride.UserId,
                    Source = ride.Source,
                    Destination = ride.Destination,
                    Status = true,
                    RideTime = ride.RideTime
                };
                
                // adding the ride into the DB
                _dbContext.CompletedRides.Add(completedRide);
                _dbContext.SaveChanges();

            }
        }
        public int GetPendingRequestsCount()
        {
            return _rideRequests.Count;
        }
    }
}











