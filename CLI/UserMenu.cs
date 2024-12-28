namespace SmartRide.CLI;

public class UserMenu(SmartRideDbContext context, UserDto currentUser)
{
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
        RideCli rideCli = new(context, CurrentUser);
        rideCli.Run();
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("Press any key to continue.");
        Console.ReadKey();
        Console.ResetColor();
    }
}
