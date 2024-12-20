global using SmartRide.Models;
global using SmartRide.src.Dtos;
global using SmartRide.src.Services;
using Microsoft.EntityFrameworkCore;
using System;

class Program
{
    static void Main(string[] args)
    {
        // Mock DbContext (replace with actual DbContext implementation)
        var dbContext = new SmartRideDbContext();

        // Initialize the RideRequestsQueueService
        var rideRequestService = new RideRequestsQueueService(dbContext);


        // Add some ride requests to the queue
        var rideRequest1 = new RideRequestDto
        {
            UserId = 1,
            Source = "Downtown",
            Destination = "Uptown",
            Status = "Pending",
            RideTime = DateTime.Now
        };

        var rideRequest2 = new RideRequestDto
        {
            UserId = 2,
            Source = "Midtown",
            Destination = "Airport",
            Status = "Pending",
            RideTime = DateTime.Now.AddMinutes(15)
        };

        rideRequestService.AddRideRequest(rideRequest1);
        rideRequestService.AddRideRequest(rideRequest2);

        Console.WriteLine("Ride requests added to the queue.");

        // Process and remove a ride request, then save to completed rides
        rideRequestService.RemoveRideRequest();
        Console.WriteLine("A ride request processed and saved to completed rides.");

        // Retrieve and display completed rides
        Console.WriteLine("Completed Rides:");
        var completedRides = dbContext.CompletedRides.ToList();
        foreach (var completedRide in completedRides)
        {
            Console.WriteLine($"ID: {completedRide.Id}, UserId: {completedRide.UserId}, From: {completedRide.Source}, To: {completedRide.Destination}, Status: {completedRide.Status}, RideTime: {completedRide.RideTime}");
        }

        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}
