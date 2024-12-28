namespace SmartRide.CLI;

public class DriverLogin(SmartRideDbContext context)
{
    private readonly DriverService _driverService = new(context);

    public bool Run()
    {
        Console.Clear();
        Console.WriteLine("Driver Login");

        int attempts = 0;
        const int maxAttempts = 3;

        while (attempts < maxAttempts)
        {
            try
            {
                // Prompt driver for their credentials
                Console.Write("Enter your email: ");
                string email = Console.ReadLine()?.Trim().ToUpper() ?? string.Empty;

                Console.Write("Enter your phone number: ");
                string phoneNumber = Console.ReadLine()?.Trim().ToUpper() ?? string.Empty;

                // Validate the driver using the HashMap
                if (_driverService.ValidateDriver(email, phoneNumber, out var driver))
                {
                    // Display success message and driver details
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nLogin successful!");
                    Console.ResetColor();

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("---------------------");
                    Console.WriteLine($"ID: {driver!.Id}");
                    Console.WriteLine($"Name: {driver.Name}");
                    Console.WriteLine($"Email: {driver.Email}");
                    Console.WriteLine($"License Number: {driver.LicenseNumber}");
                    Console.WriteLine("---------------------");
                    Console.ResetColor();

                    return true; // Exit after successful login
                }
                else
                {
                    throw new ArgumentException("Invalid email or phone number.");
                }
            }
            catch (ArgumentException ex)
            {
                attempts++;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nError: {ex.Message} ({maxAttempts - attempts} attempts remaining)");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nAn unexpected error occurred: {ex.Message}");
                Console.ResetColor();
                break; // Exit loop for unexpected errors
            }
        }

        // If the user fails all attempts
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\nYou have exceeded the maximum number of login attempts. Access denied.");
        Console.ResetColor();
        return false;
    }

}
