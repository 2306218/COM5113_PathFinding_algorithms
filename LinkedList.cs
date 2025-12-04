using System;

namespace PathFindingAssignment
{
    // Simple generic linked list implementation.
    // This is used to build our own stack, queue and priority queue
    // instead of using the built-in .NET collections.
    public class LinkedList<T>
    {
        // Reference to the first and last nodes of the list
        private LinkedListNode<T> head;
        private LinkedListNode<T> tail;

        // Number of elements currently stored
        public int Count { get; private set; }

        // Checks whether the list is empty
        public bool IsEmpty()
        {
            return Count == 0;
        }

        // Add a new item at the start of the list
        public void AddFirst(T item)
        {
            LinkedListNode<T> node = new LinkedListNode<T>(item);

            // New node points to the old head
            node.Next = head;

            // New node becomes the new head
            head = node;

            // If the list was previously empty, tail also points to this node
            if (tail == null)
                tail = node;

            Count++;
        }

        // Add a new item at the end of the list
        public void AddLast(T item)
        {
            LinkedListNode<T> node = new LinkedListNode<T>(item);

            if (tail == null)
            {
                // Empty list: both head and tail are this node
                head = tail = node;
            }
            else
            {
                // Attach to the end and move tail
                tail.Next = node;
                tail = node;
            }

            Count++;
        }

        // Remove and return the first item in the list
        public T RemoveFirst()
        {
            if (head == null)
                throw new InvalidOperationException("List is empty.");

            T value = head.Value;
            head = head.Next;

            // If head becomes null, the list is now empty
            if (head == null)
                tail = null;

            Count--;
            return value;
        }

        // Remove and return the last item in the list
        public T RemoveLast()
        {
            if (tail == null)
                throw new InvalidOperationException("List is empty.");

            // If there is only one node
            if (head == tail)
            {
                T value = head.Value;
                head = tail = null;
                Count--;
                return value;
            }

            // Otherwise we need to find the node just before the tail
            LinkedListNode<T> current = head;
            while (current.Next != tail)
            {
                current = current.Next;
            }

            T result = tail.Value;
            tail = current;
            tail.Next = null;

            Count--;
            return result;
        }

        // Return the value at a given index (0-based)
        public T GetAt(int index)
        {
            if (index < 0 || index >= Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            LinkedListNode<T> current = head;

            for (int i = 0; i < index; i++)
            {
                current = current.Next;
            }

            return current.Value;
        }

        // Remove all elements from the list
        public void Clear()
        {
            head = null;
            tail = null;
            Count = 0;
        }

        // Inserts an item into the list in sorted order,
        // using the comparison function supplied.
        // Used by the priority queue.
        public void InsertSorted(T item, Comparison<T> comparison)
        {
            LinkedListNode<T> node = new LinkedListNode<T>(item);

            // Empty list: node becomes both head and tail
            if (head == null)
            {
                head = tail = node;
                Count = 1;
                return;
            }

            // If it should go before the current head
            if (comparison(item, head.Value) <= 0)
            {
                node.Next = head;
                head = node;
                Count++;
                return;
            }

            // Otherwise find the correct position in the middle or at the end
            LinkedListNode<T> current = head;
            while (current.Next != null && comparison(item, current.Next.Value) > 0)
            {
                current = current.Next;
            }

            node.Next = current.Next;
            current.Next = node;

            // If we inserted at the very end, update tail
            if (node.Next == null)
                tail = node;

            Count++;
        }
    }
}
