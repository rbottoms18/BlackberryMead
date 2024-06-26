﻿using BlackberryMead.Framework;
using System.Collections.Generic;

namespace BlackberryMead.Collections
{
    /// <summary>
    /// Defines common methods for manipulating and combining (stacking) various collections 
    /// of objects of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T"><see cref="INullImplementable{T}"/> type.</typeparam>
    public interface IStackable<T> : IQuantifiable, INullImplementable<IStackable<T>>, IEnumerable<T>
        where T : INullImplementable<T>
    {
        static IStackable<T> INullImplementable<IStackable<T>>.Null => Null;

        /// <inheritdoc cref="INullImplementable{T}.Null"/>
        // Override it here so there isn't an ambiguity between Null properties.
        static new IStackable<T> Null { get; }

        bool INullImplementable<IStackable<T>>.IsNull => IsNull;

        /// <inheritdoc cref="INullImplementable{T}.IsNull"/>
        // Override it here so there isn't an ambiguity between IsNull properties.
        public new abstract bool IsNull { get; }

        /// <summary>
        /// Maximum number of objects than can be stored in the <see cref="IStackable{T}"/>.
        /// </summary>
        /// <remarks>
        /// To represent a (functionally) unlimited number of objects, use <see cref="System.Int32.MaxValue"/>.
        /// </remarks>
        public abstract int MaxStackSize { get; }


        /// <summary>
        /// Adds a <see cref="T"/> to the <see cref="IStackable{T}"/>.
        /// </summary>
        /// <param name="item"><typeparamref name="T"/> to add to the <see cref="IStackable{T}"/>.</param>
        /// <returns><see langword="true"/> if <paramref name="item"/> is added to the <see cref="IStackable{T}"/>;
        /// otherwise, <see langword="false"/>.</returns>
        public abstract bool Add(T item);


        /// <summary>
        /// Removes a number of <typeparamref name="T"/> objects from the <see cref="IStackable{T}"/>.
        /// </summary>
        /// <param name="amount">Number of <typeparamref name="T"/> objects to remove.</param>
        /// <returns><see langword="true"/> if <paramref name="amount"/> number of objects were removed or
        /// if the <see cref="IStackable{T}"/> becomes emtpy in the process; otherwise, <see langword="false"/>.</returns>
        public abstract bool Remove(int amount);


        /// <summary>
        /// Removes a <typeparamref name="T"/> object from the <see cref="IStackable{T}"/>.
        /// </summary>
        /// <param name="item"><typeparamref name="T"/> to remove.</param>
        /// <returns><see langword="true"/> if <paramref name="item"/> was removed; otherwise, <see langword="false"/>.</returns>
        public abstract bool Remove(T item);


        /// <summary>
        /// Splits the <see cref="IStackable{T}"/> into two <see cref="IStackable{T}"/>.
        /// </summary>
        /// <param name="amount">Amount of <typeparamref name="T"/> to be split from the <see cref="IStackable{T}"/>
        /// and returned. </param>
        /// <returns>An <see cref="IStackable{T}"/> containing <paramref name="amount"/> number of <typeparamref name="T"/> objects
        /// from the <see cref="IStackable{T}"/>.</returns>
        public abstract IStackable<T> Split(int amount);


        /// <summary>
        /// Stacks contents of another <see cref="IStackable{T}"/> into the <see cref="IStackable{T}"/>.
        /// </summary>
        /// <param name="other">Other <see cref="IStackable{T}"/> whose contents to stack into 
        /// the <see cref="IStackable{T}"/>.</param>
        /// <returns><see langword="true"/> if <paramref name="other"/> successfully stacks for any amount; otherwise,
        /// <see langword="false"/>.</returns>
        public abstract bool Stack(IStackable<T> other);


        /// <param name="values">Objects that were added to the <see cref="IStackable{T}"/>.</param>
        /// <inheritdoc cref="IStackable{T}.Stack(IStackable{T})"/>
        public abstract bool Stack(IStackable<T> other, out IStackable<T> values);


        /// <summary>
        /// Evaluates whether the <see cref="IStackable{T}"/> can be stacked with another <see cref="IStackable{T}"/>.
        /// </summary>
        /// <param name="other">Other <see cref="IStackable{T}"/> to evaluate whether it can be stacked
        /// into the <see cref="IStackable{T}"/>.</param>
        /// <returns><see langword="true"/> if <paramref name="other"/> can be stacked into the
        /// <see cref="IStackable{T}"/>; otherwise, <see langword="false"/>.</returns>
        public abstract bool IsStackableWith(IStackable<T> other);


        /// <summary>
        /// Takes and returns one <typeparamref name="T"/> from the <see cref="IStackable{T}"/>.
        /// </summary>
        /// <returns>A <typeparamref name="T"/> object from the <see cref="IStackable{T}"/>.</returns>
        public abstract T Take();
    }
}
