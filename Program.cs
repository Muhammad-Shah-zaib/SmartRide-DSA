global using SmartRide.Models;
global using SmartRide.src.Dtos;
global using SmartRide.src.Services;
using Microsoft.EntityFrameworkCore;

class Program
{
    static void Main(string[] args)
    {
        var options = new DbContextOptionsBuilder<SmartRideDbContext>()
            .UseNpgsql("Host=localhost;Database=SmartRideDb;Username=postgres;Password=16df")
            .Options;

        var dbContext = new SmartRideDbContext(options);

        var userService = new UserService(dbContext);
        var driverService = new DriverService(dbContext);

        // Test UserService
        var user = new UserDto { Id = 1, Name = "shahzaib", Email = "shahzaib@example.com", PhoneNumber = "1234567890" };
        userService.AddUser(user);
        user = userService.GetUser("alice@example.com");
        if (user != null)
            Console.WriteLine($"User Retrieved: {user.Name}");
        else Console.WriteLine($"User not found with email alice@example.com");

        Console.WriteLine($"User: {userService.GetUser("shahzaib@example.com")}");

        // Test DriverService
        var driver = new DriverDto { Id = 1, Name = "Bob", LicenseNumber = "LIC123", Rating = 4.5,Email = "DRIVER@EXAMPLE.COM" };
        driverService.AddDriver(driver);
        var d = driverService.GetDriver("DRIVER@EXAMPLE.COM");
        if (d != null)
            Console.WriteLine($"Driver Retrieved: {driver.Name}");
        else Console.WriteLine($"Driver with email:DRIVER@EXAMPLE.COM not found");
    }

}
