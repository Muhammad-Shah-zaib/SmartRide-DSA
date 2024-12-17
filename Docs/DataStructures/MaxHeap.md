# MaxHeap Documentation

## Class: `MaxHeap<T>`

<details>
<summary><strong>Purpose</strong></summary>
The `MaxHeap` is a custom implementation of a max-priority heap. It allows for efficient retrieval and management of the largest element, ensuring logarithmic time complexity for insertion and removal operations.
</details>

---

### Generic Type Parameters:
- **`T`**: The type of elements stored in the heap. `T` must implement the `IComparable<T>` interface for comparisons.

---

### Constructor: `MaxHeap()`

<details>
<summary><strong>Summary</strong></summary>
Initializes an empty `MaxHeap`.
</details>

---

### Method: `Insert(T item)`

<details>
<summary><strong>Summary</strong></summary>
Adds a new element to the heap while maintaining the max-heap property.
</details>

**Parameters**:
- `item` (`T`): The element to be inserted into the heap.

**Time Complexity**: `O(log n)`

---

### Method: `ExtractMax()`

<details>
<summary><strong>Summary</strong></summary>
Removes and returns the maximum element (root) from the heap.
</details>

**Returns**:
- `T`: The maximum element in the heap.

**Exceptions**:
- `InvalidOperationException`: Thrown if the heap is empty.

**Time Complexity**: `O(log n)`

---

### Method: `Peek()`

<details>
<summary><strong>Summary</strong></summary>
Returns the maximum element in the heap without removing it.
</details>

**Returns**:
- `T`: The maximum element.

**Exceptions**:
- `InvalidOperationException`: Thrown if the heap is empty.

**Time Complexity**: `O(1)`

---

### Method: `ToList()`

<details>
<summary><strong>Summary</strong></summary>
Returns the heap as a list.
</details>

**Returns**:
- `List<T>`: A list representation of the heap.

---

### Example Usage:

```csharp
var maxHeap = new MaxHeap<int>();

maxHeap.Insert(10);
maxHeap.Insert(20);
maxHeap.Insert(15);

Console.WriteLine(maxHeap.ExtractMax()); // Output: 20
Console.WriteLine(maxHeap.Peek());       // Output: 15
