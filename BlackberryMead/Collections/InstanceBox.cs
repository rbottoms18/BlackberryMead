using BlackberryMead.Framework;
using System;

namespace BlackberryMead.Collections
{
    /// <summary>
    /// Implementation of <see cref="Box{T}"/> that stores a set of equivalent instances of an object of
    /// type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T"><see cref="INullImplementable{T}"/> type that implements <see cref="IEquatable{T}"/>.</typeparam>
    /// <param name="size">Size of the see <see cref="InstanceBox{T}"/>.</param>
    public class InstanceBox<T>(int size) : Box<T>(size), IInstanceStackable<T> where T : INullImplementable<T>, IEquatable<T>
    {
        public override bool IsNull => Count == 0 || Value.IsNull;

        public T Value => item;

        T IInstanceStackable<T>.Value => item;

        public override int Count
        {
            get => base.Count;
            set
            {
                base.Count = value;
                if (Count == 0)
                    this.item = T.Null;
            }
        }

        /// <summary>
        /// <see cref="T"/> instance that is stacked in this.
        /// </summary>
        protected T item;


        /// <returns><see langword="true"/> if <paramref name="item"/> is equatable with <see cref="Value"/>
        /// and is successfully stacked into the <see cref="InstanceBox{T}"/>; otherwise, <see langword="false"/>.</returns>
        /// <inheritdoc cref="IStackable{T}.Add(T)"/>
        public override bool Add(T item)
        {
            // If this is a null stack, make it a new stack of item.
            if (Value.IsNull)
            {
                this.item = item;
                return base.Add(item);
            }

            if (!item.Equals(Value)) return false;

            return base.Add(item);
        }


        public override bool IsStackableWith(IStackable<T> other)
        {
            // Stackable if other is also a UniqueItemStack and they both have the
            // same instance item.
            return (other is InstanceBox<T> stack) && item.Equals(stack.Value);
        }


        public override InstanceBox<T> Split(int amount)
        {
            InstanceBox<T> newStack = new InstanceBox<T>(amount);

            for (int i = 0; i < Math.Min(amount, count); i++)
            {
                newStack.Add(Take());
            }

            return newStack;
        }
    }
}
