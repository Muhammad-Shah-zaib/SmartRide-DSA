# HashMap Documentation

## Class: `HashMap<TKey, TValue>`

<details>
<summary><strong>Purpose</strong></summary>
A custom hash map implementation for efficient storage and retrieval of data.
</details>

---

### Constructor: `HashMap(int capacity)`

<details>
<summary><strong>Summary</strong></summary>
Initializes a new instance of the `HashMap` class with a specified capacity.
</details>

**Parameters**:
- `capacity` (int): The maximum number of elements the hash map can hold.

---

### Method: `Put(TKey key, TValue value)`

<details>
<summary><strong>Summary</strong></summary>
Adds a key-value pair to the hash map.
</details>

**Parameters**:
- `key` (TKey): The key to associate with the value.
- `value` (TValue): The value to store.

**Exceptions**:
- Throws an exception if the key already exists.

---

### Method: `Get(TKey key)`

<details>
<summary><strong>Summary</strong></summary>
Retrieves the value associated with the specified key.
</details>

**Parameters**:
- `key` (TKey): The key of the value to retrieve.

**Returns**:
- `TValue`: The value associated with the key.

**Exceptions**:
- Throws an exception if the key is not found.

---

### Method: `Remove(TKey key)`

<details>
<summary><strong>Summary</strong></summary>
Removes a key-value pair from the hash map.
</details>

**Parameters**:
- `key` (TKey): The key of the value to remove.

**Exceptions**:
- Throws an exception if the key is not found.

---

### Method: `ContainsKey(TKey key)`

<details>
<summary><strong>Summary</strong></summary>
Checks if the hash map contains the specified key.
</details>

**Parameters**:
- `key` (TKey): The key to check.

**Returns**:
- `bool`: `true` if the key exists; otherwise, `false`.
