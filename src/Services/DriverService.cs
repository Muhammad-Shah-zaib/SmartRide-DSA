using SmartRide.src.DataStructures;

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
                _ => "WADERA CHOWK0.ToUpper()" // Default to empty string if not matching any of the specified IDs
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
}

