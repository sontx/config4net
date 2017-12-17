using Config4Net.Utils.Wrapper;
using System.Collections;
using System.Collections.Generic;

namespace Config4Net.Utils
{
    public static class WrapperUtils
    {
        public static IEnumerable<T> GetEnumerable<T>(IEnumerator<T> enumerator)
        {
            return new EnumerableWrapper<T>(enumerator);
        }

        public static IEnumerable<T> GetEnumerable<T>(IEnumerator enumerator)
        {
            return new EnumerableWrapper<T>(enumerator);
        }

        public static IList<object> GetObjectList<T>(IList<T> source)
        {
            return new ListWrapper<T>(source);
        }

        public static IList<T> GetGenericList<T>(IList<object> source)
        {
            return new ListUnwrapper<T>(source);
        }
    }
}