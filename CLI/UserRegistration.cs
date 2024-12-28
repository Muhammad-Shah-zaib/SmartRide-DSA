namespace SmartRide.CLI;

public class UserRegistration(SmartRideDbContext context)
{
    private readonly UserService _userService = new(context);

    public void Run()
    {
        Console.Clear();
        Console.WriteLine("User Registration");

        try
        {
            // Ask user for required fields
            Console.Write("Enter your name: ");
            string name = Console.ReadLine() ?? string.Empty;
            name = name.Trim().ToUpper();

            Console.Write("Enter your email: ");
            string email = Console.ReadLine() ?? string.Empty;
            email = email.Trim().ToUpper();

            Console.Write("Enter your phone number: ");
            string phoneNumber = Console.ReadLine() ?? string.Empty;
            phoneNumber = phoneNumber.Trim().ToUpper();

            // Construct a UserDto
            UserDto user = new()
            {
                Name = name,
                Email = email,
                PhoneNumber = phoneNumber
            };

            // Add the user via the service
            _userService.AddUser(user);

            // Display success message with user details
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nUser registered successfully!");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("---------------------");
            Console.WriteLine($"ID: {user.Id}");
            Console.WriteLine($"Name: {user.Name}");
            Console.WriteLine($"Email: {user.Email}");
            Console.WriteLine($"Phone Number: {user.PhoneNumber}");
            Console.WriteLine("---------------------");
            Console.ResetColor();
        }
        catch (ArgumentException ex) when (ex.Message.Contains("Email has already used"))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nError: The email you entered is already in use. Please try a different email.");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\nAn unexpected error occurred: {ex.Message}");
            Console.ResetColor();
        }

    }
}
