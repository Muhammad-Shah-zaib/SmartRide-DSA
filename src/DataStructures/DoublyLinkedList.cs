namespace SmartRide.src.DataStructures;

public class DoublyLinkedList<T>
{
    public class Node
    {
        public T Data { get; set; }
        public Node? Next { get; set; }
        public Node? Prev { get; set; }

        public Node(T data)
        {
            Data = data;
        }
    }

    private Node? _head;
    private Node? _tail;
    private int _count;

    public int Count => _count;

    public void AddLast(T item)
    {
        var newNode = new Node(item);
        if (_tail == null)
        {
            _head = _tail = newNode;
        }
        else
        {
            _tail.Next = newNode;
            newNode.Prev = _tail;
            _tail = newNode;
        }
        _count++;
    }

    public void Remove(Node node)
    {
        if (node.Prev != null)
        {
            node.Prev.Next = node.Next;
        }
        else
        {
            _head = node.Next;
        }

        if (node.Next != null)
        {
            node.Next.Prev = node.Prev;
        }
        else
        {
            _tail = node.Prev;
        }

        _count--;
    }

    public IEnumerable<T> ToList()
    {
        var current = _head;
        while (current != null)
        {
            yield return current.Data;
            current = current.Next;
        }
    }

    public bool Remove(T item)
    {
        var current = _head;
        while (current != null)
        {
            if (EqualityComparer<T>.Default.Equals(current.Data, item))
            {
                Remove(current);
                return true;
            }
            current = current.Next;
        }
        return false;
    }
}
