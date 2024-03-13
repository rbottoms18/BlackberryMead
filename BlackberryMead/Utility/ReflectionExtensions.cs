using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace BlackberryMead.Utility
{
    /// <summary>
    /// Utility class that holds methods useful for reflection.
    /// </summary>
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Gets all the derived types of type <paramref name="baseType"/> across all assemblies.
        /// </summary>
        /// <param name="baseType">Type to get all derived types of.</param>
        /// <returns></returns>
        // https://stackoverflow.com/questions/16220242/generally-accepted-way-to-avoid-knowntype-attribute-for-every-derived-class
        public static IEnumerable<Type> GetKnownTypes(this Type baseType)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.DefinedTypes)
                .Where(x => x.IsClass && !x.IsAbstract &&
                x.GetCustomAttribute<DataContractAttribute>() != null && baseType.IsAssignableFrom(x));
        }
    }
}
