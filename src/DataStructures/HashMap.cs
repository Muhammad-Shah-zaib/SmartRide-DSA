namespace SmartRide.src.DataStructures;

public class HashMap<TKey, TValue>
{
    private class HashNode(TKey key, TValue value)
    {
        public TKey Key { get; set; } = key;
        public TValue Value { get; set; } = value;
        public HashNode Next { get; set; }

        public HashNode(TKey key, TValue value, HashNode next) : this(key, value)
        {
            Next = next;
        }
    }

    private readonly int _capacity;
    private readonly LinkedList<HashNode>[] _buckets;

    public HashMap(int capacity)
    {
        _capacity = capacity;
        _buckets = new LinkedList<HashNode>[capacity];

        for (int i = 0; i < _capacity; i++)
        {
            _buckets[i] = new LinkedList<HashNode>();
        }
    }

    private int GetBucketIndex(TKey key)
    {
        return Math.Abs(key.GetHashCode()) % _capacity;
    }

    public void Put(TKey key, TValue value)
    {
        int bucketIndex = GetBucketIndex(key);
        foreach (var node in _buckets[bucketIndex])
        {
            if (EqualityComparer<TKey>.Default.Equals(node.Key, key))
            {
                node.Value = value; // Update existing value
                return;
            }
        }

        // Add new node if key not found
        _buckets[bucketIndex].AddLast(new HashNode(key, value));
    }

    public TValue? Get(TKey key)
    {
        int bucketIndex = GetBucketIndex(key);

        foreach (var node in _buckets[bucketIndex])
        {
            if (EqualityComparer<TKey>.Default.Equals(node.Key, key))
            {
                return node.Value;
            }
        }

        return default; // return null or default
    }


    public bool Remove(TKey key)
    {
        int bucketIndex = GetBucketIndex(key);
        var bucket = _buckets[bucketIndex];

        foreach (var node in bucket)
        {
            if (EqualityComparer<TKey>.Default.Equals(node.Key, key))
            {
                bucket.Remove(node);
                return true;
            }
        }

        return false; // Key not found
    }

    public bool ContainsKey(TKey key)
    {
        int bucketIndex = GetBucketIndex(key);
        foreach (var node in _buckets[bucketIndex])
        {
            if (EqualityComparer<TKey>.Default.Equals(node.Key, key))
            {
                return true;
            }
        }

        return false;
    }

    public override string ToString()
    {
        var result = new List<string>();
        foreach (var bucket in _buckets)
        {
            foreach (var node in bucket)
            {
                result.Add($"{node.Key} : {node.Value}");
            }
        }
        return string.Join(Environment.NewLine, result);
    }
}