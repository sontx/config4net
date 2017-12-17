using System;
using System.Collections;
using System.Collections.Generic;

namespace Config4Net.Utils.Wrapper
{
    internal class ListWrapper<T> : IList<object>
    {
        private readonly IList<T> _source;

        public ListWrapper(IList<T> source)
        {
            _source = source;
        }

        public IEnumerator<object> GetEnumerator()
        {
            return new EnumeratorWrapper<object>(_source.GetEnumerator());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(object item)
        {
            _source.Add((T)item);
        }

        public void Clear()
        {
            _source.Clear();
        }

        public bool Contains(object item)
        {
            return _source.Contains((T)item);
        }

        public void CopyTo(object[] array, int arrayIndex)
        {
            var genericArray = new T[array.Length];
            Array.Copy(array, genericArray, array.Length);
            _source.CopyTo(genericArray, arrayIndex);
        }

        public bool Remove(object item)
        {
            return _source.Remove((T)item);
        }

        public int Count => _source.Count;
        public bool IsReadOnly => _source.IsReadOnly;

        public int IndexOf(object item)
        {
            return _source.IndexOf((T)item);
        }

        public void Insert(int index, object item)
        {
            _source.Insert(index, (T)item);
        }

        public void RemoveAt(int index)
        {
            _source.RemoveAt(index);
        }

        public object this[int index]
        {
            get => _source[index];
            set => _source[index] = (T)value;
        }
    }
}