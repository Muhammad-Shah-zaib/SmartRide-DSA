namespace SmartRide.src.Dtos
{
    public class RideRequestDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }

        public string Status {  get; set; }
        public DateTime RideTime {  get; set; }

    }
}
