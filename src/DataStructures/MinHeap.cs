namespace SmartRide.src.DataStructures;
using System;
using System.Collections.Generic;

public class MinHeap<T> where T : IComparable<T>
{
    private readonly List<(T, double)> _heap;  // Tuple of (node, weight)

    public MinHeap()
    {
        _heap = new List<(T, double)>();
    }

    public int Count => _heap.Count;

    public void Insert(T item, double weight)
    {
        _heap.Add((item, weight));
        HeapifyUp(_heap.Count - 1);
    }

    public (T, double) ExtractMin()
    {
        if (_heap.Count == 0) throw new InvalidOperationException("Heap is empty.");

        var min = _heap[0];
        _heap[0] = _heap[^1];
        _heap.RemoveAt(_heap.Count - 1);
        HeapifyDown(0);

        return min;
    }

    public (T, double) Peek()
    {
        if (_heap.Count == 0) throw new InvalidOperationException("Heap is empty.");
        return _heap[0];
    }

    private void HeapifyUp(int index)
    {
        while (index > 0)
        {
            int parent = (index - 1) / 2;
            if (_heap[index].Item2 >= _heap[parent].Item2) break;

            Swap(index, parent);
            index = parent;
        }
    }

    private void HeapifyDown(int index)
    {
        int leftChild, rightChild, smallest;

        while (true)
        {
            leftChild = 2 * index + 1;
            rightChild = 2 * index + 2;
            smallest = index;

            if (leftChild < _heap.Count && _heap[leftChild].Item2 < _heap[smallest].Item2)
                smallest = leftChild;

            if (rightChild < _heap.Count && _heap[rightChild].Item2 < _heap[smallest].Item2)
                smallest = rightChild;

            if (smallest == index) break;

            Swap(index, smallest);
            index = smallest;
        }
    }

    private void Swap(int i, int j)
    {
        (_heap[i], _heap[j]) = (_heap[j], _heap[i]);
    }

    public List<(T, double)> ToList() => new(_heap);
}


    
