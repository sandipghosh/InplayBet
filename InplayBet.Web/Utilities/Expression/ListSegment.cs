
namespace InplayBet.Web.Utilities.Expression
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// This is very simple, but not finished class representing segment of the List<>. Similar to ArraySegment<>, but more useful
    /// </summary>
    public class ListSegment<T>
    {
        List<T> list;
        int offset;
        int count;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListSegment{T}"/> class.
        /// </summary>
        /// <param name="list">The list.</param>
        public ListSegment(List<T> list) : this(list, 0, list.Count) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListSegment{T}"/> class.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The count.</param>
        public ListSegment(List<T> list, int offset, int count)
        {
            this.list = list;
            this.offset = offset;
            this.count = count;
        }

        /// <summary>
        /// Gets the <see cref="T"/> at the specified index.
        /// </summary>
        /// <value>
        /// The <see cref="T"/>.
        /// </value>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        /// <exception cref="System.IndexOutOfRangeException"></exception>
        public T this[int index]
        {
            get
            {
                if (index >= count) throw new IndexOutOfRangeException();
                return list[offset + index];
            }
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>
        /// The count.
        /// </value>
        public int Count
        {
            get { return count; }
        }

        /// <summary>
        /// Gets the segment.
        /// </summary>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The count.</param>
        /// <returns></returns>
        /// <exception cref="System.IndexOutOfRangeException"></exception>
        public ListSegment<T> GetSegment(int offset, int count)
        {
            if (offset >= this.count || offset + count > this.count)
                throw new IndexOutOfRangeException();
            return new ListSegment<T>(list, this.offset + offset, count);
        }

        /// <summary>
        /// Binaries the search.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public int BinarySearch(T item)
        {
            return BinarySearch(item, Comparer<T>.Default);
        }

        /// <summary>
        /// Binaries the search.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns></returns>
        public int BinarySearch(T item, IComparer<T> comparer)
        {
            return list.BinarySearch(offset, count, item, comparer);
        }

        /// <summary>
        /// Binaries the search.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="count">The count.</param>
        /// <param name="item">The item.</param>
        /// <param name="comparer">The comparer.</param>
        /// <returns></returns>
        /// <exception cref="System.IndexOutOfRangeException"></exception>
        public int BinarySearch(int index, int count, T item, IComparer<T> comparer)
        {
            if (index >= this.count || index + count > this.count)
                throw new IndexOutOfRangeException();

            return list.BinarySearch(offset + index, count, item, comparer);
        }

        /// <summary>
        /// Determines whether [contains] [the specified item].
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public bool Contains(T item)
        {
            for (int i = offset; i < offset + count; ++i)
                if (list[i].Equals(item)) return true;
            return false;
        }

        /// <summary>
        /// Existses the specified match.
        /// </summary>
        /// <param name="match">The match.</param>
        /// <returns></returns>
        public bool Exists(Predicate<T> match)
        {
            return FindIndex(match) != -1;
        }

        /// <summary>
        /// Finds the index.
        /// </summary>
        /// <param name="match">The match.</param>
        /// <returns></returns>
        public int FindIndex(Predicate<T> match)
        {
            for (int i = offset; i < offset + count; ++i)
                if (match(list[i])) return i - offset;
            return -1;
        }

        /// <summary>
        /// Finds the index.
        /// </summary>
        /// <param name="startIndex">The start index.</param>
        /// <param name="match">The match.</param>
        /// <returns></returns>
        /// <exception cref="System.IndexOutOfRangeException"></exception>
        public int FindIndex(int startIndex, Predicate<T> match)
        {
            if (startIndex >= count) throw new IndexOutOfRangeException();

            for (int i = offset + startIndex; i < offset + count; ++i)
                if (match(list[i])) return i - offset;
            return -1;
        }

        /// <summary>
        /// Finds the index.
        /// </summary>
        /// <param name="startIndex">The start index.</param>
        /// <param name="count">The count.</param>
        /// <param name="match">The match.</param>
        /// <returns></returns>
        /// <exception cref="System.IndexOutOfRangeException"></exception>
        public int FindIndex(int startIndex, int count, Predicate<T> match)
        {
            if (startIndex >= this.count || startIndex + count > this.count)
                throw new IndexOutOfRangeException();

            for (int i = offset + startIndex; i < offset + startIndex + count; ++i)
                if (match(list[i])) return i - offset;
            return -1;
        }

        /// <summary>
        /// Finds the last index.
        /// </summary>
        /// <param name="match">The match.</param>
        /// <returns></returns>
        public int FindLastIndex(Predicate<T> match)
        {
            for (int i = offset + count; --i >= offset; ++i)
                if (match(list[i])) return i - offset;
            return -1;
        }

        /// <summary>
        /// Finds the last index.
        /// </summary>
        /// <param name="startIndex">The start index.</param>
        /// <param name="match">The match.</param>
        /// <returns></returns>
        /// <exception cref="System.IndexOutOfRangeException"></exception>
        public int FindLastIndex(int startIndex, Predicate<T> match)
        {
            if (startIndex >= count) throw new IndexOutOfRangeException();

            for (int i = offset + count; --i >= offset + startIndex; ++i)
                if (match(list[i])) return i - offset;
            return -1;
        }

        /// <summary>
        /// Finds the last index.
        /// </summary>
        /// <param name="startIndex">The start index.</param>
        /// <param name="count">The count.</param>
        /// <param name="match">The match.</param>
        /// <returns></returns>
        /// <exception cref="System.IndexOutOfRangeException"></exception>
        public int FindLastIndex(int startIndex, int count, Predicate<T> match)
        {
            if (startIndex >= this.count || startIndex + count > this.count)
                throw new IndexOutOfRangeException();

            for (int i = offset + count; --i >= offset + startIndex; ++i)
                if (match(list[i])) return i - offset;
            return -1;
        }

        /// <summary>
        /// Finds the specified match.
        /// </summary>
        /// <param name="match">The match.</param>
        /// <returns></returns>
        public T Find(Predicate<T> match)
        {
            for (int i = offset; i < offset + count; ++i)
                if (match(list[i])) return list[i];
            return default(T);
        }

        /// <summary>
        /// Finds the last.
        /// </summary>
        /// <param name="match">The match.</param>
        /// <returns></returns>
        public T FindLast(Predicate<T> match)
        {
            for (int i = offset + count; --i >= offset; )
                if (match(list[i])) return list[i];
            return default(T);
        }

        /// <summary>
        /// Fors the each.
        /// </summary>
        /// <param name="action">The action.</param>
        public void ForEach(Action<T> action)
        {
            for (int i = offset; i < count; ++i)
                action(list[i]);
        }

        /// <summary>
        /// Trues for all.
        /// </summary>
        /// <param name="match">The match.</param>
        /// <returns></returns>
        public bool TrueForAll(Predicate<T> match)
        {
            for (int i = offset; i < count; ++i)
                if (!match(list[i])) return false;
            return true;
        }

        /// <summary>
        /// Finds all.
        /// </summary>
        /// <param name="match">The match.</param>
        /// <returns></returns>
        public List<T> FindAll(Predicate<T> match)
        {
            List<T> result = new List<T>();
            for (int i = offset; i < offset + count; ++i)
                if (match(list[i])) result.Add(list[i]);
            return result;
        }
    }
}