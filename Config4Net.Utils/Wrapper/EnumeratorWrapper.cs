using System.Collections;
using System.Collections.Generic;

namespace Config4Net.Utils.Wrapper
{
    internal class EnumeratorWrapper<T> : IEnumerator<T>
    {
        private readonly IEnumerator _enumerator;

        public EnumeratorWrapper(IEnumerator enumerator)
        {
            _enumerator = enumerator;
        }

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            return _enumerator.MoveNext();
        }

        public void Reset()
        {
            _enumerator.Reset();
        }

        public T Current => (T)_enumerator.Current;

        object IEnumerator.Current => Current;
    }
}