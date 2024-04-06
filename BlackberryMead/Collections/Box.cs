using BlackberryMead.Framework;
using BlackberryMead.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BlackberryMead.Collections
{
    /// <summary>
    /// Represents a strongly typed, fixed-size collection of objects that implements a top-down
    /// structure where objects are be added to the <see cref="Box{T}"/> at the lowest index possible
    /// and drawn from the greatest index possible.
    /// </summary>
    /// <typeparam name="T">Type of object to be stored.</typeparam>
    /// <param name="size">Number of items able to be stored in the <see cref="Box{T}"/>.</param>
    public class Box<T> : IEnumerable<T>, INullImplementable<Box<T>>, IStackable<T>
        where T : INullImplementable<T>
    {
        public static Box<T> Null => new Box<T>(0);

        public virtual bool IsNull => (Count == 0);

        /// <summary>
        /// Amount of objects in the <see cref="Box{T}"/>.
        /// </summary>
        public virtual int Count { get => count; set => count = value; }

        int IQuantifiable.Count => count;

        /// <summary>
        /// Number of objects that can fit in the <see cref="Box{T}"/>.
        /// </summary>
        public int Size { get; protected set; }

        int IStackable<T>.MaxStackSize => MaxStackSize;

        /// <summary>
        /// Maximum number of objects that can be stacked in the <see cref="Box{T}"/>.
        /// </summary>
        public virtual int MaxStackSize => maxStackSize;

        /// <summary>
        /// <see langword="true"/> if the <see cref="Box{T}"/> contains no non-null items; otherwise,
        /// <see langword="false"/>.
        /// </summary> 
        public virtual bool IsEmpty => count == 0;

        /// <summary>
        /// Collection of <see cref="T"/> this holds.
        /// </summary>
        protected virtual T[] collection { get; set; }

        /// <summary>
        /// Number of objects in this.
        /// </summary>
        protected int count;

        /// <summary>
        /// Maximum number of objects than can be stored in the <see cref="FlatStack{T}"/>.
        /// </summary>
        protected int maxStackSize;


        public Box(List<T> items, int size = 0) : this(size == 0 ? items.Count : size)
        {
            maxStackSize = size;

            for (int i = 0; i < items.Count; i++)
            {
                // Set directly because the collection is null
                collection[i] = items[i];
                // Only increase the count if the item is not null.
                if (!items[i].IsNull)
                    Count++;
            }
        }


        protected Box() { }


        public Box(int size)
        {
            collection = ArrayHelper.FillArray(new T[size], T.Null);
            this.Size = size;
        }


        /// <summary>
        /// Gets the <typeparamref name="T"/> at the specified <paramref name="index"/>.
        /// </summary>
        /// <param name="index">Index to retrieve the <typeparamref name="T"/> at.</param>
        /// <returns><typeparamref name="T"/> at the specified <paramref name="index"/>.</returns>
        /// <exception cref="IndexOutOfRangeException"> Throws an exception if <paramref name="index"/> is
        /// greater or equal to <see cref="Size"/>. </exception>
        public virtual T this[int index]
        {
            get
            {
                if (index >= Size)
                    throw new IndexOutOfRangeException();
                return collection[index];
            }
            set
            {
                if (index >= Size)
                    throw new IndexOutOfRangeException();
                collection[index] = value;
            }
        }


        /// <remarks>Base implementation always returns <see langword="true"/>.</remarks>
        /// <returns><see langword="true"/>.</returns>
        /// <inheritdoc cref="IStackable{T}.IsStackableWith(IStackable{T})"/>
        public virtual bool IsStackableWith(IStackable<T> other)
        {
            return true;
        }


        public virtual bool Add(T item)
        {
            // If the item is null, don't add it.
            if (item.IsNull) return false;

            for (int i = 0; i < Size; i++)
            {
                // If the collection item is not null, we can't add anything there.
                if (!collection[i].IsNull) continue;

                collection[i] = item;
                // Added an item so the count goes up.
                Count++;
                return true;
            }
            return false;
        }


        /// <summary>
        /// Gets whether a <typeparamref name="T"/> is contained in the <see cref="Box{T}"/>.
        /// </summary>
        /// <param name="item"><typeparamref name="T"/> to check if is in <see cref="Box{T}"/>.</param>
        /// <returns><see langword="true"/> if the <see cref="Box{T}"/> contains <paramref name="item"/>;
        /// otherwise, false.</returns>
        public virtual bool Contains(T item)
        {
            foreach (T t in collection)
            {
                if (t.Equals(item)) return true;
            }
            return false;
        }


        /// <summary>
        /// Removes a <typeparamref name="T"/> from the <see cref="Box{T}"/>.
        /// </summary>
        /// <remarks>Equality is checked using Reference Equality.</remarks>
        /// <param name="item"><typeparamref name="T"/> to remove from the <see cref="Box{T}"/>.</param>
        /// <returns><see langword="true"/> if <paramref name="item"/> is removed from the <see cref="Box{T}"/>;
        /// otherwise, <see langword="false"/>.</returns>
        public virtual bool Remove(T item)
        {
            for (int i = Size - 1; i >= 0; i--)
            {
                if (ReferenceEquals(item, collection[i]))
                {
                    collection[i] = T.Null;
                    Count--;
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Removes all occurances of of a specific object from the <see cref="Box{T}"/>.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="Box{T}"/>.</param>
        /// <returns><see langword="true"/> if an instance of <paramref name="item"/> was successfully removed
        /// from the <see cref="Box{T}"/>; otherwise, <see langword="false"/>. Also returns 
        /// <see langword="false"/> if no instances of <paramref name="item"/> were found.</returns>
        public virtual bool RemoveAll(T item)
        {
            bool removed = false;
            for (int i = 0; i < Size; i++)
            {
                if (item.Equals(collection[i]))
                {
                    collection[i] = T.Null;
                    Count--;
                    removed = true;
                }
            }
            return removed;
        }


        /// <summary>
        /// Returns the first non-<see cref="T.Null"/> <typeparamref name="T"/> in the <see cref="Box{T}"/>.
        /// </summary>
        /// <returns>The first non-<see cref="T.Null"/> <typeparamref name="T"/> in the <see cref="Box{T}"/>, taken from the
        /// top of the box (highest index).
        /// If no non-<see cref="T.Null"/> <typeparamref name="T"/> is found, <see cref="T.Null"/> will
        /// be returned instead.</returns>
        public virtual T Take()
        {
            for (int i = Size - 1; i >= 0; i--)
            {
                if (collection[i].IsNull) continue;
                Count--;
                T item = collection[i];
                collection[i] = T.Null;
                // Will this return Null? Don't think it should.
                return item;
            }
            return T.Null;
        }


        public virtual bool Remove(int amount)
        {
            int removed = 0;
            for (int i = Size - 1; i >= 0; i--)
            {
                if (collection[i].IsNull) continue;
                Count--;
                removed++;
                collection[i] = T.Null;

                if (removed == amount) return true;
            }
            return false;
        }


        /// <remarks>
        /// Takes the first <paramref name="amount"/>  <typeparamref name="T"/> from the 
        /// <see cref="Box{T}"/> and places them in a new <see cref="Box{T}"/>.
        /// </remarks>
        /// <inheritdoc cref="IStackable{T}.Split(int)"/>
        public virtual IStackable<T> Split(int amount)
        {
            if (Size == 0)
                Size = count;

            Box<T> newBox = new Box<T>(Size);
            for (int i = 0; i < Math.Min(count, amount); i++)
            {
                newBox.Add(Take());
            }
            return newBox;
        }


        public virtual bool Stack(IStackable<T> other)
        {
            return Stack(other, out _);
        }


        public virtual bool Stack(IStackable<T> other, out IStackable<T> values)
        {
            Box<T> addedItems = new Box<T>(other.Count);
            // Assign added to addedItems so they share the same reference.
            values = addedItems;
            int index = 0;

            int startingCount = other.Count;
            for (int i = 0; i < startingCount; i++)
            {
                // If there are no more slots avaliable, return.
                if (Count == 0) return false;

                // If count > 0, it's gauranteed that an item can fit.
                T item = other.Take();
                Add(item);
                addedItems[index] = item;

                // If the other is empty, return.
                if (other.Count == 0)
                    return true;
            }

            // If the ending count is less than the starting count,
            // some objects were stacked.
            if (other.Count < startingCount)
                return true;


            values = addedItems;
            return false;
        }


        /// <summary>
        /// Moves a number of objects from the bottom of the <see cref="Box{T}"/> to the top.
        /// </summary>
        /// <param name="x">Number of <typeparamref name="T"/> to move to the top of the box.</param>
        public virtual void Permute(int x)
        {
            List<T> _ = collection.ToList();
            List<T> top = _.Take(x).ToList();
            _.RemoveRange(0, x);
            _ = _.Concat(top).ToList();
            collection = _.ToArray();
        }


        public IEnumerator<T> GetEnumerator()
        {
            for (int i = Size - 1; i >= 0; i--)
            {
                yield return collection[i];
            }
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public bool Stack(IStackable<T> other, int amount)
        {
            throw new NotImplementedException();
        }
    }
}
