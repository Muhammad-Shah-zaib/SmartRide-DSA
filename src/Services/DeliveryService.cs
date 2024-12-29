namespace SmartRide.src.Services;

public class DeliveryService
{
    //map 
    public Graph<string> map;
    private SmartRideDbContext _dbcontext;
    public DeliveryService(SmartRideDbContext dbcontext,Graph<string> map)

    {
        _dbcontext = dbcontext ?? throw new ArgumentNullException(nameof(dbcontext)); ;
        this.map = map;
    }

    private T ConvertToT<T>(string input)
    {
        return (T)Convert.ChangeType(input, typeof(T));
    }
    //Updates the status of the package to picked after it has been assigned to a driver
    public void UpdatePackageStatus(PackageDto package, UserDto user)
    {
        package.Status = "Picked";
        var pack = new Package
        {
            Packageid = package.PackageId,
            Userid = user.Id,
            Source = package.Source,
            Destination = package.Destination,
            Status = package.Status
        };

        try { 
            //insert it into the table
            _dbcontext.Packages.Add(pack);
            _dbcontext.SaveChanges();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.InnerException.Message);
        }
        Console.WriteLine($"The package is {package.Status}");

    }
    //Status update after the package is delivered
    public void MarkPackageAsDelivered(PackageDto package)
    {
        if (package == null) throw new ArgumentNullException(nameof(package));

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
            catch(Exception e)
            {
                Console.WriteLine(e.InnerException.Message);
            }
        }
        else
        {
            throw new InvalidOperationException($"Package with ID {package.PackageId} not found.");
        }
    }
    //finds the nearest driver for the package
    // In the DeliveryService class

    // In the DeliveryService class

    public void Assign_package(UserDto user, PackageDto package, List<DriverDto> drivers)
    {
        if (user == null) throw new ArgumentNullException(nameof(user));
        if (package == null) throw new ArgumentNullException(nameof(package));
        if (drivers == null || drivers.Count == 0) throw new ArgumentException("No drivers available.");

        // Instantiate without the generic argument
        var req_matching = new RideRequestMatching();

        // Call RequestMatching with appropriate parameters (user's current position, source, and destination)
        var (driver, _, _) = req_matching.RequestMatching(map, user.CurrentPosition, package.Destination, drivers);

        if (driver != null)
        {
            UpdatePackageStatus(package, user); // Updates the status
        }
        else
        {
            Console.WriteLine("No driver available");
        }

        // Question about the current location
        Console.WriteLine("Have you delivered the package?\nyes or no\n");
        var loc = Console.ReadLine().ToLower();

        // Updates the driver location
        if (loc == "yes")
        {
            driver.CurrentPosition.Name = package.Destination;
        }

        if (driver.CurrentPosition.Name == package.Destination)
        {
            MarkPackageAsDelivered(package);
            Console.WriteLine($"Package {package.PackageId} has been delivered.");
        }
        else
        {
            Console.WriteLine("Package could not be delivered");
        }
    }


}