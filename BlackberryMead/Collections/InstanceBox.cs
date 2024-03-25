using BlackberryMead.Utility;
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
        public T Value => value;

        public new int Count
        {
            get { return count; }
            set
            {
                count = value;
                // If the count goes to zero, make this stack become Null
                if (count == 0)
                    this.value = T.Null;
            }
        }

        /// <summary>
        /// <see cref="T"/> instance that is stacked in this.
        /// </summary>
        private T value;


        public override bool Add(T item)
        {
            if (base.Add(item))
            {
                // If the stack instance is null and param item isn't, set param item as the
                // new instance
                if (Value.IsNull())
                    this.value = item;
                return true;
            }
            return false;
        }


        public override bool IsStackableWith(IStackable<T> other)
        {
            // Stackable if other is also a UniqueItemStack and they both have the
            // same instance item.
            return (other is InstanceBox<T> stack) && value.Equals(stack.Value);
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


        public override bool IsNull()
        {
            return count == 0 || value.IsNull();
        }
    }
}
