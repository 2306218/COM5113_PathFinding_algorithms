namespace PathFindingAssignment
{
    // Simple priority queue built on top of our LinkedList<T>.
    // Items are kept in sorted order according to the comparison function.
    public class PriorityQueueAdapter<T>
    {
        private LinkedList<T> list = new LinkedList<T>();
        private readonly System.Comparison<T> comparison;

        public PriorityQueueAdapter(System.Comparison<T> comparison)
        {
            this.comparison = comparison;
        }

        // True if there are no elements in the queue
        public bool IsEmpty()
        {
            return list.IsEmpty();
        }

        // Inserts an item into the list using the comparison to keep it ordered
        public void Enqueue(T item)
        {
            list.InsertSorted(item, comparison);
        }

        // Removes and returns the item with the highest priority (front of the list)
        public T Dequeue()
        {
            return list.RemoveFirst();
        }
    }
}
