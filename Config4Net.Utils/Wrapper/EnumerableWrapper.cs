using System;
using System.Collections;
using System.Collections.Generic;

namespace Config4Net.Utils.Wrapper
{
    internal sealed class EnumerableWrapper<T> : IEnumerable<T>, IDisposable
    {
        private readonly IEnumerator<T> _enumerator;

        public EnumerableWrapper(IEnumerator<T> enumerator)
        {
            _enumerator = enumerator;
        }

        public EnumerableWrapper(IEnumerator enumerator)
        {
            _enumerator = new EnumeratorWrapper<T>(enumerator);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _enumerator;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Dispose()
        {
            _enumerator?.Dispose();
        }
    }
}