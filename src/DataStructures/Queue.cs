using System;
using SmartRide.src.DataStructures;

namespace SmartRide.src.DataStructures
{
    public class Queues<T>
    {
        private readonly DoublyLinkedList<T> _list = new();

        public void Enqueue(T item)
        {
            // Add the item to the end of the doubly linked list
            _list.AddLast(item);
        }

        public T Dequeue()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("The Queue is empty");
            }

            // Get the first item from the linked list
            var headData = _list.ToList().First();

            // Remove the head node
            _list.Remove(headData);

            return headData;
        }

        public T Peek()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("The Queue is empty");
            }

            // Return the first item without removing it
            return _list.ToList().First();
        }

        public bool IsEmpty()
        {
            return !_list.ToList().Any();
        }

        public int Count => _list.Count;
    }
}
