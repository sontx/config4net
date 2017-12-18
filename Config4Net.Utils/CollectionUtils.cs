using System.Collections.Generic;

namespace Config4Net.Utils
{
    public static class CollectionUtils
    {
        public static void CopyDictionary<TKey, TValue>(
            Dictionary<TKey, TValue> sourceDictionary,
            Dictionary<TKey, TValue> destinationDictionary)
        {
            foreach (var node in sourceDictionary)
            {
                destinationDictionary.Add(node.Key, node.Value);
            }
        }
    }
}