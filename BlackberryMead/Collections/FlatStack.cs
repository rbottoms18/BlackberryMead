using BlackberryMead.Utility;
using System;
using System.Collections;
using System.Collections.Generic;

namespace BlackberryMead.Collections
{
    /// <summary>
    /// An <see cref="IStackable{T}"/> that tracks quantities added to it.
    /// </summary>
    /// <remarks>
    /// <see cref="FlatStack{T}"/> does not keep or remember any <typeparamref name="T"/> added to it.
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    public class FlatStack<T>(int count = 0) : IStackable<T> where T : INullImplementable<T>
    {
        public int MaxStackSize => int.MaxValue;

        public T Value => T.Null;

        public int Count => count;

        /// <summary>
        /// Number of objects contained in this.
        /// </summary>
        protected int count = count;


        public virtual bool Add(T item)
        {
            if (count < MaxStackSize)
            {
                count++;
                return true;
            }
            return false;
        }


        public virtual bool IsNull()
        {
            return Count == 0;
        }


        /// <remarks>
        /// Default implementation returns <see langword="true"/> always.
        /// </remarks>
        /// <returns> <see langword="true"/>. </returns>
        /// <inheritdoc cref="IStackable{T}.IsStackableWith(IStackable{T})"/>
        public virtual bool IsStackableWith(IStackable<T> other)
        {
            return true;
        }


        public virtual bool Remove(int amount)
        {
            count -= amount;
            if (count < 0)
                count = 0;
            return true;
        }


        public virtual IStackable<T> Split(int amount)
        {
            FlatStack<T> stack = new FlatStack<T>();

            // Return the fewest number of objects between amount and count
            for (int i = 0; i < Math.Min(amount, count); i++)
            {
                // If the stack fails to add, means the collection is full.
                if (!stack.Add(Take())) break;
            }

            return stack;
        }


        public virtual bool Stack(IStackable<T> other)
        {
            return Stack(other, out _);
        }


        public virtual bool Stack(IStackable<T> other, out IStackable<T> values)
        {
            bool stackAdded = false;
            int otherCount = other.Count;
            for (int i = 0; i < otherCount; i++)
            {
                if (Add(other.Take()))
                    stackAdded = true;
                else
                    // If the stack fails to add it means collection is full.
                    break;
            }

            values = new FlatStack<T>(otherCount - other.Count);
            return stackAdded;
        }


        /// <returns>
        /// Returns <see cref="INullImplementable{T}.Null"/>.
        /// </returns>
        /// <inheritdoc cref="IStackable{T}.Take"/>
        public virtual T Take()
        {
            return T.Null;
        }


        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < count; i++)
            {
                yield return T.Null;
            }
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
