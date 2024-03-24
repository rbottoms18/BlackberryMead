using System;
using System.Collections;
using System.Collections.Generic;

namespace BlackberryMead.Framework
{
    /// <summary>
    /// Create a new <see cref="Bag{T}"/>.
    /// </summary>
    /// <param name="size">Size of the <see cref="Bag{T}"/>.</param>
    internal abstract class Bag
<T>(int size) : ICollection<T>
    {
        /// <summary>
        /// Collection of <see cref="T"/> this holds.
        /// </summary>
        protected T[] collection = new T[size];


        /// <summary>
        /// Enumerator class for <see cref="Bag{T}"/>.
        /// </summary>
        public class BagEnumerator : IEnumerator<Bag<T>>
        {
            public object Current => throw new NotImplementedException();

            Bag<T> IEnumerator<Bag<T>>.Current => throw new NotImplementedException();

            public void Dispose() { }

            public bool MoveNext()
            {
                throw new NotImplementedException();
            }

            public void Reset()
            {
                throw new NotImplementedException();
            }
        }


        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }


        public int Count => throw new NotImplementedException();


        public virtual bool IsReadOnly => false;


        public abstract void Add(T item);


        public abstract void Clear();


        public abstract bool Contains(T item);


        public abstract void CopyTo(T[] array, int arrayIndex);


        public abstract bool Remove(T item);


        /// <summary>
        /// Merges another <see cref="Bag{T}"/> into this. <typeparamref name="T"/> objects will be taken
        /// from <paramref name="other"/> and added into this until either this is full
        /// or <paramref name="other"/> is empty.
        /// </summary>
        /// <param name="other">Other <see cref="Bag{T}"/> to merge into this.</param>
        public abstract void Merge(ref Bag<T> other);


        /// <summary>
        /// Splits this into two <see cref="Bag{T}"/> by taking the first <paramref name="count"/>
        /// <typeparamref name="T"/> from this and putting them in a new <see cref="Bag{T}"/>.
        /// </summary>
        /// <returns><see cref="Bag{T}"/> containing the first <paramref name="count"/> <typeparamref name="T"/>
        /// of this.</returns>
        public abstract Bag<T> Split(int count);


        /// <summary>
        /// Returns the first <typeparamref name="T"/> in this.
        /// </summary>
        /// <returns>The first <typeparamref name="T"/> in this.</returns>
        public abstract T Take();
    }
}
