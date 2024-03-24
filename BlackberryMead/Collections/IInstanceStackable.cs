using BlackberryMead.Utility;
using System;

namespace BlackberryMead.Collections
{
    /// <summary>
    /// <see cref="IStackable{T}"/> that stores and stacks with a specific instance
    /// of a type <typeparamref name="T"/> instead of any object of <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T"><see cref="INullImplementable{T}"/> type. Must include
    /// <see cref="IEquatable{T}"/> implementation.</typeparam>
    internal interface IInstanceStackable<T> : IStackable<T> where T : INullImplementable<T>, IEquatable<T>
    {
        /// <summary>
        /// Instance of type <typeparamref name="T"/> that the <see cref="IStackable{T}"/> stores
        /// and stacks with.
        /// </summary>
        public abstract T Value { get; }
    }
}
