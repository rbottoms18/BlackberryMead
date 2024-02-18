using System;

namespace BlackberryMead.Serialization
{
    /// <summary>
    /// Attribute to mark a property to be excluded from serialization, but not ignored.
    /// </summary>
    /// <example>
    /// TypeInfoResolver = new EntityPolyTypeResolver().ExcludeByAttribute<JsonExclude>()
    /// </example>
    [System.AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class JsonExclude : System.Attribute { }
}
