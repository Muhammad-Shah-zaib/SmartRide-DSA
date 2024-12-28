namespace SmartRide.src.Services;

public class DriverRatingService
{
    private readonly SmartRideDbContext _dbContext;
    private readonly MaxHeap<DriverRatingDto> _topDrivers;

    // In-memory collection to store driver ratings using custom HashMap
    private readonly HashMap<int, List<Driverrating>> _driverRatings;

    public DriverRatingService(SmartRideDbContext dbContext)
    {
        _dbContext = dbContext;
        _topDrivers = new MaxHeap<DriverRatingDto>();
        _driverRatings = new HashMap<int, List<Driverrating>>();

        LoadDriverRatings();
    }

    // Load ratings into an in-memory data structure
    private void LoadDriverRatings()
    {
        var allRatings = _dbContext.Driverratings.ToList(); // Load all ratings from the DB into memory

        // Organize ratings by DriverId
        foreach (var rating in allRatings)
        {
            // Check if the driver already exists in the HashMap
            var existingRatings = _driverRatings.Get(rating.Driverid);

            if (existingRatings == null)
                _driverRatings.Put(rating.Driverid, [rating]);
            else
                existingRatings.Add(rating);
        }

        // Calculate the average rating for each driver
        var driverIds = _driverRatings.Keys();
        foreach (var driverId in driverIds)
        {
            var ratings = _driverRatings.Get(driverId);
            var averageRating = ratings.Average(r => (double)r.Rating);
            var ratingCount = ratings.Count;

            // Create and insert into MaxHeap
            var driverRatingDto = new DriverRatingDto
            {
                DriverId = driverId,
                AverageRating = averageRating,
                RatingCount = ratingCount,
                Comment = ""
            };

            _topDrivers.Insert(driverRatingDto);
        }
    }

    // Add a rating for a driver
    public void AddRating(int driverId, double rating, string comment)
    {
        if (rating < 1 || rating > 5) throw new ArgumentException("Rating must be between 1 and 5.");

        // Create a new rating and add to the in-memory collection
        var newRating = new Driverrating
        {
            Driverid = driverId,
            Rating = (decimal)rating,
            Comment = comment,
            Createdat = DateTime.UtcNow
        };

        // Add rating to the in-memory data structure
        var existingRatings = _driverRatings.Get(driverId);
        if (existingRatings == null)
        {
            _driverRatings.Put(driverId, new List<Driverrating> { newRating });
        }
        else
        {
            existingRatings.Add(newRating);
        }

        // Save to DB
        _dbContext.Driverratings.Add(newRating);
        _dbContext.SaveChanges();

        // Recalculate average for the driver
        var updatedAverage = existingRatings.Average(r => (double)r.Rating);
        var ratingCount = existingRatings.Count;

        // Update the driver's entry in the heap
        var updatedRatingDto = new DriverRatingDto
        {
            DriverId = driverId,
            AverageRating = updatedAverage,
            RatingCount = ratingCount
        };

        _topDrivers.Insert(updatedRatingDto); // Insert the updated rating
    }

    // Get top rated drivers
    public IEnumerable<DriverRatingDto> GetTopRatedDrivers(int count)
    {
        return _topDrivers.ToList()
                          .OrderByDescending(d => d.AverageRating)
                          .Take(count);
    }

    // Get comments for a specific driver
    public IEnumerable<string> GetDriverComments(int driverId)
    {
        var ratings = _driverRatings.Get(driverId);
        return ratings != null
            ? ratings.Select(r => r.Comment).ToList()
            : new List<string>();
    }

    // Calculate the average rating for a specific driver (in-memory)
    private double CalculateDriverAverage(int driverId)
    {
        var ratings = _driverRatings.Get(driverId);
        return ratings != null
            ? ratings.Average(r => (double)r.Rating)
            : 0;
    }

    // Get the number of ratings for a specific driver (in-memory)
    private int GetDriverRatingCount(int driverId)
    {
        var ratings = _driverRatings.Get(driverId);
        return ratings?.Count ?? 0;
    }
}
