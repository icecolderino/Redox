using System;
using System.Collections.Generic;

namespace Redox.API.Extensions
{
    public static class CollectionExtension
    {
        #region ForEach
        public static void ForEach<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> collections, Action<TKey, TValue> callback)
        {
            foreach (var pair in collections)
                callback.Invoke(pair.Key, pair.Value);
        }

        public static void ForEach<T>(this IEnumerable<T> collections, Action<T> callBack)
        {
            foreach (var col in collections)
                callBack.Invoke(col);
        }
        #endregion

    }
}
