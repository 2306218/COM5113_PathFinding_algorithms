using System;

namespace PathFindingAssignment
{
    // Simple stack built on top of our own LinkedList<T>.
    // Used by the DFS algorithm.
    public class StackAdapter<T>
    {
        private LinkedList<T> list = new LinkedList<T>();

        // True if stack is empty
        public bool IsEmpty()
        {
            return list.IsEmpty();
        }

        // Push item onto the top of the stack
        public void Push(T item)
        {
            list.AddFirst(item);
        }

        // Pop item from the top of the stack
        public T Pop()
        {
            return list.RemoveFirst();
        }

        // Look at the top item without removing it
        public T Peek()
        {
            if (list.IsEmpty())
                throw new InvalidOperationException("Stack empty.");

            return list.GetAt(0);
        }
    }
}
