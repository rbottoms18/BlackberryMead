using BlackberryMead.Framework;
using System;

namespace BlackberryMead.Collections
{
    /// <summary>
    /// A <see cref="Box{T}"/> where <typeparamref name="T"/> is a <see cref="IInstanceStackable{T}"/>,
    /// a stackable collection of objects of a specific instance of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T"><see cref="INullImplementable{T}"/> type.</typeparam>
    /// <param name="size">Size of the <see cref="StackingBox{T}"/></param>
    public class StackingBox<T>(int size) : Box<IInstanceStackable<T>>(size) 
        where T : INullImplementable<T>, IEquatable<T>
    {
        /// <summary>
        /// Adds a <see cref="IInstanceStackable{T}"/> to the <see cref="StackingBox{T}"/>.
        /// </summary>
        /// <remarks>
        /// <paramref name="item"/> will be stacked with any equivalent <see cref="IInstanceStackable{T}"/>.
        /// If no such <see cref="IInstanceStackable{T}"/> exist, <paramref name="item"/> will be added to the
        /// first empty index.
        /// </remarks>
        /// <param name="item"><see cref="IInstanceStackable{T}"/> to add.</param>
        /// <returns><see langword="true"/> if any objects are added from <paramref name="item"/>;
        /// otherwise, if no stacking occurs, <see langword="false"/>.</returns>
        public override bool Add(IInstanceStackable<T> item)
        {
            // If the item is null, don't add it.
            if (item.IsNull()) return false;

            for (int i = 0; i < Size; i++)
            {
                if (collection[i].IsStackableWith(item)) collection[i].Stack(item);
                if (item.Count == 0) break;
            }

            if (item.Count == 0) return true;

            // If there are no places it can be stacked, put in first empty slot.
            return base.Add(item);
        }


        /// <summary>
        /// Adds an <see cref="IInstanceStackable{T}"/> to the <see cref="StackingBox{T}"/> without stacking
        /// into any other <see cref="IInstanceStackable{T}"/>.
        /// </summary>
        /// <remarks>
        /// <paramref name="item"/> will be added to the first empty slot in the <see cref="IInstanceStackable{T}"/>.
        /// </remarks>
        /// <param name="item"></param>
        /// <returns><see langword="true"/> if <paramref name="item"/> is added to the <see cref="StackingBox{T}"/>;
        /// otherwise, <see langword="false"/>.</returns>
        public virtual bool DirectAdd(IInstanceStackable<T> item)
        {
            return base.Add(item);
        }


        /// <remarks>
        /// <see cref="IInstanceStackable{T}"/> objects of <paramref name="other"/> will be stacked
        /// with equivalent <see cref="IInstanceStackable{T}"/> objects in the <see cref="StackingBox{T}"/>
        /// if possible.
        /// </remarks>
        /// <inheritdoc cref="IInstanceStackable{T}.Stack(ref IInstanceStackable{T})"/>
        // Stack the StackingBox with another StackingBox -- NOT adding an IStackable to the StackingBox.
        public override bool Stack(IStackable<IInstanceStackable<T>> other)
        {
            return Stack(other, out _);
        }


        /// <param name="values">Objects that were added to the <see cref="IStackable{T}"/>. 
        /// Returns as a <see cref="StackingBox{T}"/> containing <see cref="InstanceBox{T}"/>.</param>
        /// <inheritdoc cref="IStackable{T}.Stack(IStackable{T}, out IStackable{T})"/>
        public override bool Stack(IStackable<IInstanceStackable<T>> other, out IStackable<IInstanceStackable<T>> values)
        {
            bool stacked = false;

            StackingBox<T> added = new StackingBox<T>(other.Count);
            // Assign values to added
            values = added;

            // Foreach item in other
            foreach (IInstanceStackable<T> item in other)
            {
                // Initialize a box to hold objects stacked from item.
                InstanceBox<T> box = new InstanceBox<T>(item.Count);

                // Try and stack with each stackable in this
                foreach (IInstanceStackable<T> stack in collection)
                {
                    if (!stack.IsStackableWith(item)) continue;

                    if (stack.Stack(item, out IStackable<T> addedStacks))
                    {
                        stacked = true;
                        // item is constant so objects taken from it and stacked into the StackingBox
                        // can be stacked together
                        box.Stack(addedStacks);
                    }
                }

                added.Add(box);
            }

            return stacked;
        }
    }
}
