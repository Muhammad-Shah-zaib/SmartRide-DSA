namespace SmartRide.src.Dtos;

public class CarpoolRideDto
{
    public int CarpoolId { get; set; }
    public int DriverId { get; set; }
    public string Route { get; set; } = string.Empty;
    public int MaxPassengers { get; set; } 
    public int CurrentPassengers { get; set; }
    public string Status { get; set; } = string.Empty; // (ACTIVE, FULL, COMPLETED)
    public string Src { get; set; } = string.Empty;
    public string Dest { get; set; } = string.Empty;
    public double TotalCost { get; set; } = 0.0;
    public DateTime CreatedAt { get; set; } 
    public DateTime UpdatedAt { get; set; } // Last updated timestamp
}
