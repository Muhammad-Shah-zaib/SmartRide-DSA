# UserRideHistoryService Documentation

## Class: `UserRideHistoryService`

<details>
<summary><strong>Purpose</strong></summary>
Manages the ride history of users by maintaining an in-memory data structure for efficient access and interacting with the database for persistence.
</details>

---

### Namespace
`SmartRide.src.Services`

---

### Dependencies
- **Namespace**:
  - `SmartRide.src.DataStructures`
  - `SmartRide.src.Dtos`
  - `SmartRide.Models`
- **Database Context**: `SmartRideDbContext`
- **Data Structure**: `HashMap<int, DoublyLinkedList<UserRideHistoryDto>>`
- **Data Transfer Object**: `UserRideHistoryDto`
- **Model**: `Userridehistory`

---

### Constructor

#### `UserRideHistoryService(SmartRideDbContext dbContext)`
- **Parameters**:
  - `dbContext`: An instance of `SmartRideDbContext` used for database interactions.
- **Description**: Initializes the service and populates the in-memory `HashMap` with user ride histories from the database.

---

### Properties
#### Private Fields
- **`_dbContext`**
  - **Type**: `SmartRideDbContext`
  - **Description**: Database context for persisting and retrieving user ride data.

- **`_rideHistoryMap`**
  - **Type**: `HashMap<int, DoublyLinkedList<UserRideHistoryDto>>`
  - **Description**: In-memory `HashMap` storing user ride histories for efficient access.

---

### Methods

#### `void AddRide(UserRideHistoryDto rideDto)`
- **Parameters**:
  - `rideDto`: An instance of `UserRideHistoryDto` containing the details of the ride.
- **Description**: Adds a new ride to the database and updates the in-memory `HashMap`.
- **Steps**:
  1. Converts the `UserRideHistoryDto` to a `Userridehistory` model.
  2. Saves the ride to the `Userridehistories` table in the database.
  3. Updates the `rideDto` with the generated ID.
  4. Adds the ride to the `HashMap` under the user's ID.

#### `IEnumerable<UserRideHistoryDto> GetUserRideHistory(int userId)`
- **Parameters**:
  - `userId`: The ID of the user whose ride history is being requested.
- **Returns**: A collection of `UserRideHistoryDto` objects.
- **Description**: Retrieves all ride history entries for a specified user from the `HashMap`.
- **Throws**: `KeyNotFoundException` if no history exists for the user.

#### `void RemoveRide(int rideId)`
- **Parameters**:
  - `rideId`: The ID of the ride to be removed.
- **Description**: Deletes a ride from the database and removes it from the `HashMap`.
- **Steps**:
  1. Finds the ride in the `Userridehistories` table and removes it.
  2. Calls `_dbContext.SaveChanges()` to persist the removal.
  3. Locates the ride in the `HashMap` and removes it from the corresponding `DoublyLinkedList`.
  4. Deletes the user key from the `HashMap` if no rides remain.

#### `private void LoadRidesFromHistory()`
- **Description**: Loads all user ride histories from the database and populates the in-memory `HashMap`.
- **Steps**:
  1. Queries the `Userridehistories` table for all entries.
  2. Converts each entry to a `UserRideHistoryDto`.
  3. Groups rides by `UserId` and populates the `HashMap` with `DoublyLinkedList<UserRideHistoryDto>`.

---

### Example Usage

```csharp
using SmartRide.src.Dtos;
using SmartRide.src.Services;

var dbContext = new SmartRideDbContext();
var rideHistoryService = new UserRideHistoryService(dbContext);

// Adding a new ride
var newRide = new UserRideHistoryDto
{
    UserId = 1,
    RideStartLocation = "Downtown",
    RideEndLocation = "Uptown",
    RideDistance = 12.5,
    RideDuration = TimeSpan.FromMinutes(30),
    RideDate = DateTime.Now,
    RideCost = 15.75M
};
rideHistoryService.AddRide(newRide);
Console.WriteLine("Ride added successfully.");

// Fetching user ride history
var userRides = rideHistoryService.GetUserRideHistory(1);
foreach (var ride in userRides)
{
    Console.WriteLine($"Ride from {ride.RideStartLocation} to {ride.RideEndLocation} on {ride.RideDate}");
}

// Removing a ride
rideHistoryService.RemoveRide(newRide.Id);
Console.WriteLine("Ride removed successfully.");