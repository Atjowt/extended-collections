namespace System.Collections.Extended;

public class Trie<T> where T : notnull
{
    /// <summary>
    /// The children nodes of this <see cref="Trie{T}"/> node, which may be accessed by providing a key <typeparamref name="T"/>.
    /// </summary>
    public Dictionary<T, Trie<T>> Children { get; set; }

    /// <summary>
    /// A flag to determine whether this <see cref="Trie{T}"/> node is the end of a collection.
    /// </summary>
    public bool IsTerminal { get; set; }

    /// <summary>
    /// Initializes a new <see cref="Trie{T}"/>.
    /// </summary>
    public Trie()
    {
        Children = new();
        IsTerminal = false;
    }

    /// <summary>
    /// Inserts a <paramref name="collection"/> to the <see cref="Trie{T}"/>.
    /// </summary>
    /// <param name="collection">The collection to be inserted into the <see cref="Trie{T}"/>.</param>
    public void Insert(IEnumerable<T> collection)
    {
        Trie<T> pointer = this;
        foreach (T item in collection)
        {
            pointer.Children.TryAdd(item, new());
            pointer = pointer.Children[item];
        }
        pointer.IsTerminal = true;
    }

    /// <summary>
    /// Removes a <paramref name="collection"/> from the <see cref="Trie{T}"/>.
    /// </summary>
    /// <param name="collection">The collection to be removed from the <see cref="Trie{T}"/>.</param>
    /// <returns>
    /// <see langword="true"/> if the removal was successful, otherwise <see langword="false"/>.
    /// </returns>
    public bool Remove(IEnumerable<T> collection)
    {
        Stack<(T, Trie<T>)> stack = new();

        Trie<T> pointer = this;

        foreach (T item in collection)
        {
            if (pointer.Children.TryGetValue(item, out Trie<T>? next))
            {
                stack.Push((item, pointer));
                pointer = next;
            }
            else
            {
                return false;
            }
        }

        if (pointer.IsTerminal)
        {
            pointer.IsTerminal = false;

            while (stack.Count > 0 && pointer.Children.Count == 0)
            {
                (T parentKey, Trie<T> parentNode) = stack.Pop();
                parentNode.Children.Remove(parentKey);
                pointer = parentNode;
            }

            return true;
        }

        return false;
    }

    /// <summary>
    /// Searches the <see cref="Trie{T}"/> for a collection.
    /// </summary>
    /// <param name="collection">The collection to search for in the <see cref="Trie{T}"/>.</param>
    /// <returns>The <see cref="Trie{T}"/> node which terminates the collection, if found.</returns>
    public Trie<T>? Search(IEnumerable<T> collection)
    {
        Trie<T> pointer = this;
        foreach (T item in collection)
        {
            if (pointer.Children.TryGetValue(item, out Trie<T>? next))
            {
                pointer = next;
            }
            else
            {
                return null;
            }
        }
        return pointer.IsTerminal ? pointer : null;
    }

    /// <summary>
    /// Searches the <see cref="Trie{T}"/> for a collection that begins with a given <paramref name="prefix"/>.
    /// </summary>
    /// <param name="prefix">The collection prefix to search for in the <see cref="Trie{T}"/>.</param>
    /// <returns>The <see cref="Trie{T}"/> node at the end of the <paramref name="prefix"/>, if found.</returns>
    public Trie<T>? SearchPrefix(IEnumerable<T> prefix)
    {
        Trie<T> pointer = this;
        foreach (T item in prefix)
        {
            if (pointer.Children.TryGetValue(item, out Trie<T>? next))
            {
                pointer = next;
            }
            else
            {
                return null;
            }
        }
        return pointer;
    }
}