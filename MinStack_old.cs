namespace System.Collections.Extended;

public class MinStack<T> where T : IComparable<T>
{
    private class MinPtr
    {
        public readonly int index;
        public readonly MinPtr? next;
        public MinPtr(int index, MinPtr? next = null)
        {
            this.index = index;
            this.next = next;
        }
    }

    private readonly T[] _items;
    private readonly T _empty;
    private MinPtr? _min;

    public MinStack(int capacity = 16)
    {
        _items = new T[capacity];
        _empty = (new T[1])[0];
        _min = null;
    }

    public int Capacity => _items.Length;
    public int Count { get; private set; }
    public bool IsEmpty => Count == 0;
    public bool Any => !IsEmpty;
    public bool IsFull => Count >= Capacity;

    public T Peek()
    {
        if (IsEmpty) throw new InvalidOperationException("Stack is empty!");
        return _items[Count - 1];
    }

    public void Push(T item)
    {
        if (IsFull) throw new InvalidOperationException("Stack is full!");
        int i = Count;
        _items[i] = item;
        Count++;
        if(_min is null || item.CompareTo(_items[_min.index]) < 0)
        {
            _min = new(i, _min);
        }
    }

    public T Pop()
    {
        T item = Peek();
        int i = Count - 1;
        _items[i] = _empty;
        Count--;
        if(_min is not null && _min.index == i)
        {
            _min = _min.next;
        }
        return item;
    }

    public T GetMin()
    {
        if (_min is null) throw new InvalidOperationException("Stack has no min!");
        return _items[_min.index];
    }

}