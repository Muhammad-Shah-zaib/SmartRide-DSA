using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRide.src.DataStructures
{
    public class PriorityQueues<T> where T : IComparable<T>
    {
        private readonly MinHeap<T> _minHeap;

        public PriorityQueues()
        {
            _minHeap = new MinHeap<T>();
        }

        public int Count => _minHeap.Count;

        // Insert an item with its weight
        public void Enqueue(T item, double weight)
        {
            _minHeap.Insert(item, weight);
        }

        // Extract the item with the minimum weight (highest priority)
        public (T, double) Dequeue()
        {
            if (_minHeap.Count == 0)
                throw new InvalidOperationException("The priority queue is empty.");

            return _minHeap.ExtractMin();
        }

        //getting the (highest priority) without removing it
        public (T, double) Peek()
        {
            return _minHeap.Peek();
        }

        public List<(T, double)> ToList()
        {
            return _minHeap.ToList();
        }
    }
}

