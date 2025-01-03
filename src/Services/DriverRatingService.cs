﻿namespace SmartRide.src.Services;

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

        // Create a new rating
        var newRating = new Driverrating
        {
            Driverid = driverId,
            Rating = (decimal)rating,
            Comment = comment ?? "", // Ensure comment is not null
            Createdat = DateTime.UtcNow
        };

        // Add to in-memory structure
        var existingRatings = _driverRatings.Get(driverId) ?? new List<Driverrating>();
        if (!_driverRatings.ContainsKey(driverId)) _driverRatings.Put(driverId, existingRatings);
        existingRatings.Add(newRating);

        // Save the new rating to the database
        _dbContext.Driverratings.Add(newRating);

        // Recalculate driver's average rating
        var updatedAverage = existingRatings.Any() ? existingRatings.Average(r => (double)r.Rating) : 0;
        var ratingCount = existingRatings.Count;

        // Update driver's MaxHeap entry
        var updatedRatingDto = new DriverRatingDto
        {
            DriverId = driverId,
            AverageRating = updatedAverage,
            RatingCount = ratingCount
        };
        _topDrivers.Insert(updatedRatingDto);

        // Update driver's rating in the database
        var driver = _dbContext.Drivers.FirstOrDefault(d => d.Id == driverId);
        if (driver != null)
        {
            driver.Rating = (decimal?)updatedAverage;
        }

        // Save changes to the database
        _dbContext.SaveChanges();
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

    // Get the average rating for a specific driver from the database
    public DriverRatingDto GetDriverRating(int driverId)
    {       
        // Retrieve ratings for the driver
        var ratings = _driverRatings.Get(driverId);

        if (ratings == null || !ratings.Any())
        {
            // Return a default value if no ratings exist for the driver
            return new DriverRatingDto
            {
                DriverId = driverId,
                AverageRating = 0,
                RatingCount = 0,
                Comment = "No ratings yet."
            };
        }

        // Calculate the average rating
        var averageRating = ratings.Average(r => (double)r.Rating);
        var ratingCount = ratings.Count;

        // Create a DriverRatingDto to return
        return new DriverRatingDto
        {
            DriverId = driverId,
            AverageRating = averageRating,
            RatingCount = ratingCount,
            Comment = string.Join(", ", ratings.Select(r => r.Comment)) // Combine all comments
        };
    }

    // Get all comments for a specific driver from the HashMap
    public List<string> GetAllDriverComments(int driverId)
    {
        this.LoadDriverRatings();
        var ratings = _driverRatings.Get(driverId);

        // If there are no ratings, return an empty list
        if (ratings == null || !ratings.Any())
        {
            return new List<string> { "No comments available for this driver." };
        }

        // Extract and return all comments
        return ratings.Select(r => r.Comment).ToList();
    }

}