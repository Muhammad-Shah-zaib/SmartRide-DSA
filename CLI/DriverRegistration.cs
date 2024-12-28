namespace SmartRide.CLI;

public class DriverRegistration(SmartRideDbContext context)
{
    private readonly DriverService _driverService = new DriverService(context);

    public void Run()
    {
        Console.Clear();
        Console.WriteLine("----Driver Registration----");

        try
        {
            // Ask user for required fields
            Console.Write("Enter your name: ");
            string name = Console.ReadLine() ?? string.Empty;
            name = name.Trim().ToUpper();

            Console.Write("Enter your license number: ");
            string licenseNumber = Console.ReadLine() ?? string.Empty;
            licenseNumber = licenseNumber.Trim().ToUpper();

            Console.Write("Enter your email: ");
            string email = Console.ReadLine() ?? string.Empty;
            email = email.Trim().ToUpper();

            // Construct a DriverDto with default values for non-required fields
            DriverDto driver = new()
            {
                Name = name,
                LicenseNumber = licenseNumber,
                Email = email,
                Rating = 0.0, // Default rating
            };

            // Attempt to add the driver
            Console.ForegroundColor = ConsoleColor.Green;
            _driverService.AddDriver(driver);
            Console.ResetColor();

            Console.WriteLine("Driver registered successfully!");
            // print driverDetails
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("---------------------");
            Console.WriteLine($"id: {driver.Id}");
            Console.WriteLine($"name: {driver.Name}");
            Console.WriteLine($"Email: {driver.Email}");
            Console.WriteLine($"LicenseNumber: {driver.LicenseNumber}");
            Console.WriteLine("---------------------");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            // Handle specific exceptions and provide meaningful feedback
            if (ex.Message.Contains("Driver with the same Email already exists"))
            {
                Console.WriteLine("Error: A driver with the same email already exists. Please use a different email.");
            }
            else
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        Console.WriteLine("\nPress any key to return to the main menu...");
        Console.ReadKey();
    }

}
