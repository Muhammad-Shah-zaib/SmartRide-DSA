namespace SmartRide.CLI;

public class Main(SmartRideDbContext context)
{
    private readonly SmartRideDbContext _context = context;


    public void Run()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Welcome to SmartRide!");
            Console.WriteLine("1. Register as Driver");
            Console.WriteLine("2. Register as User");
            Console.WriteLine("3. Login as Driver");
            Console.WriteLine("4. Login as User");
            Console.WriteLine("5. Exit");
            Console.Write("Select an option: ");

            string choice = Console.ReadLine() ?? "5";

            // Handle the user's choice
            switch (choice)
            {
                case "1":
                    RegisterDriver();
                    break;
                case "2":
                    RegisterUser();
                    break;
                case "3":
                    LoginDriver();
                    break;
                case "4":
                    LoginUser();
                    break;
                case "5":
                    Console.WriteLine("Goodbye!");
                    return; // Exit the application
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\nPress any key to return to the main menu...");
            Console.ReadKey();
            Console.ResetColor();
        }
    }

    // Register Driver
    private void RegisterDriver()
    {
        DriverRegistration driverRegistration = new(this._context);
        driverRegistration.Run();
    }

    // Register User
    private void RegisterUser()
    {
        UserRegistration userRegistration = new(this._context);
        userRegistration.Run();
    }

    // login Driver
    private void LoginDriver()
    {
        DriverLogin driverLogin = new(this._context);
        driverLogin.Run();
    }

    // login User
    private void LoginUser()
    {
        UserLogin userLogin = new(this._context);
        var userDto = userLogin.Run();  // Get UserDto after successful login

        if (userDto == null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Login failed. Try again.");
            Console.ResetColor();
            return;
        }

        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("Press any key to continue.");
        Console.ReadKey();
        Console.ResetColor();

        // Pass the UserDto to UserMenu
        UserMenu userMenu = new(this._context, userDto);  // Passing UserDto here
        userMenu.Run();
    }
}
