namespace SmartRide.src.Dtos;

public class PackageDeliveryCli(SmartRideDbContext context, UserDto currentUser, Graph<string> map)
{
    private readonly DeliveryService _deliveryService = new(context, map);
    private readonly DriverService _driverService = new(context);
    private UserDto CurrentUser { get; set; } = currentUser;

    public void Run()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine($"Welcome, {CurrentUser.Name}, to SmartRide Package Delivery CLI!");
            Console.WriteLine("\nWhat would you like to do?");
            Console.WriteLine("1. Assign a driver to a package");
            Console.WriteLine("2. Mark a package as delivered");
            Console.WriteLine("3. Exit");
            Console.Write("Enter your choice (1/2/3): ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AssignDriverToPackage();
                    break;
                case "2":
                    MarkPackageAsDelivered();
                    break;
                case "3":
                    Console.WriteLine("Exiting the CLI. Thank you!");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
            Prompt.PressKeyToContinue();
        }
    }

    private void AssignDriverToPackage()
    {
        Console.Write("\nEnter the source location of the package: ");
        string source = Console.ReadLine()?.Trim().ToUpper() 
            ?? throw new ArgumentNullException("Source location is required.");

        Console.Write("Enter the destination location of the package: ");
        string destination = Console.ReadLine()?.Trim().ToUpper() 
            ?? throw new ArgumentNullException("Destination location is required.");

        // Create a PackageDto
        var package = new PackageDto
        {
            UserId = CurrentUser.Id,
            Status = "Assigned".ToUpper(),
            Source = source,
            Destination = destination
        };

        // Retrieve available drivers using DriverService
        var drivers = _driverService.GetAllAvailableDrivers();
        if (drivers.Count == 0)
        {
            Console.WriteLine("No drivers available to assign.");
            return;
        }

        // Use DeliveryService to assign a driver
        _deliveryService.AssignDriver(CurrentUser.Id, package, drivers);
    }

    private void MarkPackageAsDelivered()
    {
        try
        {
            // Prompt for the package ID
            Console.Write("\nEnter the package ID: ");
            int packageId = int.Parse(Console.ReadLine() ?? throw new ArgumentNullException("Package ID is required."));

            // Use the DeliveryService to retrieve both the package and the driver by package ID
            var (package, driver) = _deliveryService.GetPackageAndDriverById(packageId);

            // Validate if the package exists
            if (package == null)
            {
                Console.WriteLine($"Package with ID {packageId} not found.");
                return;
            }

            // Validate if the driver exists
            if (driver == null)
            {
                Console.WriteLine($"Driver for package ID {packageId} not found.");
                return;
            }

            // Confirm delivery
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Has the package been delivered? (yes/no): ");
            Console.ResetColor();
            string confirmation = Console.ReadLine()?.Trim().ToLower();

            if (confirmation == "yes")
            {
                // Complete the delivery using the DeliveryService
                _deliveryService.CompleteDelivery(package, driver);
            }
            else
            {
                Console.WriteLine("Delivery not completed.");
            }
        }
        catch (FormatException)
        {
            Console.WriteLine("Invalid input. Please enter a valid package ID.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

    }

}
