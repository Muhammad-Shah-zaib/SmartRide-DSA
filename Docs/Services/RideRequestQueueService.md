# RideRequestsQueueService Documentation

## Class: `RideRequestsQueueService`

<details>
<summary><strong>Purpose</strong></summary>
Manages ride requests in a queue structure, allowing for the addition of new requests and processing of completed rides. This service interacts with the database to persist completed rides.
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
- **Data Transfer Object**: `RideRequestDto`
- **Model**: `RideRequest`

---

### Constructor

#### `RideRequestsQueueService(SmartRideDbContext dbContext)`
- **Parameters**:
  - `dbContext`: An instance of `SmartRideDbContext` used for database interactions.
- **Description**: Initializes the service and sets up an in-memory queue for ride requests.

---

### Properties
#### Private Fields
- **`_dbContext`**
  - **Type**: `SmartRideDbContext`
  - **Description**: Database context for persisting and retrieving ride data.
  
- **`_rideRequests`**
  - **Type**: `Queue<RideRequestDto>`
  - **Description**: In-memory queue storing ride requests.

---

### Methods

#### `void AddRideRequest(RideRequestDto request)`
- **Parameters**:
  - `request`: An instance of `RideRequestDto` containing the details of the ride request.
- **Description**: Adds a new ride request to the in-memory queue.

#### `void RemoveRideRequest()`
- **Description**: Processes the oldest ride request in the queue, converting it into a completed ride and saving it to the database.
- **Steps**:
  1. Dequeues a `RideRequestDto` from `_rideRequests`.
  2. Converts the dequeued DTO to a `RideRequest` model.
  3. Saves the completed ride to the `CompletedRides` table in the database.
  4. Calls `_dbContext.SaveChanges()` to persist the data.

---

### Example Usage

```csharp
using SmartRide.src.Dtos;
using SmartRide.src.Services;

var dbContext = new SmartRideDbContext();
var rideRequestService = new RideRequestsQueueService(dbContext);

// Adding a ride request
var rideRequest = new RideRequestDto
{
    UserId = 1,
    Source = "Downtown",
    Destination = "Uptown",
    Status = "Pending",
    RideTime = DateTime.Now
};
rideRequestService.AddRideRequest(rideRequest);
Console.WriteLine("Ride request added.");

// Processing a ride request
rideRequestService.RemoveRideRequest();
Console.WriteLine("Ride request processed and saved to the database.");
