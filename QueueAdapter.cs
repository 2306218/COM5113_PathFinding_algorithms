using System;

namespace PathFindingAssignment
{
    // Simple queue built on top of our own LinkedList<T>.
    // Used by the BFS algorithm.
    public class QueueAdapter<T>
    {
        private LinkedList<T> list = new LinkedList<T>();

        // True if queue has no elements
        public bool IsEmpty()
        {
            return list.IsEmpty();
        }

        // Add item at the back of the queue
        public void Enqueue(T item)
        {
            list.AddLast(item);
        }

        // Remove item from the front of the queue
        public T Dequeue()
        {
            return list.RemoveFirst();
        }

        // Look at the item at the front without removing it
        public T Peek()
        {
            if (list.IsEmpty())
                throw new InvalidOperationException("Queue empty.");

            return list.GetAt(0);
        }
    }
}
