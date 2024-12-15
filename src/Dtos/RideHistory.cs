namespace SmartRide.src.Dtos;

public class RideHistoryDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Source { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public DateTime RideDate { get; set; }
    public string Log { get; set; } = string.Empty; // for description
}
