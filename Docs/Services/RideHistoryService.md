# RideHistoryService Documentation

## Class: `RideHistoryService`

<details>
<summary><strong>Purpose</strong></summary>
Handles all ride history-related operations, including adding rides, retrieving a user's ride history, and removing rides. User ride histories are stored in a `HashMap` where the key is the `UserId`, and the value is a `DoublyLinkedList` of ride records for efficient traversal.
</details>

---

### Constructor: `RideHistoryService(SmartRideDbContext dbContext)`

<details>
<summary><strong>Summary</strong></summary>
Initializes the `RideHistoryService` with the database context and loads ride history data into a `HashMap` with `DoublyLinkedList` for each user.
</details>

**Parameters**:
- `dbContext` (SmartRideDbContext): The database context used for accessing ride history data.

---

### Method: `LoadRideHistoryFromDb`

<details>
<summary><strong>Summary</strong></summary>
Loads all ride history records from the database into the `HashMap`. Each user’s ride history is stored in a `DoublyLinkedList` for efficient addition, removal, and traversal.
</details>

---

### Method: `AddRide(RideHistoryDto ride)`

<details>
<summary><strong>Summary</strong></summary>
Adds a new ride history record to the database and updates the `HashMap`. The record is appended to the end of the user's `DoublyLinkedList`.
</details>

**Parameters**:
- `ride` (RideHistoryDto): The ride history data to be added.

**Exceptions**:
- `Exception`: Thrown if any database issue occurs while saving the ride.

---

### Method: `GetUserRideHistory(int userId)`

<details>
<summary><strong>Summary</strong></summary>
Retrieves all ride history records for a given user. The records are traversed from the user's `DoublyLinkedList` and returned as an enumerable collection.
</details>

**Parameters**:
- `userId` (int): The unique ID of the user whose ride history is to be retrieved.

**Returns**:
- `IEnumerable<RideHistoryDto>`: A collection of ride history records for the specified user.

**Exceptions**:
- `KeyNotFoundException`: Thrown if no ride history is found for the specified user.

---

### Method: `RemoveRide(int rideId)`

<details>
<summary><strong>Summary</strong></summary>
Removes a specific ride history record from the database and updates the user's `DoublyLinkedList` in the `HashMap`. If the user has no remaining rides after removal, their entry is removed from the `HashMap`.
</details>

**Parameters**:
- `rideId` (int): The unique ID of the ride history record to be removed.

**Exceptions**:
- `Exception`: Thrown if the ride history record is not found in the database.

---

### Data Structures Used

<details>
<summary><strong>HashMap</strong></summary>
The `HashMap<int, DoublyLinkedList<RideHistoryDto>>` is used to store ride histories for each user. Each `UserId` acts as the key, and the value is a `DoublyLinkedList` containing the user's ride history records.
</details>

<details>
<summary><strong>DoublyLinkedList</strong></summary>
A custom `DoublyLinkedList` implementation is used to store and manage ride history records for efficient traversal and removal.
</details>

---

### Example Usage

**Adding a Ride**:
```csharp
var rideHistoryService = new RideHistoryService(dbContext);

var newRide = new RideHistoryDto
{
    UserId = <user_id_>,
    Source = "h12",
    Destination = "g12",
    RideDate = DateTime.Now,
    Log = "Smooth ride, no issues." // can be left empty
};

rideHistoryService.AddRide(newRide);
Console.WriteLine("Ride added successfully!");
```

**Retrieving user ride history**
```csharp
var userRides = rideHistoryService.GetUserRideHistory(1);

foreach (var ride in userRides)
{
    Console.WriteLine($"Ride from {ride.Source} to {ride.Destination} on {ride.RideDate}");
}
```

**Removing Ride**
```csharp
rideHistoryService.RemoveRide(rideId: 101);
Console.WriteLine("Ride removed successfully!");
```