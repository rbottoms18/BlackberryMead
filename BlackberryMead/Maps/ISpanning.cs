using BlackberryMead.Utility;
using System.Collections.Generic;

namespace BlackberryMead.Maps
{
    /// <summary>
    /// Marks an object as spaning a region in <see cref="Maps.Map2D{T}"/>-space.
    /// </summary>
    public interface ISpanning
    {
        /// <summary>
        /// Gets the span of the object in units <see cref="GridPoint"/> as displacement from the object's origin.
        /// </summary>
        /// <remarks>
        /// The object's origin is where the object is placed within the <see cref="Maps.Map2D{T}"/>.
        /// </remarks>
        /// <example>
        /// If the object's span in Grid units is 2x2 and the origin is the bottom left of the object,
        /// then the Span would get the <see cref="GridPoint"/>s {(0, 0), (0, 1), (1, 0), (1, 1)}.
        /// </example>
        abstract List<GridPoint> GetSpan();

        /// <summary>
        /// Subset of the <see cref="Span"/> for which the object is to be impassible.
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <example>
        /// If the span of an object is {(0, 0), (0, 1), (1, 0), (1, 1)} and the object is to be impassible at it's origin,
        /// then the Hitboxes would be {(0, 0)}.
        /// </example>
        // The <see cref="Hitboxes"/> is a subset of the <see cref="Span"/> because it makes little sense for an
        // object to be impassible somewhere where it is not "is".
        abstract List<GridPoint> Hitboxes { get; }

        /// <summary>
        /// Dimensions of the Object.
        /// </summary>
        Size Dimensions { get; init; }
    }
}
