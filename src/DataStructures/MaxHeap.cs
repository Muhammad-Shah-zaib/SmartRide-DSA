namespace SmartRide.src.DataStructures;

public class MaxHeap<T> where T : IComparable<T>
{
    private readonly List<T> _heap;

    public MaxHeap()
    {
        _heap = [];
    }

    public int Count => _heap.Count;

    public void Insert(T item)
    {
        _heap.Add(item);
        HeapifyUp(_heap.Count - 1);
    }

    public T ExtractMax()
    {
        if (_heap.Count == 0) throw new InvalidOperationException("Heap is empty.");

        var max = _heap[0];
        _heap[0] = _heap[^1];
        _heap.RemoveAt(_heap.Count - 1);
        HeapifyDown(0);

        return max;
    }

    public T Peek()
    {
        if (_heap.Count == 0) throw new InvalidOperationException("Heap is empty.");
        return _heap[0];
    }

    private void HeapifyUp(int index)
    {
        while (index > 0)
        {
            int parent = (index - 1) / 2;
            if (_heap[index].CompareTo(_heap[parent]) <= 0) break;

            Swap(index, parent);
            index = parent;
        }
    }

    private void HeapifyDown(int index)
    {
        int leftChild, rightChild, largest;

        while (true)
        {
            leftChild = 2 * index + 1;
            rightChild = 2 * index + 2;
            largest = index;

            if (leftChild < _heap.Count && _heap[leftChild].CompareTo(_heap[largest]) > 0)
                largest = leftChild;

            if (rightChild < _heap.Count && _heap[rightChild].CompareTo(_heap[largest]) > 0)
                largest = rightChild;

            if (largest == index) break;

            Swap(index, largest);
            index = largest;
        }
    }

    private void Swap(int i, int j)
    {
        (_heap[i], _heap[j]) = (_heap[j], _heap[i]);
    }

    public List<T> ToList() => new(_heap);
}
