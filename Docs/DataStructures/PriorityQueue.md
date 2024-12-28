﻿# PriorityQueues Class Documentation

## Namespace
`SmartRide.src.DataStructures`

## Description
The `PriorityQueues<T>` class is a generic implementation of a priority queue, using a min-heap as its underlying data structure. It allows enqueuing items with associated weights and provides efficient operations to retrieve and manipulate items based on their priority (weight).

---

## Type Parameters
- **`T`**: The type of items stored in the priority queue. Must implement the `IComparable<T>` interface.

---

## Constructors
### `PriorityQueues()`
Initializes a new instance of the `PriorityQueues<T>` class.

---

## Properties
### `Count`
- **Type**: `int`
- **Description**: Gets the number of elements currently in the priority queue.

---

## Methods
### `Enqueue(T item, double weight)`
- **Description**: Adds an item to the priority queue with the specified weight.
- **Parameters**:
  - `T item`: The item to enqueue.
  - `double weight`: The weight associated with the item (lower weight indicates higher priority).

---

### `Dequeue()`
- **Description**: Removes and returns the item with the minimum weight (highest priority) from the priority queue.
- **Returns**: `(T, double)` - A tuple containing the item and its weight.
- **Exceptions**: Throws `InvalidOperationException` if the queue is empty.

---

### `Peek()`
- **Description**: Retrieves the item with the minimum weight (highest priority) without removing it from the queue.
- **Returns**: `(T, double)` - A tuple containing the item and its weight.

---

### `ToList()`
- **Description**: Converts the contents of the priority queue to a list.
- **Returns**: `List<(T, double)>` - A list of tuples, where each tuple contains an item and its weight.

---

## Example Usage
```csharp
var priorityQueue = new PriorityQueues<string>();

// Enqueue items with weights
priorityQueue.Enqueue("Task A", 3.0);
priorityQueue.Enqueue("Task B", 1.5);
priorityQueue.Enqueue("Task C", 2.0);

// Peek the highest-priority item
var top = priorityQueue.Peek();
Console.WriteLine($"Highest priority: {top.Item1}, Weight: {top.Item2}");

// Dequeue the highest-priority item
var dequeued = priorityQueue.Dequeue();
Console.WriteLine($"Dequeued: {dequeued.Item1}, Weight: {dequeued.Item2}");

// Get all items as a list
var items = priorityQueue.ToList();
foreach (var item in items)
{
    Console.WriteLine($"Item: {item.Item1}, Weight: {item.Item2}");
}
```