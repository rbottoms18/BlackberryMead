using BlackberryMead.Framework;
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
    public class FlatStack<T> : IStackable<T> where T : INullImplementable<T>
    {
        public virtual bool IsNull => Count == 0;

        public int MaxStackSize => maxStackSize;

        int IStackable<T>.MaxStackSize => maxStackSize;

        public virtual int Count { get { return count; } set { count = value; } }

        int IQuantifiable.Count => count;

        /// <summary>
        /// Number of objects contained in this.
        /// </summary>
        private int count;

        /// <summary>
        /// Maximum number of objects than can be stored in the <see cref="FlatStack{T}"/>.
        /// </summary>
        protected virtual int maxStackSize => int.MaxValue;


        public FlatStack(int count)
        {
            this.count = count;
        }


        public virtual bool Add(T item)
        {
            if (count < maxStackSize)
            {
                count++;
                return true;
            }
            return false;
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
            Count -= amount;
            if (Count < 0)
                Count = 0;
            return true;
        }


        public virtual bool Remove(T item)
        {
            return Remove(1);
        }


        public virtual IStackable<T> Split(int amount)
        {
            FlatStack<T> stack = new FlatStack<T>(0);

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
            count--;
            return T.Null;
        }


        public virtual IEnumerator<T> GetEnumerator()
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
