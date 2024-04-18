using BlackberryMead.Maps;
using System.Collections.Generic;
using System.Linq;

namespace BlackberryMead.Collections
{
    /// <summary>
    /// 2D grid of <see cref="HashSet{T}"/> objects.
    /// </summary>
    public class HashGrid<T>
    {
        /// <summary>
        /// Grid of <see cref="HashSet{T}"/> objects.
        /// </summary>
        protected HashSet<T>[,] grid;


        /// <summary>
        /// Create a new <see cref="HashGrid{T}"/>.
        /// </summary>
        /// <param name="rows">Number of rows in the grid.</param>
        /// <param name="cols">Number of columns in the grid.</param>
        public HashGrid(int rows, int cols)
        {
            grid = new HashSet<T>[rows, cols];
        }


        /// <summary>
        /// Gets the Hashset <typeparamref name="T"/> of objects at a given grid location.
        /// </summary>
        /// <param name="p">Coordinate position in the <see cref="HashGrid{T}"/>.</param>
        /// <returns>A non-null Hashset<typeparamref name="T"/> at index <paramref name="p"/>.</returns>
        public HashSet<T> this[GridPoint p]
        {
            get
            {
                HashSet<T> result = grid[p.Row, p.Column] == null ? new HashSet<T>() : grid[p.Row, p.Column];
                return result;
            }
            protected set { grid[p.Row, p.Column] = value; }
        }


        /// <summary>
        /// Adds the object to a position in the <see cref="HashGrid{T}"/>.
        /// </summary>
        /// <param name="obj">Object to be added.</param>
        /// <param name="p">Index to add the object.</param>
        /// <returns><see langword="true"/> when the object already existed in the grid; otherwise, <see langword="false"/>.</returns>
        public bool Add(T obj, GridPoint p)
        {
            if (p.Row >= grid.GetUpperBound(0) || p.Column >= grid.GetUpperBound(1) || p.Row < 0 || p.Column < 0) return false;
            if (grid[p.Row, p.Column] == null)
                grid[p.Row, p.Column] = new HashSet<T>();
            return grid[p.Row, p.Column].Add(obj);
        }


        /// <summary>
        /// Removes the object from a position in the <see cref="HashGrid{T}"/>.
        /// </summary>
        /// <param name="obj">Object to be removed.</param>
        /// <param name="p">Index to remove the object at.</param>
        /// <returns><see langword="true"/> if the object was successfully removed, <see langword="false"/> if the 
        /// object was not found</returns>
        public bool Remove(T obj, GridPoint p)
        {
            if (grid[p.Row, p.Column] == null)
                return false;

            if (grid[p.Row, p.Column].Remove(obj))
            {
                if (grid[p.Row, p.Column].Count == 0)
                    grid[p.Row, p.Column] = null;
                return true;
            }

            return false;
        }


        /// <summary>
        /// Removes an object from a set of indeces.
        /// </summary>
        /// <param name="obj">Object to be removed.</param>
        /// <param name="points">List of indeces to remove the object at.</param>
        /// <returns><see langword="true"/> if the object was removed from at least one index; otherwise, <see langword="false"/>.</returns>
        public bool Remove(T obj, List<GridPoint> points)
        {
            bool _ = false;
            foreach (GridPoint p in points)
                if (Remove(obj, p))
                    _ = true;
            return _;
        }


        /// <summary>
        /// Gets the union of the <see cref="HashSet{T}"/> objects at at list of indeces.
        /// </summary>
        /// <param name="points">Indeces to union <see cref="HashSet{T}"/> objects from.</param>
        /// <returns>A <see cref="HashSet{T}"/> containing all the objects in the union.</returns>
        public HashSet<T> GetSetUnion(List<GridPoint> points)
        {
            HashSet<T> set = new HashSet<T>();
            foreach (GridPoint p in points)
            {
                HashSet<T> union = this[p];
                set = new HashSet<T>(set.Union(union));
            }
            return set;
        }
    }
}
