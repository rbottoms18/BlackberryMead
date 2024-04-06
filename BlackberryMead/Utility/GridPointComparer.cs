using BlackberryMead.Maps;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace BlackberryMead.Utility
{
    /// <summary>
    /// Comparer class to compare two <see cref="GridPoint"/>.
    /// </summary>
    public class GridPointComparer : IEqualityComparer<GridPoint>
    {
        /// <summary>
        /// Compare whether two <see cref="GridPoint"/>s are equal.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Equals(GridPoint x, GridPoint y)
        {
            return x.Row == y.Row && x.Column == y.Column;
        }

        /// <summary>
        /// Get the hash code of a <see cref="GridPoint"/>.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int GetHashCode([DisallowNull] GridPoint obj)
        {
            return obj.Row.GetHashCode() ^ obj.Column.GetHashCode();
        }
    }
}
