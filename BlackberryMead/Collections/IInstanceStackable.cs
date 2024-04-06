using BlackberryMead.Framework;
using System;

namespace BlackberryMead.Collections
{
    /// <summary>
    /// <see cref="IStackable{T}"/> that stores and stacks <typeparamref name="T"/> objects that are equivalent 
    /// to a specific instance of a <typeparamref name="T"/> object.
    /// </summary>
    /// <typeparam name="T"><see cref="INullImplementable{T}"/> type. Must include
    /// <see cref="IEquatable{T}"/> implementation.</typeparam>
    public interface IInstanceStackable<T> : IStackable<T>, INullImplementable<IInstanceStackable<T>>
        where T : INullImplementable<T>, IEquatable<T>
    {
        static IInstanceStackable<T> INullImplementable<IInstanceStackable<T>>.Null { get; }

        /// <inheritdoc cref="INullImplementable{T}.Null"/>;
        // Override it here so there isn't an ambiguity between Null properties.
        static new IInstanceStackable<T> Null => Null;

        /// <inheritdoc cref="INullImplementable{T}.IsNull"/>
        new bool IsNull() => IsNull();

        abstract bool INullImplementable<IInstanceStackable<T>>.IsNull();

        /// <summary>
        /// Instance of type <typeparamref name="T"/> that the <see cref="IStackable{T}"/> stores
        /// and stacks with.
        /// </summary>
        public abstract T Value { get; }
    }
}
