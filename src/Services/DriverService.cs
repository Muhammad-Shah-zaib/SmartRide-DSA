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
        var drivers = _dbContext.Drivers.ToList();
        foreach (var driver in drivers)
        {
            _driverMap.Put(driver.Email, new DriverDto() {
                Id = driver.Id,
                Name = driver.Name,
                LicenseNumber = driver.Licensenumber,
                Rating = driver.Rating.HasValue ? (double)driver.Rating.Value : 0.0,
            });
        }
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

