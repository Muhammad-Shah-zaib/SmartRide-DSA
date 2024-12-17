namespace SmartRide.src.Dtos;

public class DriverRatingDto : IComparable<DriverRatingDto>
{
    public int DriverId { get; set; }
    public double AverageRating { get; set; }
    public int RatingCount { get; set; }
    public string Comment { get; set; } = string.Empty;

    public int CompareTo(DriverRatingDto? other)
    {
        if (other == null) return 1;
        return AverageRating.CompareTo(other.AverageRating);
    }

    public override string ToString() =>
        $"DriverId: {DriverId}, AverageRating: {AverageRating:F1}, RatingCount: {RatingCount}";
}
