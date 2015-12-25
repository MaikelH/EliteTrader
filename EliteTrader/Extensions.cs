using System.Collections.Generic;
using Optional;

namespace EliteTrader
{
    public static class Extensions
    {
        public static Option<V> TryGetValue<T, V>(this Dictionary<T, V> dictionary, T key)
        {
            V value;

            bool result = dictionary.TryGetValue(key, out value);

            if (result)
            {
                return Option.Some(value);
            }

            return Option.None<V>();
        }
    }
}
