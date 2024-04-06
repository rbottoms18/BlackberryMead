using System;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
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
        public static DefaultJsonTypeInfoResolver ExcludeByAttribute<TAttribute>(this DefaultJsonTypeInfoResolver resolver)
            where TAttribute : System.Attribute
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


        /// <summary>
        /// Modifier that adds all derived types of type <typeparamref name="T"/> in the current assembly to be included as KnownTypes
        /// by the serializer.
        /// </summary>
        /// <remarks>
        /// Uses type discriminator <c>$type</c> and <see cref="JsonUnknownDerivedTypeHandling.FallBackToNearestAncestor"/>. <br/>
        /// The discriminator names of types are the full names of the types. For instance, to deserialize an
        /// <see cref="BlackberryMead.Input.UI.UIComponent"/>, the discriminator would be 
        /// <c>"$type": "BlackberryMead.Input.UI.UIComponent"</c>.
        /// </remarks>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonTypeInfo"></param>
        //https://stackoverflow.com/questions/77809445/custom-typeinforesolver-to-manage-polymorphic-deserialization-in-system-text-jso
        public static void AddNativePolymorphicTypeInfo<T>(JsonTypeInfo jsonTypeInfo)
        {
            Type baseValueObjectType = typeof(T);
            if (jsonTypeInfo.Type == baseValueObjectType)
            {
                jsonTypeInfo.PolymorphismOptions = new JsonPolymorphismOptions
                {
                    TypeDiscriminatorPropertyName = "$type",
                    IgnoreUnrecognizedTypeDiscriminators = true,
                    UnknownDerivedTypeHandling = JsonUnknownDerivedTypeHandling.FallBackToNearestAncestor,
                };
                var types = Assembly
                    .GetExecutingAssembly()
                    .GetTypes()
                    .Where(t => t.IsSubclassOf(typeof(T)));
                foreach(var t in types)
                {
                    // Allowing the type to be abstract will throw an error.
                    if (baseValueObjectType.IsAssignableFrom(t) && !t.IsAbstract)
                    {
                        jsonTypeInfo.PolymorphismOptions.DerivedTypes.Add(new JsonDerivedType(t, t.FullName));
                    }
                }
            }
        }


        /// <summary>
        /// Modifier that allows all properites with <typeparamref name="TAttribute"/> 
        /// to be excluded from serialization.
        /// </summary>
        /// <typeparam name="TAttribute">Type of <see cref="Attribute"/> to exclude from serialization.</typeparam>
        /// <remarks>
        /// <example>
        /// This shows how to add the modifier to <see cref="DefaultJsonTypeInfoResolver"/>.
        /// <code>
        /// TypeInfoResolver = <see cref="DefaultJsonTypeInfoResolver"/>()
        /// {
        ///     Modifiers = { JsonExtensions.ExcludeByAttribute&lt;<typeparamref name="TAttribute"/>&gt; }
        /// }
        /// </code>
        /// </example>
        /// </remarks>
        public static void ExcludeByAttribute<TAttribute>(JsonTypeInfo typeInfo) where TAttribute : Attribute
        {
            if (typeInfo.Kind == JsonTypeInfoKind.Object)
                foreach (var property in typeInfo.Properties)
                    if (property.AttributeProvider?.IsDefined(typeof(TAttribute), true) == true)
                        property.ShouldSerialize = static (obj, value) => false;
        }


        /// <summary>
        /// Modifer for <see cref="JsonTypeInfoResolver"/> that requires a class decorated with
        /// <typeparamref name="TClassAttribute"/> to only serialize its properties decorated with
        /// <typeparamref name="TPropertyAttribute"/> (Opt-In Serialization).
        /// </summary>
        /// <typeparam name="TClassAttribute">Attribute that opts a class in.</typeparam>
        /// <typeparam name="TPropertyAttribute">Attribute that opts a property into serialization.</typeparam>
        /// <param name="typeInfo"></param>
        /// <remarks>
        ///  <example>
        /// This shows how to add the modifier to <see cref="DefaultJsonTypeInfoResolver"/>.
        /// <code>
        /// TypeInfoResolver = <see cref="DefaultJsonTypeInfoResolver"/>()
        /// {
        ///     Modifiers = 
        ///     { 
        ///         JsonExtensions.OptInByAttributeMethod&lt;<typeparamref name="TClassAttribute"/>, 
        ///         <typeparamref name="TAttribute"/>&gt; }
        ///     }
        /// }
        /// </code>
        /// </example>
        /// </remarks>
        public static void OptInSerialization<TClassAttribute, TPropertyAttribute>(JsonTypeInfo typeInfo)
            where TClassAttribute : Attribute where TPropertyAttribute : Attribute
        {
            if (typeInfo.Kind == JsonTypeInfoKind.Object)
            {
                if (typeInfo.Type.GetCustomAttribute<TClassAttribute>(false) != null)
                {
                    foreach (var property in typeInfo.Properties)
                        if (property.AttributeProvider?.IsDefined(typeof(TPropertyAttribute), true) == false)
                            property.ShouldSerialize = static (obj, value) => false;
                }
            }
        }
    }

    /// <summary>
    /// Attribute to mark a property to be excluded from serialization, but not ignored.
    /// </summary>
    /// <example>
    /// TypeInfoResolver = new EntityPolyTypeResolver().ExcludeByAttribute<JsonExclude>()
    /// </example>
    [System.AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class JsonExclude : System.Attribute { }

    /// <summary>
    /// Attribute to mark a property to be serialized when using 
    /// <see cref="JsonExtensions.OptInByAttribute{TAttribute}(JsonTypeInfo)"/>.
    /// </summary>
    /// <example>
    /// TypeInfoResolver = new EntityPolyTypeResolver().ExcludeByAttribute<JsonExclude>()
    /// </example>
    [System.AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class JsonOptIn : System.Attribute { }

    /// <summary>
    /// Attribute to mark a class to use Opt-In serialization.
    /// </summary>
    /// <remarks>
    /// See <see cref="Json.OptInSerialization{TClassAttribute, TPropertyAttribute}(JsonTypeInfo)"/>.
    /// </remarks>
    [System.AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false)]
    public class OptInJsonSerialization : System.Attribute { }
}
