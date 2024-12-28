using Microsoft.EntityFrameworkCore;

namespace SmartRide.CLI;

public class RideCli(SmartRideDbContext context, UserDto currentUser)
{
    private readonly SmartRideDbContext _dbContext = context;
    private readonly MapService _mapService = new MapService(context);
    private readonly DriverService _driverService = new DriverService(context);
    private readonly DriverRatingService _driverRatingService = new DriverRatingService(context);
    private readonly RideRequestsQueueService _rideRequestsQueueService = new RideRequestsQueueService(context);
    public UserDto CurrentUser { get; set; } = currentUser;

    public void Run()
    {
        Console.Clear();
        Console.WriteLine("Let’s get you a ride!");

        // Ask user for source and destination
        Console.Write("Enter your current location (source): ");
        string userSrc = Console.ReadLine()?.Trim().ToUpper() ?? string.Empty;

        Console.Write("Enter your destination: ");
        string userDest = Console.ReadLine()?.Trim().ToUpper() ?? string.Empty;

        if (string.IsNullOrEmpty(userSrc) || string.IsNullOrEmpty(userDest))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Source and destination cannot be empty. Please try again.");
            Console.ResetColor();
            return;
        }

        try
        {
            // Simulate the graph and driver data (these should ideally come from the system)
            List<DriverDto> drivers = _driverService.GetAllAvailableDrivers(); // Fetch from database or mock data
            
            // Call RideRequestMatching to get the nearest driver and path
            var rideRequestMatching = new RideRequestMatching();
            var (driver, path, cost) = rideRequestMatching.RequestMatching(_mapService._graph, userSrc, userDest, drivers);

            if (drivers.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("No drivers available right now. You will be added to the queue.");
                // Add ride request to the queue
                _rideRequestsQueueService.AddRideRequest(new RideRequestDto
                {
                    UserId = CurrentUser.Id,
                    Source = userSrc,
                    Destination = userDest,
                    RideTime = DateTime.Now
                });
                Console.WriteLine("You will be notified once a driver becomes available.");
                Console.ResetColor();
                return;
            }

            
            // Display ride details
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nRide Details:");
            Console.WriteLine($"Driver: {driver.Name}");
            Console.WriteLine($"Driver Location: {driver.CurrentPosition.Name}");
            Console.WriteLine($"Path: {string.Join(" -> ", path)}");
            Console.WriteLine($"Total Cost: {cost:C}");
            Console.ResetColor();

            // Ask user to confirm ride
            Console.Write("\nDo you want to proceed with this ride? (yes/no): ");
            string confirmation = Console.ReadLine()?.Trim().ToLower() ?? "no";

            if (confirmation != "yes")
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Ride cancelled.");
                Console.ResetColor();
                return;
            }

            // Simulate reaching destination
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nPress any key once you have reached your destination...");
            Console.ReadKey();
            Console.ResetColor();

            // Ask for driver rating
            Console.Write("\nThank you for riding with us! Would you like to rate your driver? (yes/no): ");
            string rateConfirmation = Console.ReadLine()?.Trim().ToLower() ?? "no";

            if (rateConfirmation == "yes")
            {
                double rating = 0;
                string comment = string.Empty;

                // Keep asking for a valid rating until the user provides one
                while (rating < 1 || rating > 5)
                {
                    Console.Write("\nPlease give a rating between 1 and 5 (1 being terrible, 5 being excellent): ");
                    string ratingInput = Console.ReadLine()?.Trim() ?? "1";

                    // Try to parse the rating input
                    bool isValidRating = double.TryParse(ratingInput, out rating);

                    // If the input is not valid or outside the valid range, ask again
                    if (!isValidRating || rating < 1 || rating > 5)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Oops! That's not a valid rating. Please enter a number between 1 and 5.");
                        Console.ResetColor();
                    }
                    else
                    {
                        break; // Valid rating, break out of the loop
                    }
                }

                // Ask for a comment
                Console.Write("\nPlease provide a comment for the driver (optional): ");
                comment = Console.ReadLine()?.Trim() ?? "";

                try
                {
                    // Add the rating using DriverRatingService
                    _driverRatingService.AddRating(driver.Id, rating, comment);

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"You rated the driver {rating}/5. Thank you for your feedback!");

                    // Cheerful message after giving feedback
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("\nWe appreciate your feedback! Your opinion makes us better.");
                    Console.ResetColor(); // reset console color
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Something went wrong while submitting your rating. Please try again later.\nError: {ex.Message}");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("You chose not to rate the driver. Thank you for your ride!");
                Console.ResetColor();
            }

            // Create and add the completed ride to the Completedrides table
            var completedRide = new Completedride
            {
                Userid = CurrentUser.Id,
                Source = userSrc,
                Destination = userDest,
                Status = "completed",
                Driverid = driver.Id,
                Ridetime = DateTime.Now
            };

            _dbContext.Completedrides.Add(completedRide);
            _dbContext.SaveChanges();

            // Remove the ride from the queue
            _rideRequestsQueueService.RemoveRideRequestIfInQueue(CurrentUser.Id);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Ride completed successfully! Have a great day!");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error: {ex.Message}");
            Console.ResetColor();
        }
    }
}
