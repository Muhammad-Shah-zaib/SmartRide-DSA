namespace SmartRide.CLI;

public class UserLogin
{
    private readonly UserService _userService;

    public UserLogin(SmartRideDbContext context)
    {
        _userService = new UserService(context);
    }

    public UserDto Run()  // Change return type to UserDto instead of bool
    {
        Console.Clear();
        Console.WriteLine("User Login");

        int attempts = 0;
        const int maxAttempts = 3;

        while (attempts < maxAttempts)
        {
            try
            {
                // Prompt user for credentials
                Console.Write("Enter your email: ");
                string email = Console.ReadLine()?.Trim().ToUpper() ?? string.Empty;

                Console.Write("Enter your phone number: ");
                string phoneNumber = Console.ReadLine()?.Trim() ?? string.Empty;

                // Validate user using HashMap
                var user = _userService.GetUser(email);
                if (user != null && user.PhoneNumber == phoneNumber)
                {
                    // Successful login
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("\nLogin successful!");
                    Console.ResetColor();

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("---------------------");
                    Console.WriteLine($"ID: {user.Id}");
                    Console.WriteLine($"Name: {user.Name}");
                    Console.WriteLine($"Email: {user.Email}");
                    Console.WriteLine($"Phone Number: {user.PhoneNumber}");
                    Console.WriteLine("---------------------");
                    Console.ResetColor();

                    return user;  // Return the UserDto object on successful login
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

        // If user fails all attempts
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("\nYou have exceeded the maximum number of login attempts. Access denied.");
        Console.ResetColor();
        return null;  // Return null if login failed after multiple attempts
    }
}
