namespace SmartRide.src.Services;

public class DeliveryService(SmartRideDbContext context, Graph<string> map)
{
    public Graph<string> map = map;
    private readonly SmartRideDbContext _dbcontext = context;
    private readonly DriverService _driverService = new(context);

    private T ConvertToT<T>(string input)
    {
        return (T)Convert.ChangeType(input, typeof(T));
    }

    // Updates the status of the package to picked after it has been assigned to a driver
    public void UpdatePackageStatus(PackageDto package, int userId, int driverId)
    {
        package.Status = "Assigned".ToUpper();
        var pack = new Package
        {
            Userid = userId,
            Source = package.Source,
            Driverid = driverId,
            Destination = package.Destination,
            Status = package.Status
        };

        try
        {
            // Insert it into the table
            _dbcontext.Packages.Add(pack);
            _dbcontext.SaveChanges();
            package.PackageId = pack.Packageid;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.InnerException?.Message);
        }

        // Display in green for success
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"The package is {package.Status}");
        Console.ResetColor();
    }

    // Status update after the package is delivered
    public void MarkPackageAsDelivered(PackageDto package)
    {
        ArgumentNullException.ThrowIfNull(package);

        package.Status = "Delivered";

        // Updating the status in the database
        var existingPackage = _dbcontext.Packages.FirstOrDefault(p => p.Packageid == package.PackageId);

        if (existingPackage != null)
        {
            existingPackage.Status = package.Status;
            try
            {
                _dbcontext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.InnerException.Message);
            }

            // Display in green for success
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Package {package.PackageId} has been marked as delivered.");
            Console.ResetColor();
        }
        else
        {
            // Display in red for error
            Console.ForegroundColor = ConsoleColor.Red;
            throw new InvalidOperationException($"Package with ID {package.PackageId} not found.");
        }

    }

    // Finds the nearest driver for the package
    public void AssignDriver(int userId, PackageDto package, List<DriverDto> drivers)
    {
        ArgumentNullException.ThrowIfNull(userId);
        ArgumentNullException.ThrowIfNull(package);
        if (drivers == null || drivers.Count == 0) throw new ArgumentException("No drivers available.");

        // Instantiate RideRequestMatching
        var req_matching = new RideRequestMatching();

        // Call RequestMatching with appropriate parameters
        var (driver, route, totalCost) = req_matching.RequestMatching(map, package.Source, package.Destination, drivers);

        // If a driver is found
        if (driver == null)
        {
            // Display in red for no available driver
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Unfortunately, no driver is available for the package right now.");
            Console.ResetColor();
            return;
        }

        // Show the total cost in yellow and ask the user for confirmation
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"\nThe total cost for this delivery will be: {totalCost:C}");
        Console.ResetColor();

        Console.Write("\nDo you want to proceed with this delivery? (Yes/No): ");
        string userResponse = Console.ReadLine()?.Trim().ToUpper() ?? "no";

        if (userResponse == "YES")
        {
            // Proceed with updating the package status and assigning the driver
            UpdatePackageStatus(package, userId, driver.Id);

            // Display driver info in green
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Driver {driver.Name} has been assigned to deliver package {package.PackageId}.");
            Console.WriteLine($"Route of your package: {string.Join(" -> ", route)}");
            Console.WriteLine($"Package Id: {package.PackageId} -- Keep this ID safe, you will need it to mark the package as complete.");
            Console.ResetColor();

        }
        else
        {
            // Display in red for no action
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("No action taken. Driver not assigned.");
            Console.ResetColor();
        }
    }

    public void CompleteDelivery(PackageDto package, DriverDto driver)
    {
        ArgumentNullException.ThrowIfNull(package);
        ArgumentNullException.ThrowIfNull(driver);

        // Directly mark the package as delivered
        MarkPackageAsDelivered(package);

        // Inform the user that the package has been successfully delivered
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Package {package.PackageId} has been successfully delivered.");
        Console.ResetColor();
    }

    int id_to_int(int? id)
    {
        return (int)id;
    }


    // Single method to retrieve both package and driver by package ID
    public (PackageDto? package, DriverDto? driver) GetPackageAndDriverById(int packageId)
    {
        // Retrieve the package from the database
        var package = _dbcontext.Packages.FirstOrDefault(p => p.Packageid == packageId);

        // Initialize PackageDto
        PackageDto? packageDto = null;
        if (package != null)
        {
            packageDto = new PackageDto
            {
                PackageId = package.Packageid,
                DriverId = id_to_int(package.Driverid),
                UserId = package.Userid,
                Source = package.Source,
                Destination = package.Destination,
                Status = package.Status ?? "ASSIGNED"
            };
        }

        // Retrieve the driver associated with the package using DriverId
        DriverDto? driverDto = null;
        if (packageDto != null && packageDto.DriverId != 0)
        {
            var driver = _dbcontext.Drivers.FirstOrDefault(d => d.Id == packageDto.DriverId);

            if (driver != null)
            {
                driverDto = this._driverService.GetDriver(driver.Email.ToUpper());
            }
        }

        // Return both PackageDto and DriverDto as a tuple
        return (packageDto, driverDto);
    }
}
