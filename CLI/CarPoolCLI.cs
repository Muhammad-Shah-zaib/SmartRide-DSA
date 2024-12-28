using SmartRide.src.Services;

namespace SmartRide.CLI;

public class CarPoolCLI(SmartRideDbContext context, UserDto currentUser)
{
    private readonly CarpoolService _carpoolService = new CarpoolService(context);
    private readonly MapService _mapService = new MapService(context);
    private readonly DriverRatingService _driverRatingService = new DriverRatingService(context);
    private UserDto CurrentUser { get; set; } = currentUser;

    public void Run()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"Welcome {CurrentUser.Name} to SmartRide Carpool CLI!");
        Console.ResetColor();

        // Step 1: Display available destinations
        var destinations = _carpoolService.GetAvailableDestinations();
        if (destinations.Count == 0)
        {
            Console.WriteLine("Currently, there are no available destinations. Please check back later.");
            return;
        }

        // Step 2: Ask user for source and destination
        Console.WriteLine("\nEnter your current location (Source):");
        var userSrc = Console.ReadLine()?.Trim();

        if (string.IsNullOrEmpty(userSrc))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid source location. Please restart the application.");
            Console.ResetColor();
            return;
        }

        Console.WriteLine("\nAvailable destinations:");
        foreach (var dest in destinations)
        {
            Console.WriteLine($"- {dest}");
        }

        Console.WriteLine("\nEnter your desired destination:");
        var userDest = Console.ReadLine()?.Trim();

        if (string.IsNullOrEmpty(userDest) || !destinations.Contains(userDest, StringComparer.OrdinalIgnoreCase))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid or unavailable destination.");
            Console.ResetColor();
            return;
        }

        // Step 3: Find and assign the nearest carpool
        var result = _carpoolService.FindAndAssignNearestCarpool(userSrc, _mapService._graph);
        if (result != null)
        {
            var (carpool, route) = result.Value;

            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.WriteLine($"\nYour Route to {carpool.Src} => {string.Join(" -> ", route)}");
            Console.ResetColor();

            Console.WriteLine ($"\nYour Route to {carpool.Src} => {string.Join(" -> ", route)}");
            Console.ResetColor();

            Console.WriteLine("\nDo you want to join this carpool? (yes/no):");
            var response = Console.ReadLine()?.Trim().ToLower();

            if (response == "yes")
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\nYou have successfully joined Carpool {carpool.CarpoolId}.");
                Console.ResetColor();

                // Simulate reaching destination
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nPress any key once you have reached your destination...");
                Console.ReadKey();
                Console.ResetColor();

                Console.WriteLine($"You have reached your destination using Carpool {carpool.CarpoolId}.");

                // Ask for driver rating
                Console.Write("\nThank you for riding with us! Would you like to rate your driver? (yes/no): ");
                var rateConfirmation = Console.ReadLine()?.Trim().ToLower() ?? "no";

                if (rateConfirmation == "yes")
                {
                    RateDriver(carpool.DriverId);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("You chose not to rate the driver. Thank you for riding with us!");
                    Console.ResetColor();
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("You chose not to join the carpool.");
                Console.ResetColor();
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No suitable carpools found at the moment. Please try again later.");
            Console.ResetColor();
        }
    }

    private void RateDriver(int driverId)
    {
        double rating = 0;

        // Prompt for a valid rating
        while (rating < 1 || rating > 5)
        {
            Console.Write("\nPlease provide a rating for the driver (1-5): ");
            var ratingInput = Console.ReadLine()?.Trim();

            if (!double.TryParse(ratingInput, out rating) || rating < 1 || rating > 5)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid rating. Please enter a number between 1 and 5.");
                Console.ResetColor();
            }
        }

        Console.Write("\nWould you like to add a comment for the driver? (optional): ");
        var comment = Console.ReadLine()?.Trim() ?? "";

        try
        {
            _driverRatingService.AddRating(driverId, rating, comment);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"You rated the driver {rating}/5. Thank you for your feedback!");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Error while submitting your rating: {ex.Message}");
            Console.ResetColor();
        }
    }
}
