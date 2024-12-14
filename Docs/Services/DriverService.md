# DriverService Documentation

## Class: `DriverService`

<details>
<summary><strong>Purpose</strong></summary>
Handles all driver-related operations such as adding, retrieving, and removing drivers.
</details>

---

### Constructor: `DriverService(SmartRideDbContext context)`

<details>
<summary><strong>Summary</strong></summary>
Initializes the `DriverService` with the database context and loads drivers into the `HashMap`.
</details>

**Parameters**:
- `context` (SmartRideDbContext): The database context used for accessing driver data.

---

### Method: `LoadDriversFromDb`

<details>
<summary><strong>Summary</strong></summary>
Loads all drivers from the database into the `HashMap` for efficient retrieval.
</details>

---

### Method: `AddDriver(DriverDto driver)`

<details>
<summary><strong>Summary</strong></summary>
Adds a new driver to the database and the `HashMap`.
</details>

**Parameters**:
- `driver` (DriverDto): The driver data to be added.

**Exceptions**:
- `Exception`: Thrown if a driver with the same ID already exists.

---

### Method: `GetDriver(int id)`

<details>
<summary><strong>Summary</strong></summary>
Retrieves a driver by their unique ID.
</details>

**Parameters**:
- `id` (int): The unique ID of the driver to retrieve.

**Returns**:
- `DriverDto`: The driver data.

---

### Method: `RemoveDriver(int id)`

<details>
<summary><strong>Summary</strong></summary>
Removes a driver from the database and the `HashMap`.
</details>

**Parameters**:
- `id` (int): The unique ID of the driver to remove.

**Exceptions**:
- `Exception`: Thrown if the driver is not found.
