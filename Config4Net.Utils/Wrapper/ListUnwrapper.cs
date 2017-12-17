using System;
using System.Collections;
using System.Collections.Generic;

namespace Config4Net.Utils.Wrapper
{
    internal class ListUnwrapper<T> : IList<T>
    {
        private readonly IList<object> _source;

        public ListUnwrapper(IList<object> source)
        {
            _source = source;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new EnumeratorWrapper<T>(_source.GetEnumerator());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(T item)
        {
            _source.Add(item);
        }

        public void Clear()
        {
            _source.Clear();
        }

        public bool Contains(T item)
        {
            return _source.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            var objectArray = new object[array.Length];
            Array.Copy(array, objectArray, array.Length);
            _source.CopyTo(objectArray, arrayIndex);
        }

        public bool Remove(T item)
        {
            return _source.Remove(item);
        }

        public int Count => _source.Count;
        public bool IsReadOnly => _source.IsReadOnly;

        public int IndexOf(T item)
        {
            return _source.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            _source.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _source.RemoveAt(index);
        }

        public T this[int index]
        {
            get => (T) _source[index];
            set => _source[index] = value;
        }
    }
}