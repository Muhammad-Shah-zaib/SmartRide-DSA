namespace SmartRide.src.Utilities.Carpool;

public class Carpool
{

    public static void InitializeCarpool()
    {
        using var dbContext = new SmartRideDbContext();
        // Initialize the CarpoolService
        var carpoolService = new CarpoolService(dbContext);

        // Create sample carpools
        var newCarpools = new List<CarpoolRideDto>
            {
                new CarpoolRideDto {
                    DriverId = 1,
                    Route = "LocationA",
                    MaxPassengers = 4,
                    CurrentPassengers = 1,
                    Status = "ACTIVE",
                    Src = "BOLAN CHOWK",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
                new CarpoolRideDto
                {
                    DriverId = 2,
                    Route = "LocationB",
                    MaxPassengers = 3,
                    CurrentPassengers = 2,
                    Status = "ACTIVE",
                    Src  = "IQBAL CHOWK",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                },
            };

        // Add each carpool using the CarpoolService
        foreach (var carpool in newCarpools)
        {
            carpoolService.AddNewCarpool(carpool);
        }
    }
}
