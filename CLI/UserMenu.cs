namespace SmartRide.CLI;

public class UserMenu
{
    private readonly MapService _mapService;
    private readonly DriverService _driverService;

    // The current logged-in user
    public UserDto CurrentUser { get; set; }

    public UserMenu(SmartRideDbContext context, UserDto currentUser)
    {
        _mapService = new MapService(context);
        _driverService = new DriverService(context);
        CurrentUser = currentUser;
    }

    public void Run()
    {
        // Loop until user decides to log out
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Welcome to SmartRide!");

            // Check if the user is logged in
            if (CurrentUser != null)
            {
                Console.WriteLine($"Hello, {CurrentUser.Name}! Ready for your next adventure?");
            }
            else
            {
                Console.WriteLine("Please log in. Press any key to continue.");
                Console.ReadLine();
                return; // Exit the menu if no user is logged in
            }

            // Main menu with options
            Console.WriteLine("\nWhat’s your choice today? Let’s make your journey unforgettable!");
            Console.WriteLine("1. Take a Ride");
            Console.WriteLine("2. Go with Carpool Today");
            Console.WriteLine("3. Deliver/Receive a Package");
            Console.WriteLine("4. Log out");

            string choice = Console.ReadLine()?.Trim() ?? "";

            switch (choice)
            {
                case "1":
                    Console.WriteLine("You’re all set! Let’s get you on the road!");
                    this.Ride();
                    break;

                case "2":
                    // Call the method for Carpool Today
                    Console.WriteLine("Ready to share the ride? Let’s hit the road together!");
                    // Add logic for carpool here
                    break;

                case "3":
                    // Call the method for package delivery
                    Console.WriteLine("Package ready to be delivered? Let’s make it happen!");
                    // Add logic for package delivery here
                    break;

                case "4":
                    // Log out the user
                    Console.WriteLine("Logging out... Safe travels!");
                    CurrentUser = null; // Clear the current user session
                    return; // Exit the loop and logout the user

                default:
                    Console.WriteLine("Oops! That’s not a valid option. Let’s try again.");
                    break;
            }
        }
    }

    public void Ride()
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
            List<DriverDto> drivers = _driverService.GetAllDrivers(); // Fetch from database or mock data
            if (drivers.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("No drivers available right now. You will be added to the queue.");
                Console.ResetColor();
                return;
            }

            // Call RideRequestMatching to get the nearest driver and path
            var rideRequestMatching = new RideRequestMatching();
            var (driver, path, cost) = rideRequestMatching.RequestMatching(_mapService._graph, userSrc, userDest, drivers);

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
                Console.Write("Please give a rating (1-5): ");
                string rating = Console.ReadLine()?.Trim() ?? "0";
                Console.WriteLine($"You rated the driver {rating}/5. Thank you for your feedback!");
                // Placeholder for saving the rating
                // SaveDriverRating(driver, int.Parse(rating)); // Uncomment when implemented
            }

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
