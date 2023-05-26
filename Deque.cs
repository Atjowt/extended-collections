namespace System.Collections.Extended;

public class Deque<T> : IEnumerable<T>
{
    private T[] _items;
    private readonly T _empty;
    private int _left;
    private int _right;

    /// <summary>
    /// Initializes a new <see cref="Deque{T}"/> with an initial <paramref name="capacity"/>.
    /// </summary>
    /// <param name="capacity">The initial capacity of the <see cref="Deque{T}"/>.</param>
    public Deque(int capacity = 16)
    {
        _items = new T[capacity];
        _empty = (new T[1])[0];

        _left = 0;
        _right = Decrement(_left);
    }

    /// <summary>
    /// Initializes a new <see cref="Deque{T}"/> from a <paramref name="collection"/>.
    /// </summary>
    /// <param name="capacity">The initial capacity of the <see cref="Deque{T}"/>.</param>
    public Deque(IEnumerable<T> collection)
    {
        _items = collection.ToArray();
        _empty = (new T[1])[0];

        _left = 0;
        _right = Decrement(_left);
    }

    /// <summary>
    /// The maximum capacity of the <see cref="Deque{T}"/>.
    /// </summary>
    public int Capacity => _items.Length;

    /// <summary>
    /// The number of items in the <see cref="Deque{T}"/>.
    /// </summary>
    public int Count { get; private set; }

    /// <summary>
    /// Whether or not the <see cref="Deque{T}"/> is empty.
    /// </summary>
    public bool IsEmpty => Count == 0;

    /// <summary>
    /// Whether or not the <see cref="Deque{T}"/> contains any items.
    /// </summary>
    public bool Any => !IsEmpty;

    /// <summary>
    /// Whether or not the <see cref="Deque{T}"/> is full.
    /// </summary>
    public bool IsFull => Count >= Capacity;

    /// <summary>
    /// Peeks at the left-most element in the <see cref="Deque{T}"/> without removing it.
    /// </summary>
    /// <returns>The left-most element in the <see cref="Deque{T}"/>.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public T PeekLeft()
    {
        if (IsEmpty) throw new InvalidOperationException("Deque is empty!");
        return _items[_left];
    }

    /// <summary>
    /// Peeks at the right-most element in the <see cref="Deque{T}"/> without removing it.
    /// </summary>
    /// <returns>The right-most element in the <see cref="Deque{T}"/>.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public T PeekRight()
    {
        if (IsEmpty) throw new InvalidOperationException("Deque is empty!");
        return _items[_right];
    }

    /// <summary>
    /// Removes and returns the the left-most element in the <see cref="Deque{T}"/>.
    /// </summary>
    /// <returns>The left-most element in the <see cref="Deque{T}"/>.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public T DequeueLeft()
    {
        T item = PeekLeft();
        _items[_left] = _empty;
        _left = Increment(_left);
        Count--;
        return item;
    }

    /// <summary>
    /// Removes and returns the the right-most element in the <see cref="Deque{T}"/>.
    /// </summary>
    /// <returns>The right-most element in the <see cref="Deque{T}"/>.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public T DequeueRight()
    {
        T item = PeekRight();
        _items[_right] = _empty;
        _right = Decrement(_right);
        Count--;
        return item;
    }

    /// <summary>
    /// Adds an <paramref name="item"/> to the left of the <see cref="Deque{T}"/>.
    /// May reallocate if <paramref name="reallocate"/> is set to <see langword="true"/>.
    /// </summary>
    /// <param name="item">The item to add to the <see cref="Deque{T}"/>.</param>
    /// <param name="reallocate">Whether or not the <see cref="Deque{T}"/> may automatically reallocate when capacity is reached.</param>
    /// <exception cref="InvalidOperationException"></exception>
    public void EnqueueLeft(T item, bool reallocate = true)
    {
        if (IsFull)
        {
            if (reallocate) Reallocate(2 * Capacity);
            else throw new InvalidOperationException("Deque is full!");
        }
        _left = Decrement(_left);
        _items[_left] = item;
        Count++;
    }

    /// <summary>
    /// Adds an <paramref name="item"/> to the right of the <see cref="Deque{T}"/>.
    /// May reallocate if <paramref name="reallocate"/> is set to <see langword="true"/>.
    /// </summary>
    /// <param name="item">The item to add to the <see cref="Deque{T}"/>.</param>
    /// <param name="reallocate">Whether or not the <see cref="Deque{T}"/> may automatically reallocate when capacity is reached.</param>
    /// <exception cref="InvalidOperationException"></exception>
    public void EnqueueRight(T item, bool reallocate = true)
    {
        if (IsFull)
        {
            if (reallocate) Reallocate(2 * Capacity);
            else throw new InvalidOperationException("Deque is full!");
        }
        _right = Increment(_right);
        _items[_right] = item;
        Count++;
    }

    /// <summary>
    /// Resizes the <see cref="Deque{T}"/> to ensure the specified <paramref name="capacity"/>.
    /// </summary>
    /// <param name="capacity">The new capacity of the <see cref="Deque{T}"/>.</param>
    public void Reallocate(int capacity)
    {
        T[] result = new T[capacity];

        if (Any)
        {
            if (_left <= _right)
            {
                for (int i = _left; i <= _right; i++)
                {
                    result[i] = _items[i];
                }
            }
            else
            {
                for (int i = _left; i < Capacity; i++)
                {
                    result[i] = _items[i];
                }
                for (int i = 0; i <= _right; i++)
                {
                    result[i + Capacity] = _items[i];
                }
            }
        }

        if (_right < _left) _right += Capacity;

        _items = result;
    }

    private int Increment(int index) => index == Capacity - 1 ? 0 : index + 1;

    private int Decrement(int index) => index == 0 ? Capacity - 1 : index - 1;

    public IEnumerator<T> GetEnumerator()
    {
        for (int i = _left; i <= _right; i++)
        {
            yield return _items[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}