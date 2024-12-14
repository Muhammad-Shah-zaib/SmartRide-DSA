# UserService Documentation

## Class: `UserService`

<details>
<summary><strong>Purpose</strong></summary>
Manages all user-related operations such as adding, retrieving, and removing users.
</details>

---

### Constructor: `UserService(SmartRideDbContext context)`

<details>
<summary><strong>Summary</strong></summary>
Initializes the `UserService` with the database context and loads users into the `HashMap`.
</details>

**Parameters**:
- `context` (SmartRideDbContext): The database context used for accessing user data.

---

### Method: `LoadUsersFromDb`

<details>
<summary><strong>Summary</strong></summary>
Loads all users from the database into the `HashMap` for efficient retrieval.
</details>

---

### Method: `AddUser(UserDto user)`

<details>
<summary><strong>Summary</strong></summary>
Adds a new user to the database and the `HashMap`.
</details>

**Parameters**:
- `user` (UserDto): The user data to be added.

**Exceptions**:
- `ArgumentException`: Thrown if the email has already been used.

---

### Method: `GetUser(string email)`

<details>
<summary><strong>Summary</strong></summary>
Retrieves a user by their email address.
</details>

**Parameters**:
- `email` (string): The email address of the user to retrieve.

**Returns**:
- `UserDto`: The user data.

---

### Method: `RemoveUser(string email)`

<details>
<summary><strong>Summary</strong></summary>
Removes a user from the database and the `HashMap`.
</details>

**Parameters**:
- `email` (string): The email address of the user to remove.

**Exceptions**:
- `Exception`: Thrown if the user is not found.
