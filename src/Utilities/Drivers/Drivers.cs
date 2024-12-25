namespace SmartRide.src.Utilities.Drivers;

public class Drivers
{
    public static List<DriverDto> InitializeDrivers() {
        var driver1 = new DriverDto() { 
            Name = "driver1",
            Rating = 2.0,
            CurrentPosition = new Node()
            {
                Name = "GATE-1",
            }
        };

        var driver2 = new DriverDto()
        {
            Name = "driver2",
            Rating = 2.0,
            CurrentPosition = new Node()
            {
                Name = "GATE-2",
            }
        };

        var driver3 = new DriverDto()
        {
            Name = "driver3",
            Rating = 2.0,
            CurrentPosition = new Node()
            {
                Name = "SUPREME COURT",
            }
        };


        return [driver1, driver2, driver3];
    }
}
