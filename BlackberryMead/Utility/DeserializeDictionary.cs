using BlackberryMead.Serialization;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.Json;

namespace BlackberryMead.Utility
{
    /// <summary>
    /// A <see cref="Dictionary{TKey, TValue}"/> that stores key-value pairs of
    /// keys of type <typeparamref name="TKey"/> with Json strings that deserialize to type <typeparamref name="TValue"/>.
    /// Getting the value associated with a key will return the deserialized Json of type <typeparamref name="TValue"/>.
    /// </summary>
    public class DeserializeDictionary<TKey, TValue> where TKey : notnull
        where TValue : INullImplimentable<TValue>
    {
        /// <summary>
        /// List of keys in the dictionary.
        /// </summary>
        public List<TKey> Keys
        { get { return jsonDict.Keys.ToList(); } }

        /// <summary>
        /// Dictionary that stores keys to json strings.
        /// </summary>
        private Dictionary<TKey, string> jsonDict = new Dictionary<TKey, string>();

        /// <summary>
        /// Null Object implimentation of <typeparamref name="TValue"/>.
        /// </summary>
        private TValue Null = TValue.GetNull();

        /// <summary>
        /// Options to use when deserializing.
        /// </summary>
        private JsonSerializerOptions serializerOptions;


        /// <summary>
        /// Create a new DeserializeDictionary.
        /// </summary>
        /// <param name="serializerOptions">SerializerOptions for deserialization</param>
        public DeserializeDictionary(JsonSerializerOptions serializerOptions)
        {
            this.serializerOptions = serializerOptions;
        }


        /// <summary>
        /// Get the value associated with the given key.
        /// </summary>
        /// <param name="key">Key to get the value for.</param>
        /// <returns>Returns the value corresponding to the given key.</returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public TValue this[TKey key]
        {
            get
            {
                if (!jsonDict.ContainsKey(key))
                    throw new KeyNotFoundException();
                try
                {
                    TValue value = Json.Deserialize<TValue>(jsonDict[key], serializerOptions);
                    return value;
                }
                catch
                {
                    return Null;
                }
            }
        }


        /// <summary>
        /// Add a new Key-Value pair to the dictionary.
        /// </summary>
        /// <param name="key">Key of the object.</param>
        /// <param name="jsonValue">Json string of the object.</param>
        /// <exception cref="DuplicateNameException"></exception>
        public void Add(TKey key, string jsonValue)
        {
            if (jsonDict.ContainsKey(key))
                throw new DuplicateNameException();
            jsonDict.Add(key, jsonValue);
        }


        /// <summary>
        /// Remove the object with the given key from the dictionary.
        /// </summary>
        /// <param name="key">Key to remove from the dictionary.</param>
        public void Remove(TKey key)
        {
            if (jsonDict.ContainsKey(key))
                jsonDict.Remove(key);
        }


        /// <summary>
        /// Gets the Json string with the given key.
        /// </summary>
        /// <param name="key">Key of the desired Json string.</param>
        /// <returns></returns>
        public string JsonAt(TKey key)
        {
            if (jsonDict.TryGetValue(key, out string jsonValue))
            {
                return jsonValue;
            }
            return null;
        }


        /// <summary>
        /// Whether the dictionary contains the given key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(TKey key)
        {
            return jsonDict.ContainsKey(key);
        }


        /// <summary>
        /// Attemps to get the <typeparamref name="TValue"/> with the given key.
        /// </summary>
        /// <param name="key">Key of the <typeparamref name="TValue"/> to get.</param>
        /// <param name="value">The <typeparamref name="TValue"/> with the given key.</param>
        /// <returns>The <typeparamref name="TValue"/> with the given key.</returns>
        public bool TryGetValue(TKey key, out TValue value)
        {
            if (ContainsKey(key))
            {
                value = this[key];
                return true;
            }
            value = Null;
            return false;
        }
    }
}
