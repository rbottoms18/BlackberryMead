using BlackberryMead.Collections;
using BlackberryMead.Utility.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BlackberryMead.Framework
{
    /// <summary>
    /// Class that has a collection of values instantiated from Json values.
    /// </summary>
    /// <typeparam name="T">Object type to be loaded from Json.</typeparam>
    public abstract class JsonLoadableObject<T> : IJsonOnDeserialized, IJsonOnDeserializing
        where T : INullImplementable<T>
    {
        /// <summary>
        /// Dictionary of Json data of each <typeparamref name="T"/> able to be loaded.
        /// </summary>
        protected static DeserializeDictionary<int, T> objectsDict { get; set; } =
            new DeserializeDictionary<int, T>(JsonSerializerOptions.Default);


        /// <summary>
        /// Create a new instance of <typeparamref name="T"/> with ID <paramref name="ID"/>.
        /// </summary>
        /// <param name="ID">ID of the <typeparamref name="T"/> to return.</param>
        /// <remarks>
        /// If a <typeparamref name="T"/> with the given <paramref name="ID"/> is not found,
        /// <see cref="T.GetNull()"/> will be returned.
        /// </remarks>
        /// <returns><typeparamref name="T"/> with key <paramref name="ID"/> in 
        /// <see cref="objectsDict"/>.</returns>
        public static T New(int ID)
        {
            if (objectsDict.TryGetValue(ID, out var value)) return value;
            return T.Null;
        }


        /// <summary>
        /// Populate <paramref name="obj"/> with fields from the <typeparamref name="T"/> with 
        /// key <paramref name="ID"/>.
        /// </summary>
        /// <param name="obj">Object to populate.</param>
        /// <param name="ID">Key in <see cref="objectsDict"/> of the <typeparamref name="T"/> to 
        /// populate values from.</param>
        public static void Populate(dynamic obj, int ID)
        {
            if (objectsDict.ContainsKey(ID))
            {
                Json.PopulateObject(obj, objectsDict.JsonAt(ID));
            }
        }


        /// <summary>
        /// Set the <see cref="objectsDict"/> for <typeparamref name="T"/>.
        /// </summary>
        /// <param name="objectsDict"><see cref="DeserializeDictionary{int, T}"/> of all the 
        /// <typeparamref name="T"/> able to be loaded.</param>
        public static void SetObjects(DeserializeDictionary<int, T> objectsDict)
        {
            JsonLoadableObject<T>.objectsDict = objectsDict;
        }


        public virtual void OnDeserialized() { }


        public virtual void OnDeserializing() { }
    }
}
