using System.Collections.Generic;

namespace BlackberryMead.Collections
{
    /// <summary>
    /// A Dictionary that accepts two possible keys for a given value.
    /// </summary>
    /// <remarks>
    /// Does not support removing elements from the dictionary,
    /// and must only be used for entries that will not change.
    /// </remarks>
    public class TwoKeyDictionary<TKey1, TKey2, TValue>
    {
        private Dictionary<TKey1, TValue> key1Dictionary;

        private Dictionary<TKey2, TValue> key2Dictionary;


        /// <summary>
        /// Creates a new empty <see cref="TwoKeyDictionary{K1, K2, V}"/>.
        /// </summary>
        public TwoKeyDictionary()
        {
            key1Dictionary = new Dictionary<TKey1, TValue>();
            key2Dictionary = new Dictionary<TKey2, TValue>();
        }


        /// <summary>
        /// Creates a new <see cref="TwoKeyDictionary{K1, K2, V}"/> from two existing <see cref="Dictionary{TKey, TValue}"/>.
        /// that share a value type.
        /// </summary>
        public TwoKeyDictionary(Dictionary<TKey1, TValue> dictionary1,
            Dictionary<TKey2, TValue> dictionary2)
        {
            key1Dictionary = dictionary1;
            key2Dictionary = dictionary2;
        }


        /// <summary>
        /// Adds an item with the given keys to the <see cref="TwoKeyDictionary{K1, K2, V}"/>.
        /// </summary>
        /// <param name="keyPair"></param>
        /// <param name="value"></param>
        public void Add((TKey1, TKey2) keyPair, TValue value)
        {
            key1Dictionary.Add(keyPair.Item1, value);
            key2Dictionary.Add(keyPair.Item2, value);
        }


        /// <summary>
        /// Tries to get a value with the given key from the <see cref="TwoKeyDictionary{K1, K2, V}"/>.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(TKey1 key, out TValue value)
        {
            if (key1Dictionary.TryGetValue(key, out value))
                return true;
            else
                return false;
        }


        /// <summary>
        /// Tries to get a value with the given key from the <see cref="TwoKeyDictionary{TKey1, TKey2, TValue}"/>.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(TKey2 key, out TValue value)
        {
            if (key2Dictionary.TryGetValue(key, out value))
                return true;
            else
                return false;
        }
    }
}
