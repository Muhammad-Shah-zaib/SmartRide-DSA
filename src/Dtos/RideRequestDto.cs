namespace SmartRide.src.Dtos
{
    public class RideRequestDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Source { get; set; } = string.Empty;
        public string Destination { get; set; } = string.Empty;

        public string Status {  get; set; } = string.Empty;
        public DateTime RideTime {  get; set; }

    }
}
