using SmartRide.src.Dtos;

public class PackageDto
{
    public int PackageId { get; set; }
    public int DriverId { get; set; }
    public int UserId { get; set; }
    public string Source { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public string Status { get; set; } = "Pending";
}