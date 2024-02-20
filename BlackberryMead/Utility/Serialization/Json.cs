using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace BlackberryMead.Utility.Serialization
{
    /// <summary>
    /// Provides methods for working with Json strings and objects.
    /// </summary>
    public static class Json
    {
        /// <summary>
        /// Populates an existing object of type T with values from a Json string describing another
        /// instance of type T.
        /// </summary>
        /// Source: https://stackoverflow.com/questions/56835040/net-core-3-0-jsonserializer-populate-existing-object
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="target">Target object to deserialize properties to.</param>
        /// <param name="jsonSource">Json string describing object of type T.</param>
        public static void PopulateObject<T>(T target, string jsonSource, JsonSerializerOptions options = null) where T : class
        {
            options ??= new JsonSerializerOptions();

            // Deserialize json
            var json = JsonDocument.Parse(jsonSource).RootElement;

            foreach (var property in json.EnumerateObject())
            {
                OverwriteProperty(target, property, options);
            }
        }


        /// <summary>
        /// Deserialize Json from file.
        /// </summary>
        /// <typeparam name="T">Type of object to be deserialized into.</typeparam>
        /// <param name="jsonPath">File path of the json.</param>
        /// <param name="options">JsonSerializerOptions to configure serialization settings. Defaults to 
        /// JsonSerializerOptions.Default.</param>
        /// <returns></returns>
        public static T DeserializePath<T>(string jsonPath, JsonSerializerOptions options = null)
        {
            if (options == null)
                options = JsonSerializerOptions.Default;

            T deserializedJson;
            string json = File.ReadAllText(jsonPath);
            deserializedJson = JsonSerializer.Deserialize<T>(json, options);

            return deserializedJson;
        }


        /// <summary>
        /// Deserialize Json from a string.
        /// </summary>
        /// <typeparam name="T">Type of object to be deserialized into.</typeparam>
        /// <param name="s">String of json data.</param>
        /// <param name="options">JsonSerializerOptions to configure serialization settings. Defaults to 
        /// JsonSerializerOptions.Default.</param>
        /// <returns></returns>
        public static T Deserialize<T>(string s, JsonSerializerOptions options = null)
        {
            if (options == null)
                options = JsonSerializerOptions.Default;

            T deserializedJson;
            deserializedJson = JsonSerializer.Deserialize<T>(s, options);
            return deserializedJson;
        }


        /// <summary>
        /// Serialize an object to json.
        /// </summary>
        /// <param name="obj">Object to be serialized.</param>
        /// <param name="options">JsonSerializerOptions to configure serialization settings. Defaults to 
        /// JsonSerializerOptions.Default.</param>
        /// <returns></returns>
        public static string Serialize(object obj, JsonSerializerOptions options = null)
        {
            if (options == null)
                options = JsonSerializerOptions.Default;

            string serializedJson = JsonSerializer.Serialize(obj, options);
            return serializedJson;
        }


        /// <summary>
        /// Serializes an object to a Json file.
        /// </summary>
        /// <param name="path">Path of the file to write to.</param>
        /// <inheritdoc cref="Serialize(object, JsonSerializerOptions)"/>
        public static void SerializeToFile(string path, object obj, JsonSerializerOptions options = null)
        {
            File.WriteAllText(path, Serialize(obj, options));
        }


        /// <summary>
        /// Serialize an object to a JSON string.
        /// </summary>
        /// <remarks>
        /// <see cref="Serialize{T}(T, DataContractJsonSerializerSettings)"/> serializes following
        /// a DataContract if the attribute is applied to type <typeparamref name="T"/>. To serialize without a 
        /// DataContract, use <see cref="Serialize(object, JsonSerializerOptions)"/>.
        /// </remarks>
        /// <typeparam name="T">Type of object to serialize.</typeparam>
        /// <param name="obj">Object to serialize.</param>
        /// <param name="settings">Settings for serialization.</param>
        /// <returns></returns>
        public static string Serialize<T>(T obj, DataContractJsonSerializerSettings settings = null,
            JsonSerializerOptions options = null)
        {
            if (settings == null)
                settings = new DataContractJsonSerializerSettings();

            // Create a stream to serialize the object to.
            var ms = new MemoryStream();

            // Serializer the object to the stream.
            var ser = new DataContractJsonSerializer(typeof(T), settings);
            ser.WriteObject(ms, obj);
            byte[] json = ms.ToArray();
            ms.Close();
            return Encoding.UTF8.GetString(json, 0, json.Length);
        }


        /// <summary>
        /// Serialize an object to a JSON file.
        /// </summary>
        /// <param name="path">Path of the file to serialize to.</param>
        /// <param name="writeIndented">If true prints the JSON string indented in the file.<br/> Equivalent
        /// to <see cref="JsonSerializerOptions.WriteIndented"/>.</param>
        /// <inheritdoc cref="Serialize{T}(T, DataContractJsonSerializerSettings)"/>
        public static void SerializeToFile<T>(string path, T obj, bool writeIndented = true,
            DataContractJsonSerializerSettings settings = null)
        {
            string _ = Serialize<T>(obj, settings);
            if (writeIndented)
                _ = JsonNode.Parse(_)!.ToString();
            File.WriteAllText(path, _);
        }


        /// <summary>
        /// Converts a Json string to a <see cref="JsonObject"/>.
        /// </summary>
        /// <param name="obj">string object to convert.</param>
        /// <returns>A Json object constructed from <paramref name="obj"/>.</returns>
        public static JsonObject ToJsonObject(string obj)
        {
            return JsonNode.Parse(obj)!.AsObject();
        }


        /// <summary>
        /// Overwrites a property of a target object with a new value.
        /// </summary>
        /// Source: https://stackoverflow.com/questions/56835040/net-core-3-0-jsonserializer-populate-existing-object
        /// <typeparam name="T">Type of the object whose parameter will be overwritten.</typeparam>
        /// <param name="target">Target object to overwrite property of.</param>
        /// <param name="updatedProperty">Property value to overwrite 'target' with.</param>
        private static void OverwriteProperty<T>(T target, JsonProperty updatedProperty, JsonSerializerOptions options) where T : class
        {
            var propertyInfo = typeof(T).GetProperty(updatedProperty.Name);

            if (propertyInfo == null)
            {
                return;
            }

            var propertyType = propertyInfo.PropertyType;
            var parsedValue = JsonSerializer.Deserialize(
                updatedProperty.Value.GetRawText(),
                propertyType, options);
            propertyInfo.SetValue(target, parsedValue);
        }
    }
}
