using Microsoft.EntityFrameworkCore;

namespace SmartRide.src.Services;

public class DriverService
{
    private readonly SmartRideDbContext _dbContext;
    private readonly HashMap<string, DriverDto> _driverMap;

    public DriverService(SmartRideDbContext context)
    {
        _dbContext = context;
        _driverMap = new HashMap<string, DriverDto>(100);

        // Load data from the database into the HashMap
        LoadDriversFromDb();
    }
    private void LoadDriversFromDb()
    {
        this._driverMap.Clear();
        var drivers = _dbContext.Drivers.ToList();
        foreach (var driver in drivers)
        {
            // Set current position based on driver's Id
            var currentPositionName = driver.Id switch
            {
                1 => "GATE-1".ToUpper(),
                2 => "GATE-2".ToUpper(),
                3 => "SUPREME COURT".ToUpper(),
                _ => "WADERA CHOWK".ToUpper() // Default to empty string if not matching any of the specified IDs
            };

            var currentPositionType = driver.Id switch
            {
                1 => "TOWN-GATE".ToUpper(),
                2 => "TOWN-GATE".ToUpper(),
                3 => "MAIN OFFICE".ToUpper(),
                _ => "CHOWK".ToUpper()
            };

            // Add driver data to the map
            _driverMap.Put(driver.Email, new DriverDto()
            {
                Id = driver.Id,
                Name = driver.Name,
                Email = driver.Email,
                LicenseNumber = driver.Licensenumber,
                Rating = driver.Rating.HasValue ? (double)driver.Rating.Value : 0.0,
                CurrentPosition = new Node()
                {
                    Name = currentPositionName,
                    Type = currentPositionType
                }
            });
        }
    }

    public bool ValidateDriver(string email, string licenseNumber, out DriverDto? driver)
    {
        driver = _driverMap.Get(email);
        if (driver == null)
        {
            return false; // Driver not found
        }

        // Check if the phone number matches
        if (driver.LicenseNumber == licenseNumber)
        {
            return true; // Validation successful
        }

        return false; // Validation failed
    }


    public void AddDriver(DriverDto driver)
    {
        if (_driverMap.ContainsKey(driver.Email))
        {
            throw new Exception("Driver with the same Email already exists.");
        }
        var newDriver = new Driver()
        {
            Name = driver.Name,
            Licensenumber = driver.LicenseNumber,
            Email = driver.Email,
            Rating = (decimal)driver.Rating
        };
        _dbContext.Drivers.Add(newDriver);
        _dbContext.SaveChanges();

        driver.Id = newDriver.Id;

        _driverMap.Put(driver.Email, driver);
    }
    public List<DriverDto> GetAllAvailableDrivers()
    {
        return _driverMap.Values().Where(driver => driver.IsAvailable).ToList();
    }

    public DriverDto? GetDriver(string email)
    {
        return _driverMap.Get(email);
    }

    public void RemoveDriver(string email)
    {
        var driver = _dbContext.Drivers.FirstOrDefault(d => d.Email == email) 
            ?? throw new Exception("Driver not found.");
        _dbContext.Drivers.Remove(driver);
        _dbContext.SaveChanges();

        _driverMap.Remove(email);
    }

    public void UpdateDriverEmail(string oldEmail, string newEmail, SmartRideDbContext _context)
    {
        try
        {
            // Validate that the driver exists in the database
            Console.WriteLine($"Searching for driver with email: {oldEmail}");
            var driver = _context.Drivers.FirstOrDefault(d => d.Email.Trim().ToUpper() == oldEmail.Trim().ToUpper())
                ?? throw new InvalidOperationException("Driver not found.");
            Console.WriteLine($"Driver found: {driver.Name}");

            // Check if the new email is not null and valid
            if (string.IsNullOrEmpty(newEmail))
            {
                throw new ArgumentException("New email cannot be null or empty.");
            }

            // Check if the new email already exists in the database
            if (_context.Drivers.Any(d => d.Email == newEmail))
            {
                throw new InvalidOperationException($"The email {newEmail} is already in use.");
            }

            // Update the driver's email
            driver.Email = newEmail;
            var drive = _driverMap.Get(oldEmail);
            _driverMap.Remove(oldEmail);
            drive.Email = newEmail;
            _driverMap.Put(newEmail, drive);
            _context.SaveChanges();
            Console.WriteLine("Email updated successfully.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }


    public void UpdateDriver(SmartRideDbContext _context,DriverDto driver)
    {
        var existingDriver = _context.Drivers.FirstOrDefault(d => d.Email == driver.Email);
        var drive = _driverMap.Get(driver.Email);
        _driverMap.Remove(driver.Email);
        if (existingDriver != null)
            {
                
                existingDriver.Licensenumber = driver.LicenseNumber;
                drive.LicenseNumber = driver.LicenseNumber;
            _context.SaveChanges();
            }
        
        _driverMap.Put(drive.Email , drive);
    }
        
}

