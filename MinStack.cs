namespace System.Collections.Extended;

public class MinStack<T> : Stack<T> where T : IComparable<T>
{
    private readonly Stack<T> _min;

    public MinStack()
    {
        _min = new();
    }

    public T Min => _min.Peek();

    public new void Push(T item)
    {
        base.Push(item);
        if(_min.Count > 0)
        {
            T current = _min.Peek();
            T smaller = item.CompareTo(current) < 0 ? item : current;
            _min.Push(smaller);
        }
        else
        {
            _min.Push(item);
        }
    }

    public new T Pop()
    {
        _min.Pop();
        return base.Pop();
    }

}