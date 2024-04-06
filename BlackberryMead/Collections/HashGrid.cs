using BlackberryMead.Maps;
using System.Collections.Generic;
using System.Linq;

namespace BlackberryMead.Collections
{
    /// <summary>
    /// Represents a 2D grid of <see cref="HashSet{T}"/> objects.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class HashGrid<T>
    {
        public HashGrid(int rows, int cols)
        {
            grid = new HashSet<T>[rows, cols];
        }

        protected HashSet<T>[,] grid;


        /// <summary>
        /// Gets the Hashset <typeparamref name="T"/> 'bucket' of objects at this grid location.
        /// </summary>
        /// <param name="p">Coordinate vector in the grid</param>
        /// <returns>A non-null Hashset<typeparamref name="T"/></returns>
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
        /// Adds the object to a position in the grid.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="p"></param>
        /// <returns>true when the object already existed in the grid, false otherwise</returns>
        public bool Add(T obj, GridPoint p)
        {
            if (p.Row >= grid.GetUpperBound(0) || p.Column >= grid.GetUpperBound(1) || p.Row < 0 || p.Column < 0) return false;
            if (grid[p.Row, p.Column] == null)
                grid[p.Row, p.Column] = new HashSet<T>();
            return grid[p.Row, p.Column].Add(obj);
        }


        /// <summary>
        /// Removes the object from the position in the grid.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="p"></param>
        /// <returns>true if the object was successfully removed, false if the object was not found</returns>
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


        public bool Remove(T obj, List<GridPoint> points)
        {
            bool result = false;
            foreach (GridPoint p in points)
                if (Remove(obj, p))
                    result = true;
            return result;
        }


        /// <summary>
        /// Gets the union of the sets of objects of type <typeparamref name="T"/> in 
        /// indexes in 'points'.
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
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
