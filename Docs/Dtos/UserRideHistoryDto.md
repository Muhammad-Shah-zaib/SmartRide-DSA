# UserRideHistoryDto Documentation

## Class: `UserRideHistoryDto`

<details>
<summary><strong>Purpose</strong></summary>
Represents user ride history data for transfer between layers, mapping closely to the database schema.
</details>

---

### Namespace
`SmartRide.src.Dtos`

---

### Dependencies
- **Namespace**: None (pure data class).

---

### Properties

#### `int Id`
- **Description**: Unique identifier for the ride history record.
- **Mapped To**: `Id` column in the database.

#### `int UserId`
- **Description**: Foreign key linking the ride to the user who completed it.
- **Mapped To**: `Userid` column in the database.

#### `string RideStartLocation`
- **Description**: Starting location of the ride.
- **Mapped To**: `Ridestartlocation` column in the database.

#### `string RideEndLocation`
- **Description**: Ending location of the ride.
- **Mapped To**: `Rideendlocation` column in the database.

#### `decimal RideDistance`
- **Description**: Distance of the ride in kilometers or miles.
- **Mapped To**: `Ridedistance` column in the database.

#### `TimeSpan RideDuration`
- **Description**: Total duration of the ride.
- **Mapped To**: `Rideduration` column in the database.

#### `DateTime RideDate`
- **Description**: Timestamp indicating when the ride occurred.
- **Mapped To**: `Ridedate` column in the database.

#### `decimal RideCost`
- **Description**: Cost of the ride.
- **Mapped To**: `Ridecost` column in the database.

---

### Example Usage

```csharp
using SmartRide.src.Dtos;

var rideHistory = new UserRideHistoryDto
{
    Id = 1,
    UserId = 2,
    RideStartLocation = "City Center",
    RideEndLocation = "Airport",
    RideDistance = 15.2m,
    RideDuration = TimeSpan.FromMinutes(25),
    RideDate = DateTime.Now,
    RideCost = 20.5m
};

Console.WriteLine($"Ride from {rideHistory.RideStartLocation} to {rideHistory.RideEndLocation} cost {rideHistory.RideCost} USD.");
