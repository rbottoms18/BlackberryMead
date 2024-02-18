using System.Collections.Generic;

namespace BlackberryMead.Utility
{
    /// <summary>
    /// A Dictionary that takes one of two possible keys to return a value.
    /// Does not support removing elements from the dictionary,
    /// and must only be used for entries that will not change
    /// </summary>
    public class TwoKeyDictionary<K1, K2, V>
    {
        private Dictionary<K1, V> key1Dictionary;

        private Dictionary<K2, V> key2Dictionary;


        /// <summary>
        /// Creates a new empty TwoKeyDictionary
        /// </summary>
        public TwoKeyDictionary()
        {
            key1Dictionary = new Dictionary<K1, V>();
            key2Dictionary = new Dictionary<K2, V>();
        }


        /// <summary>
        /// Creates a new TwoKeyDictionary from two existing dictionaries
        /// that share a value type.
        /// </summary>
        /// <param name="dictionary1"></param>
        /// <param name="dictionary2"></param>
        public TwoKeyDictionary(Dictionary<K1, V> dictionary1,
            Dictionary<K2, V> dictionary2)
        {
            key1Dictionary = dictionary1;
            key2Dictionary = dictionary2;
        }


        /// <summary>
        /// Adds an item with the given keys to the Dictionary
        /// </summary>
        /// <param name="keyPair"></param>
        /// <param name="value"></param>
        public void Add((K1, K2) keyPair, V value)
        {
            key1Dictionary.Add(keyPair.Item1, value);
            key2Dictionary.Add(keyPair.Item2, value);
        }


        /// <summary>
        /// Tries to get a value with the given key from the Dictionary
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(K1 key, out V value)
        {
            if (key1Dictionary.TryGetValue(key, out value))
                return true;
            else
                return false;
        }


        /// <summary>
        /// Tries to get a value with the given key from the Dictionary
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(K2 key, out V value)
        {
            if (key2Dictionary.TryGetValue(key, out value))
                return true;
            else
                return false;
        }
    }
}
