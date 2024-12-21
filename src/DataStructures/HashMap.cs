namespace SmartRide.src.DataStructures
{
    public class HashMap<TKey, TValue>
    {
        private class HashNode
        {
            public TKey Key { get; set; }
            public TValue Value { get; set; }
            public HashNode? Next { get; set; }

            public HashNode(TKey key, TValue value, HashNode? next = null)
            {
                Key = key;
                Value = value;
                Next = next;
            }
        }

        private readonly int _capacity;
        private readonly LinkedList<HashNode>[] _buckets;
        private int _size;

        public HashMap(int capacity = 100)
        {
            _capacity = capacity;
            _buckets = new LinkedList<HashNode>[capacity];
            _size = 0;

            for (int i = 0; i < _capacity; i++)
            {
                _buckets[i] = new LinkedList<HashNode>();
            }
        }

        private int GetBucketIndex(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key), "Key cannot be null");

            return Math.Abs(key.GetHashCode()) % _capacity;
        }

        public void Put(TKey key, TValue value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key), "Key cannot be null");

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
            _size++;
        }

        public TValue? Get(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key), "Key cannot be null");

            int bucketIndex = GetBucketIndex(key);

            foreach (var node in _buckets[bucketIndex])
            {
                if (EqualityComparer<TKey>.Default.Equals(node.Key, key))
                {
                    return node.Value;
                }
            }

            return default; // Key not found
        }

        public bool Remove(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key), "Key cannot be null");

            int bucketIndex = GetBucketIndex(key);
            var bucket = _buckets[bucketIndex];

            foreach (var node in bucket)
            {
                if (EqualityComparer<TKey>.Default.Equals(node.Key, key))
                {
                    bucket.Remove(node);
                    _size--;
                    return true;
                }
            }

            return false; // Key not found
        }

        public bool ContainsKey(TKey key)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key), "Key cannot be null");

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

        public List<TKey> Keys()
        {
            var keys = new List<TKey>();
            foreach (var bucket in _buckets)
            {
                foreach (var node in bucket)
                {
                    keys.Add(node.Key);
                }
            }
            return keys;
        }

        public List<TValue> Values()
        {
            var values = new List<TValue>();
            foreach (var bucket in _buckets)
            {
                foreach (var node in bucket)
                {
                    values.Add(node.Value);
                }
            }
            return values;
        }

        public List<(TKey Key, TValue Value)> ToList()
        {
            var list = new List<(TKey Key, TValue Value)>();
            foreach (var bucket in _buckets)
            {
                foreach (var node in bucket)
                {
                    list.Add((node.Key, node.Value));
                }
            }
            return list;
        }

        public int Size()
        {
            return _size;
        }

        public void Clear()
        {
            for (int i = 0; i < _capacity; i++)
            {
                _buckets[i].Clear();
            }
            _size = 0;
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
}
