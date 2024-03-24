using BlackberryMead.Utility;

namespace BlackberryMead.Collections
{
    /// <summary>
    /// A <see cref="Box{T}"/> where <typeparamref name="T"/> is a <see cref="IStackable{T}"/>,
    /// a stackable collection of objects of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T"><see cref="INullImplementable{T}"/> type.</typeparam>
    /// <param name="size">Size of the <see cref="StackingBox{T}"/></param>
    public class StackingBox<T>(int size) : Box<IStackable<T>>(size) where T : INullImplementable<T>
    {
        /// <summary>
        /// Adds a <see cref="IStackable{T}"/> to the <see cref="StackingBox{T}"/>.
        /// </summary>
        /// <remarks>
        /// <paramref name="item"/> will be stacked with any equivalent <see cref="IStackable{T}"/>.
        /// If no such <see cref="IStackable{T}"/> exist, <paramref name="item"/> will be added to the
        /// first empty index.
        /// </remarks>
        /// <param name="item"><see cref="IStackable{T}"/> to add.</param>
        /// <returns><see langword="true"/> if any objects are added from <paramref name="item"/>;
        /// otherwise, if no stacking occurs, <see langword="false"/>.</returns>
        public override bool Add(IStackable<T> item)
        {
            // If the item is null, don't add it.
            if (item.IsNull()) return false;

            for (int i = 0; i < size; i++)
            {
                if (collection[i].IsStackableWith(item)) collection[i].Stack(item);
                if (item.Count == 0) break;
            }

            if (item.Count == 0) return true;

            // If there are no places it can be stacked, put in first empty slot.
            return base.Add(item);
        }


        /// <summary>
        /// Adds an <see cref="IStackable{T}"/> to the <see cref="StackingBox{T}"/> without stacking
        /// into any other <see cref="IStackable{T}"/>.
        /// </summary>
        /// <remarks>
        /// <paramref name="item"/> will be added to the first empty slot in the <see cref="IStackable{T}"/>.
        /// </remarks>
        /// <param name="item"></param>
        /// <returns><see langword="true"/> if <paramref name="item"/> is added to the <see cref="StackingBox{T}"/>;
        /// otherwise, <see langword="false"/>.</returns>
        public virtual bool DirectAdd(IStackable<T> item)
        {
            return base.Add(item);
        }


        /// <remarks>
        /// <see cref="IStackable{T}"/> objects of <paramref name="other"/> will be stacked
        /// with equivalent <see cref="IStackable{T}"/> objects in the <see cref="StackingBox{T}"/>
        /// if possible.
        /// </remarks>
        /// <inheritdoc cref="IStackable{T}.Stack(ref IStackable{T})"/>
        // Stack the StackingBox with another StackingBox -- NOT adding an IStackable to the StackingBox.
        public override bool Stack(IStackable<IStackable<T>> other)
        {
            bool stacked = false;

            // Foreach item in other
            foreach (IStackable<T> item in other)
            {
                // Try and stack with each stackable in this
                foreach (IStackable<T> stack in collection)
                {
                    if (!stack.IsStackableWith(item)) continue;

                    if (stack.Stack(item))
                        stacked = true;
                }
            }

            return stacked;
        }
    }
}
