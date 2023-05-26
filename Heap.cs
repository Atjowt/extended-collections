namespace System.Collections.Extended;

public class Heap<T> where T : IComparable<T>
{
    private T[] _items;
    private readonly T _empty;

    /// <summary>
    /// Initializes a new <see cref="Heap{T}"/> with an initial <paramref name="capacity"/>.
    /// </summary>
    /// <param name="capacity">The initial capacity of the <see cref="Heap{T}"/>.</param>
    public Heap(int capacity = 16)
    {
        _items = new T[capacity];
        _empty = (new T[1])[0];
        Comparison = (T a, T b) => a.CompareTo(b);
    }

    /// <summary>
    /// The comparison function to use during heapification.
    /// <para>
    /// To create a max heap, one could do the following:
    /// <code>
    /// Comparison = (T a, T b) => -a.CompareTo(b);
    /// </code>
    /// </para>
    /// </summary>
    public Comparison<T> Comparison { get; set; }

    /// <summary>
    /// The maximum capacity of the <see cref="Heap{T}"/>.
    /// </summary>
    public int Capacity => _items.Length;

    /// <summary>
    /// The number of items in the <see cref="Heap{T}"/>.
    /// </summary>
    public int Count { get; private set; }

    /// <summary>
    /// Whether or not the <see cref="Heap{T}"/> is empty.
    /// </summary>
    public bool IsEmpty => Count == 0;

    /// <summary>
    /// Whether or not the <see cref="Heap{T}"/> contains any items.
    /// </summary>
    public bool Any => !IsEmpty;

    /// <summary>
    /// Whether or not the <see cref="Heap{T}"/> is full.
    /// </summary>
    public bool IsFull => Count >= Capacity;

    /// <summary>
    /// Peeks at the top element of the <see cref="Heap{T}"/> without removing it.
    /// </summary>
    /// <returns>The top element of the <see cref="Heap{T}"/>.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public T Peek()
    {
        if (IsEmpty) throw new InvalidOperationException("Heap is empty!");
        return _items[0];
    }

    /// <summary>
    /// Removes and returns the the top element of the <see cref="Heap{T}"/>.
    /// </summary>
    /// <returns>The top element of the <see cref="Heap{T}"/>.</returns>
    /// <exception cref="InvalidOperationException"></exception>
    public T Pop()
    {
        T item = Peek();
        _items[Count - 1] = _empty;
        _items[0] = _items[Count - 1];
        Count--;
        HeapifyDown();
        return item;
    }

    /// <summary>
    /// Pushes an <paramref name="item"/> to the <see cref="Heap{T}"/>.
    /// May reallocate if <paramref name="reallocate"/> is set to <see langword="true"/>.
    /// </summary>
    /// <param name="item">The item to push to the <see cref="Heap{T}"/>.</param>
    /// <param name="reallocate">Whether or not the <see cref="Heap{T}"/> may automatically reallocate when capacity is reached.</param>
    /// <exception cref="InvalidOperationException"></exception>
    public void Push(T item, bool reallocate = true)
    {
        if (IsFull)
        {
            if (reallocate) Reallocate(2 * Capacity);
            else throw new InvalidOperationException("Heap is full!");
        }
        _items[Count] = item;
        Count++;
        HeapifyUp();
    }

    /// <summary>
    /// Resizes the <see cref="Heap{T}"/> to ensure the specified <paramref name="capacity"/>.
    /// </summary>
    /// <param name="capacity">The new capacity of the <see cref="Heap{T}"/>.</param>
    public void Reallocate(int capacity)
    {
        Array.Resize(ref _items, capacity);
    }

    private void HeapifyDown()
    {
        int index = 0;
        T item = _items[index];
        while (true)
        {
            int leftChildIndex = 2 * index + 1;
            int rightChildIndex = 2 * index + 2;

            if (leftChildIndex >= Count)
            {
                break;
            }

            int childIndex = Compare(_items[rightChildIndex], _items[leftChildIndex]) ? rightChildIndex : leftChildIndex;

            if (Compare(item, _items[childIndex]))
            {
                break;
            }

            _items[index] = _items[childIndex];
            _items[childIndex] = item;

            index = childIndex;
        }
    }

    private void HeapifyUp()
    {
        int index = Count - 1;
        int parent;
        T item = _items[index];
        while (true)
        {
            parent = (index - 1) / 2;
            if (parent < 0)
            {
                break;
            }
            if (Compare(item, _items[parent]))
            {
                _items[index] = _items[parent];
                _items[parent] = item;
                index = parent;
            }
            else
            {
                break;
            }
        }
    }

    private bool Compare(T a, T b)
    {
        return Comparison(a, b) < 0;
    }

}