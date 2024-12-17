namespace SmartRide.src.Services;

public class DriverRatingService
{
    private readonly SmartRideDbContext _dbContext;
    private readonly MaxHeap<DriverRatingDto> _topDrivers;

    public DriverRatingService(SmartRideDbContext dbContext)
    {
        _dbContext = dbContext;
        _topDrivers = new MaxHeap<DriverRatingDto>();

        LoadDriverRatings();
    }

    private void LoadDriverRatings()
    {
        var driverGroups = _dbContext.Driverratings
            .GroupBy(r => r.Driverid)
            .Select(g => new DriverRatingDto
            {
                DriverId = g.Key,
                AverageRating = g.Average(r => (double)r.Rating),
                RatingCount = g.Count(),
                Comment = "" // No aggregation of comments, left blank
            })
            .ToList();

        foreach (var driverRating in driverGroups)
            _topDrivers.Insert(driverRating);
    }

    public void AddRating(int driverId, double rating, string comment)
    {
        if (rating < 0 || rating > 5) throw new ArgumentException("Rating must be between 0 and 5.");

        var newRating = new Driverrating
        {
            Driverid = driverId,
            Rating = (decimal)rating,
            Comment = comment,
            Createdat = DateTime.UtcNow
        };

        _dbContext.Driverratings.Add(newRating);
        _dbContext.SaveChanges();

        // Update the driver's average rating in the heap
        var updatedRating = new DriverRatingDto
        {
            DriverId = driverId,
            AverageRating = CalculateDriverAverage(driverId),
            RatingCount = GetDriverRatingCount(driverId)
        };

        _topDrivers.Insert(updatedRating);
    }

    public IEnumerable<DriverRatingDto> GetTopRatedDrivers(int count)
    {
        return _topDrivers.ToList()
                          .OrderByDescending(d => d.AverageRating)
                          .Take(count);
    }

    public IEnumerable<string> GetDriverComments(int driverId)
    {
        return _dbContext.Driverratings
                         .Where(r => r.Driverid == driverId)
                         .Select(r => r.Comment)
                         .ToList();
    }

    private double CalculateDriverAverage(int driverId)
    {
        return _dbContext.Driverratings
               .Where(r => r.Driverid == driverId)
               .Average(r => (double)r.Rating);
    }

    private int GetDriverRatingCount(int driverId)
    {
        return _dbContext.Driverratings
               .Count(r => r.Driverid == driverId);
    }
}
