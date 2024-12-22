global using SmartRide.Models;
global using SmartRide.src.Dtos;
global using SmartRide.src.DataStructures;
global using SmartRide.src.Services;
global using Microsoft.EntityFrameworkCore;

class Program
{
    static void Main(string[] args)
    {
        // Step 1: Initialize the DbContext
        var dbContext = new SmartRideDbContext();

        // Step 2: Initialize the UserRideHistoryService
        var rideHistoryService = new UserRideHistoryService(dbContext);

        // Step 3: Add test rides to the database and HashMap
        Console.WriteLine("Adding new rides...");
        var ride1 = new UserRideHistoryDto
        {
            UserId = 1,
            RideStartLocation = "City Center",
            RideEndLocation = "Airport",
            RideDistance = 15.2m,
            RideDuration = TimeSpan.FromMinutes(20),
            RideDate = DateTime.Now,
            RideCost = 25.0m
        };
        rideHistoryService.AddRide(ride1);

        var ride2 = new UserRideHistoryDto
        {
            UserId = 1,
            RideStartLocation = "City Center",
            RideEndLocation = "University",
            RideDistance = 8.5m,
            RideDuration = TimeSpan.FromMinutes(10),
            RideDate = DateTime.Now.AddMinutes(-30),
            RideCost = 10.0m
        };
        rideHistoryService.AddRide(ride2);

        var ride3 = new UserRideHistoryDto
        {
            UserId = 2,
            RideStartLocation = "Mall",
            RideEndLocation = "Home",
            RideDistance = 5.0m,
            RideDuration = TimeSpan.FromMinutes(8),
            RideDate = DateTime.Now.AddHours(-1),
            RideCost = 7.5m
        };
        rideHistoryService.AddRide(ride3);

        Console.WriteLine("Rides added successfully!");

        // Step 4: Retrieve and display all rides for a specific user
        Console.WriteLine("\nDisplaying rides for User ID 1:");
        var user1Rides = rideHistoryService.GetUserRideHistory(1);
        foreach (var ride in user1Rides)
        {
            Console.WriteLine($"From {ride.RideStartLocation} to {ride.RideEndLocation}, Cost: {ride.RideCost} USD");
        }

        // Step 5: Remove a ride and display updated history
        Console.WriteLine("\nRemoving the first ride for User ID 1...");
        rideHistoryService.RemoveRide(ride1.Id);

        Console.WriteLine("\nUpdated rides for User ID 1:");
        user1Rides = rideHistoryService.GetUserRideHistory(1);
        foreach (var ride in user1Rides)
        {
            Console.WriteLine($"From {ride.RideStartLocation} to {ride.RideEndLocation}, Cost: {ride.RideCost} USD");
        }

        // Step 6: Display rides for another user (User ID 2)
        Console.WriteLine("\nDisplaying rides for User ID 2:");
        var user2Rides = rideHistoryService.GetUserRideHistory(2);
        foreach (var ride in user2Rides)
        {
            Console.WriteLine($"From {ride.RideStartLocation} to {ride.RideEndLocation}, Cost: {ride.RideCost} USD");
        }
    }
}
