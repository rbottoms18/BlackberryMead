using BlackberryMead.Utility;
using System;
using System.Runtime.CompilerServices;

namespace BlackberryMead.Collections
{
    /// <summary>
    /// <see cref="IStackable{T}"/> that stores and stacks with a specific instance
    /// of a type <typeparamref name="T"/> instead of any object of <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T"><see cref="INullImplementable{T}"/> type. Must include
    /// <see cref="IEquatable{T}"/> implementation.</typeparam>
    public interface IInstanceStackable<T> : IStackable<T>, INullImplementable<IInstanceStackable<T>>
        where T : INullImplementable<T>, IEquatable<T>
    {
        static IInstanceStackable<T> INullImplementable<IInstanceStackable<T>>.Null { get; }

        /// <inheritdoc cref="INullImplementable{T}.IsNull"/>
        new abstract bool IsNull();

        /// <summary>
        /// Instance of type <typeparamref name="T"/> that the <see cref="IStackable{T}"/> stores
        /// and stacks with.
        /// </summary>
        public new abstract T Value { get; }
    }
}
