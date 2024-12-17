# DriverRatingService Documentation

## Class: `DriverRatingService`

<details>
<summary><strong>Purpose</strong></summary>
Handles driver ratings, including adding new ratings, calculating average ratings, and retrieving the top-rated drivers. It uses a custom `MaxHeap` data structure for efficient access to the top-rated drivers.
</details>

---

### Constructor: `DriverRatingService(SmartRideDbContext dbContext)`

<details>
<summary><strong>Summary</strong></summary>
Initializes the `DriverRatingService` with the database context and loads existing driver ratings into a `MaxHeap` for efficient management.
</details>

**Parameters**:
- `dbContext` (`SmartRideDbContext`): The database context used for accessing and modifying driver rating data.

---

### Method: `AddRating(int driverId, double rating, string comment)`

<details>
<summary><strong>Summary</strong></summary>
Adds a new rating for a driver and updates their average rating in the system.
</details>

**Parameters**:
- `driverId` (`int`): The unique ID of the driver being rated.
- `rating` (`double`): The rating given to the driver (must be between 0 and 5).
- `comment` (`string`): Optional feedback or comment about the driver.

**Exceptions**:
- `ArgumentException`: Thrown if the rating is outside the valid range (0 to 5).

---

### Method: `GetTopRatedDrivers(int count)`

<details>
<summary><strong>Summary</strong></summary>
Retrieves the top-rated drivers based on their average rating.
</details>

**Parameters**:
- `count` (`int`): The number of top-rated drivers to retrieve.

**Returns**:
- `IEnumerable<DriverRatingDto>`: A list of top-rated drivers.

---

### Method: `GetDriverComments(int driverId)`

<details>
<summary><strong>Summary</strong></summary>
Retrieves all comments/feedback for a specific driver.
</details>

**Parameters**:
- `driverId` (`int`): The unique ID of the driver whose comments are being retrieved.

**Returns**:
- `IEnumerable<string>`: A list of comments for the specified driver.

---

### Data Structures Used:

- **`MaxHeap<DriverRatingDto>`**:  
   A custom MaxHeap data structure to ensure efficient access to the top-rated drivers.

---

### Example Usage:

**Adding a New Rating**:
```csharp
var driverRatingService = new DriverRatingService(dbContext);

driverRatingService.AddRating(
    driverId: 1001,
    rating: 4.8,
    comment: "Smooth ride and very professional driver."
);

Console.WriteLine("Rating added successfully!");
```

**Retrieving Top-Rated Drivers:**
```csharp
var topDrivers = driverRatingService.GetTopRatedDrivers(3);

Console.WriteLine("Top Rated Drivers:");
foreach (var driver in topDrivers)
{
    Console.WriteLine(driver);
}
```

**Retrieving Comments for a Driver:**
```csharp
var comments = driverRatingService.GetDriverComments(1001);

Console.WriteLine("Comments for Driver 1001:");
foreach (var comment in comments)
{
    Console.WriteLine($"- {comment}");
}

```


