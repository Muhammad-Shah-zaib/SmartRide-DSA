namespace SmartRide.src.Dtos;

public class UserRideHistoryDto
{
    public int Id { get; set; }

    public int UserId { get; set; } // ID of the user who took the ride

    public string RideStartLocation { get; set; } = string.Empty;

    public string RideEndLocation { get; set; } = string.Empty;

    public decimal RideDistance { get; set; } // total weight  of the route chosen

    public TimeSpan RideDuration { get; set; }

    public DateTime RideDate { get; set; }

    public decimal RideCost { get; set; } // Cost of the ride
}
