using System;
using System.Collections.Generic;

namespace NebzzClientFront
{
    internal static class Utility
    {
        public static IEnumerable<(T key, IEnumerable<T> group)> GroupTogether<T>(this IEnumerable<T> data, Func<T, T, bool> inTheGroup)
        {
            return GroupTogether(data, item => item, inTheGroup);
        }

        public static IEnumerable<(V key, IEnumerable<T> group)> GroupTogether<T, V>(this IEnumerable<T> data, Func<T, V> keySelector, Func<V, V, bool> inTheGroup)
        {
            V groupKeyValue = default;
            V previousKeyValue = default;
            var inner = new List<T>();
            foreach (var d in data)
            {
                var dValue = keySelector(d);
                if (!inTheGroup(previousKeyValue, dValue))
                {
                    if (!default(V).Equals(previousKeyValue))
                    {
                        yield return (groupKeyValue, inner);
                        inner = new List<T>();
                    }
                    groupKeyValue = dValue;
                }
                previousKeyValue = dValue;
                inner.Add(d);
            }
            if (inner.Count > 0)
            {
                yield return (groupKeyValue, inner);
            }
        }
    }
}
