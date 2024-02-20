using System;
using System.Text.Json.Serialization.Metadata;

namespace BlackberryMead.Utility.Serialization
{
    /// <summary>
    /// Provides useful extensions to Json serialization.
    /// </summary>
    public static class JsonExtensions
    {
        /// <summary>
        /// Extension method that allows all properites with <typeparamref name="TAttribute"/> 
        /// to be excluded from serialization.
        /// </summary>
        /// <typeparam name="TAttribute">Type of <see cref="Attribute"/> to exclude from serialization.</typeparam>
        /// <param name="resolver"><see cref="DefaultJsonTypeInfoResolver"/> to append to</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static DefaultJsonTypeInfoResolver ExcludeByAttribute<TAttribute>(this DefaultJsonTypeInfoResolver resolver) where TAttribute : System.Attribute
        {
            if (resolver == null)
                throw new ArgumentNullException();
            var attr = typeof(TAttribute);
            resolver.Modifiers.Add(typeInfo =>
            {
                if (typeInfo.Kind == JsonTypeInfoKind.Object)
                    foreach (var property in typeInfo.Properties)
                        if (property.AttributeProvider?.IsDefined(attr, true) == true)
                            property.ShouldSerialize = static (obj, value) => false;
            });
            return resolver;
        }
    }
}
