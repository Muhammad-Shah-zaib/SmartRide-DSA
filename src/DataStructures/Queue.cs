using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace SmartRide.src.DataStructures
{
    public class Queues<T>
    {
        //Tracks the front and back of the Queue
        private QueueNode? _head;
        private QueueNode? _tail;
        private int _count;//Count of the requests
        
        public class QueueNode
        {
            public T Data;
            public QueueNode Next;
            public QueueNode Previous;
        }

        public void Enqueue(T node)
        {
            var newnode = new QueueNode();
            newnode.Data = node;
            if (_head == null)
            {
                _head=_tail = newnode;
            }
            else { 
                _tail.Next = newnode;
                newnode.Previous = _tail;
                _tail = newnode;
            }
            _count++;
            
        }
        public T Dequeue()
        {
            if (IsEmpty())
            {
                throw new InvalidOperationException("The Queue is empty");
            }

            var removedData = _head.Data;

            if (_head == _tail)
            {
                // If there's only one element, reset both head and tail
                _head = _tail = null;
            }
            else
            {
                // Move the head to the next node
                _head = _head.Next;
                _head.Previous = null;
            }

            _count--;
            return removedData;
        }

        


        public T Peek()
        {
            
            if(IsEmpty())
            {
                throw new InvalidOperationException("The Queue is empty");

            }

            var top_data = _head.Data;
            return top_data;
        }

        public bool IsEmpty()
        {
            return _head == null;
        }
    }
}