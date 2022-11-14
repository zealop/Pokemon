using System.Collections.Generic;

namespace Utils
{
    public static class DictionaryExtensions
    {
        public static bool TryGetValueAs<TKey, TValue, TValueAs>(this IDictionary<TKey, TValue> dictionary, TKey key,
            out TValueAs valueAs) where TValueAs : TValue
        {
            if (dictionary.TryGetValue(key, out var value))
            {
                valueAs = (TValueAs)value;
                return true;
            }

            valueAs = default;
            return false;
        }
    }
}