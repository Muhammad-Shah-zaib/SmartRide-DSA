namespace SmartRide.CLI;

public class UserMenu(SmartRideDbContext context, UserDto currentUser)
{
    private readonly UserRideHistoryService _userRideHistoryService = new(context);

    // The current logged-in user
    public UserDto CurrentUser { get; set; } = currentUser;

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
                Console.WriteLine("Please log in");
                Prompt.PressKeyToContinue();
                return; // Exit the menu if no user is logged in
            }

            // Main menu with options
            Console.WriteLine("\nWhat’s your choice today? Let’s make your journey unforgettable!");
            Console.WriteLine("1. Take a Ride");
            Console.WriteLine("2. Go with Carpool Today");
            Console.WriteLine("3. Deliver/Receive a Package");
            Console.WriteLine("4. Show User Rides");
            Console.WriteLine("5. Log out");

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
                    this.CarPool();
                    break;

                case "3":
                    // Call the method for package delivery
                    this.PackageDelivery();
                    break;

                case "4":
                    // Show user rides
                    this.ShowUserRides();
                    break;

                case "5":
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
        RideCli rideCli = new(context, CurrentUser);
        rideCli.Run();
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("Press any key to continue.");
        Console.ReadKey();
        Console.ResetColor();
    }

    public void CarPool()
    {
        CarPoolCLI carPoolCLI = new(context, CurrentUser);
        carPoolCLI.Run();
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("Press any key to continue.");
        Console.ReadKey();
        Console.ResetColor();
    }

    public void PackageDelivery()
    {
        MapService mapService = new(context);
        PackageDeliveryCli packageDeliveryCli =
            new(
                context: context,
                currentUser: CurrentUser,
                map: mapService._graph
            );

        packageDeliveryCli.Run();
    }

    public void ShowUserRides()
    {
        Console.Clear();
        Console.WriteLine("Your Ride History:");

        try
        {
            var userRides = _userRideHistoryService.GetUserRideHistory(CurrentUser.Id);

            if (!userRides.Any())
            {
                Console.WriteLine("No rides found. Looks like it’s time to take your first ride!");
            }
            else
            {
                int rideCount = 1;
                foreach (var ride in userRides)
                {
                    Console.WriteLine($"\nRide {rideCount}:");
                    Console.WriteLine($"- Ride ID: {ride.Id}");
                    Console.WriteLine($"- Start Location: {ride.RideStartLocation}");
                    Console.WriteLine($"- End Location: {ride.RideEndLocation}");
                    Console.WriteLine($"- Distance: {ride.RideDistance} km");
                    Console.WriteLine($"- Duration: {ride.RideDuration} mins");
                    Console.WriteLine($"- Date: {ride.RideDate:MM/dd/yyyy}");
                    Console.WriteLine($"- Cost: {ride.RideCost:C}");
                    rideCount++;
                }
            }
        }
        catch (KeyNotFoundException e)
        {
            Console.WriteLine(e.Message);
        }

        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("\nPress any key to return to the main menu.");
        Console.ReadKey();
        Console.ResetColor();
    }

}
