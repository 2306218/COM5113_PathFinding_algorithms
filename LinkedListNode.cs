namespace PathFindingAssignment
{
    // Node class used internally by LinkedList<T>.
    // Each node stores a value and a reference to the next node.
    public class LinkedListNode<T>
    {
        public T Value;
        public LinkedListNode<T> Next;

        public LinkedListNode(T value)
        {
            Value = value;
        }
    }
}
