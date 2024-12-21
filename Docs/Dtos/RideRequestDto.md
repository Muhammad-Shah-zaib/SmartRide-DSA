# RideRequestDto Documentation

## Class: `RideRequestDto`

<details>
<summary><strong>Purpose</strong></summary>
Represents the details of a ride request in the SmartRide application. This Data Transfer Object (DTO) facilitates transferring ride request data between application layers.
</details>

---

### Namespace
`SmartRide.src.Dtos`

---

### Properties

<details>
<summary><strong>Id</strong></summary>
- **Type**: `int`
- **Description**: The unique identifier for the ride request.
</details>

<details>
<summary><strong>UserId</strong></summary>
- **Type**: `int`
- **Description**: The unique identifier of the user associated with the ride request.
</details>

<details>
<summary><strong>Source</strong></summary>
- **Type**: `string`
- **Description**: The starting location of the ride request.
</details>

<details>
<summary><strong>Destination</strong></summary>
- **Type**: `string`
- **Description**: The destination of the ride request.
</details>

<details>
<summary><strong>Status</strong></summary>
- **Type**: `string`
- **Description**: The current status of the ride request (e.g., "Pending", "Completed").
</details>

<details>
<summary><strong>RideTime</strong></summary>
- **Type**: `DateTime`
- **Description**: The date and time the ride is scheduled or was requested.
</details>

---

