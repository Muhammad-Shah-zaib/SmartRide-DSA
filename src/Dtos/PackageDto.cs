using SmartRide.src.Dtos;

public class PackageDto
{
    public int PackageId { get; set; }
    public int UserId { get; set; }
    public string Source { get; set; }
    public string Destination { get; set; }
    public string Status { get; set; } = "Pending";
}