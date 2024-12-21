# Queues\<T> Documentation

## Class: `Queues<T>`

<details>
<summary><strong>Purpose</strong></summary>
A custom implementation of a generic doubly linked list-based queue data structure for managing elements in a FIFO (First In, First Out) manner.
</details>

---

### Namespace
`SmartRide.src.DataStructures`

---

### Type Parameters
- **`T`**: The type of elements to be stored in the queue.

---

### Properties

<details>
<summary><strong>_head</strong></summary>
- **Type**: `QueueNode?`
- **Access**: Private
- **Description**: Points to the first node in the queue.
</details>

<details>
<summary><strong>_tail</strong></summary>
- **Type**: `QueueNode?`
- **Access**: Private
- **Description**: Points to the last node in the queue.
</details>

<details>
<summary><strong>_count</strong></summary>
- **Type**: `int`
- **Access**: Private
- **Description**: Tracks the number of elements in the queue.
</details>

---

### Nested Class: `QueueNode`
Represents a single node in the queue.

#### Properties
- **`Data`**: The data of type `T` stored in the node.
- **`Next`**: Points to the next node in the queue.
- **`Previous`**: Points to the previous node in the queue.

---

### Methods

#### `void Enqueue(T node)`
- **Description**: Adds a new element to the back of the queue.
- **Parameters**:
  - `node`: The data of type `T` to be added to the queue.
- **Behavior**:
  - If the queue is empty, the new node becomes both the `_head` and `_tail`.
  - Otherwise, the new node is appended to the `_tail`.

---

#### `T Dequeue()`
- **Description**: Removes and returns the element at the front of the queue.
- **Returns**: The data of type `T` from the front of the queue.
- **Exceptions**:
  - Throws `InvalidOperationException` if the queue is empty.
- **Behavior**:
  - If the queue has only one element, both `_head` and `_tail` are reset to `null`.
  - Otherwise, `_head` moves to the next node.

---

#### `T Peek()`
- **Description**: Returns the element at the front of the queue without removing it.
- **Returns**: The data of type `T` at the front of the queue.
- **Exceptions**:
  - Throws `InvalidOperationException` if the queue is empty.

---

#### `bool IsEmpty()`
- **Description**: Checks whether the queue is empty.
- **Returns**: 
  - `true` if the queue is empty.
  - `false` otherwise.

---

### Example Usage

```csharp
using SmartRide.src.DataStructures;

var queue = new Queues<int>();

// Adding elements to the queue
queue.Enqueue(10);
queue.Enqueue(20);
queue.Enqueue(30);

Console.WriteLine($"Front Element: {queue.Peek()}"); // Output: Front Element: 10

// Removing elements from the queue
Console.WriteLine($"Dequeued: {queue.Dequeue()}"); // Output: Dequeued: 10

Console.WriteLine($"Front Element After Dequeue: {queue.Peek()}"); // Output: Front Element After Dequeue: 20

// Checking if the queue is empty
Console.WriteLine($"Is Queue Empty? {queue.IsEmpty()}"); // Output: Is Queue Empty? False
