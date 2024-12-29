namespace SmartRide.CLI;

public class DriverMenu
{
    private readonly SmartRideDbContext _context;
    private readonly DriverService _driverService;
    private readonly DriverRatingService _driverRatingService;

    public DriverDto Driver { get; private set; }

    public DriverMenu(SmartRideDbContext context, DriverDto driver)
    {
        _context = context;
        Driver = driver;
        _driverService = new DriverService(context);
        _driverRatingService = new DriverRatingService(context);
    }

    public void Run()
    {
        while (true)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n================ WELCOME TO SMART RIDE! ================\n");
            Console.ResetColor();

            if (Driver != null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Hello, {Driver.Name}! Let's make your journey smoother!");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please log in to access your account. Press any key to continue.");
                Console.ResetColor();
                Console.ReadLine();
                return;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\nWhat would you like to do today?\n");
            Console.WriteLine("1. Update your Email Address.");
            Console.WriteLine("2. Update your License Number.");
            Console.WriteLine("3. View Your Ratings and Comments.");
            Console.WriteLine("4. View Your Rides.");
            Console.WriteLine("5. Log Out.");
            Console.ResetColor();

            Console.Write("\nEnter your choice (1-5): ");
            string choice = Console.ReadLine()?.Trim() ?? "";

            switch (choice)
            {
                case "1":
                    UpdateEmail();
                    break;
                case "2":
                    UpdateLicenseNumber();
                    break;
                case "3":
                    DisplayRatings();
                    break;
                case "4":
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("\nFeature under construction. Stay tuned!");
                    Console.ResetColor();
                    Prompt.PressKeyToContinue();
                    break;
                case "5":
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine("\nLogging out... Stay safe and drive responsibly!");
                    Console.ResetColor();
                    Driver = null;
                    return;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nInvalid choice. Please try again.");
                    Console.ResetColor();
                    Prompt.PressKeyToContinue();
                    break;
            }
        }

        private void Rides()
        {
            Console.WriteLine("Rides: ");

        }
    }

    private void UpdateEmail()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n===== Update Email Address =====\n");
        Console.ResetColor();

        Console.Write("Enter your new email address: ");
        string newEmail = Console.ReadLine()?.Trim();

        if (string.IsNullOrEmpty(newEmail))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid email address. Please try again.");
            Console.ResetColor();
            return;
        }

        if (_driverService.GetDriver(newEmail) != null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Email already in use. Please try a different one.");
            Console.ResetColor();
            return;
        }

        _driverService.UpdateDriverEmail(Driver.Email, newEmail, _context);
        Driver = _driverService.GetDriver(newEmail);

        if (Driver != null)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nEmail updated successfully! Here are your updated details:");
            Console.WriteLine($"Name: {Driver.Name}");
            Console.WriteLine($"Email: {Driver.Email}");
            Console.WriteLine($"License Number: {Driver.LicenseNumber}");
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nAn error occurred while updating your email. Please try again.");
            Console.ResetColor();
        }

        Prompt.PressKeyToContinue();
    }

    private void UpdateLicenseNumber()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n===== Update License Number =====\n");
        Console.ResetColor();

        Console.Write("Enter your new license number: ");
        string newLicenseNumber = Console.ReadLine()?.Trim();

        if (string.IsNullOrEmpty(newLicenseNumber))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid license number. Please try again.");
            Console.ResetColor();
            return;
        }

        Driver.LicenseNumber = newLicenseNumber;
        _driverService.UpdateDriver(_context, Driver);
        Driver = _driverService.GetDriver(Driver.Email);

        if (Driver != null)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\nLicense number updated successfully! Here are your updated details:");
            Console.WriteLine($"Name: {Driver.Name}");
            Console.WriteLine($"Email: {Driver.Email}");
            Console.WriteLine($"License Number: {Driver.LicenseNumber}");
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nAn error occurred while updating your license number. Please try again.");
            Console.ResetColor();
        }

        Prompt.PressKeyToContinue();
    }

    private void DisplayRatings()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n===== Your Ratings and Comments =====\n");
        Console.ResetColor();

        var rating = _driverRatingService.GetDriverRating(Driver.Id);

        if (rating != null)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Average Rating: {rating.AverageRating:F2} ({rating.RatingCount} ratings)");

            var comments = _driverRatingService.GetAllDriverComments(Driver.Id);
            if (comments != null && comments.Any())
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nCustomer Comments:");
                foreach (var comment in comments)
                {
                    Console.WriteLine($"- {comment}");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine("\nNo comments available yet.");
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("\nNo ratings available yet.");
        }

        Console.ResetColor();
        Prompt.PressKeyToContinue();
    }
}
