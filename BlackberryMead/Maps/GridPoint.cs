using Microsoft.Xna.Framework;
using System;
using System.Text.Json.Serialization;
using Point = Microsoft.Xna.Framework.Point;

namespace BlackberryMead.Maps
{
    /// <summary>
    /// A set of indeces that represent a location in a 2D grid.
    /// </summary>
    public struct GridPoint
    {
        /// <summary>
        /// Row index in the grid.
        /// </summary>
        [JsonInclude, JsonRequired]
        public int Row { get; set; }

        /// <summary>
        /// Column index in the grid.
        /// </summary>
        [JsonInclude, JsonRequired]
        public int Column { get; set; }


        /// <summary>
        /// Create a new <see cref="GridPoint"/>.
        /// </summary>
        /// <param name="row">Row index of the grid.</param>
        /// <param name="column">Column index of the grid.</param>
        public GridPoint(int row, int column)
        {
            Row = row;
            Column = column;
        }


        /// <summary>
        /// Converts the <see cref="GridPoint"/> to a <see cref="Vector2"/>.
        /// </summary>
        public Vector2 ToVector2()
        {
            return new Vector2(Column, Row);
        }


        /// <summary>
        /// Converts the <see cref="GridPoint"/> to a <see cref="Point"/>.
        /// </summary>
        public Point ToPoint()
        {
            return new Point(Column, Row);
        }


        /// <summary>
        /// Clamps the indeces of the <see cref="GridPoint"/> between an inclusive upper and inclusive lower limit.
        /// </summary>
        /// <param name="lower">Inclusive lower bound.</param>
        /// <param name="upper">Inclusive upper bound.</param>
        public void Clamp(int lower, int upper)
        {
            Row = Math.Max(lower, Row);
            Column = Math.Max(lower, Column);
            Row = Math.Min(upper, Row);
            Column = Math.Min(upper, Column);
        }


        /// <summary>
        /// Clamps the indeces of the <see cref="GridPoint"/> between independent inclusive upper and inclusive lower limits
        /// for both row and column indeces.
        /// </summary>
        /// <param name="lowerBounds">Lower bounds for <see cref="Row"/> and <see cref="Column"/>.</param>
        /// <param name="upperBounds">Upper bounds for <see cref="Row"/> and <see cref="Column"/>.</param>
        public void Clamp(GridPoint lowerBounds, GridPoint upperBounds)
        {
            ClampLower(lowerBounds);
            ClampUpper(upperBounds);
        }


        /// <summary>
        /// Clamps both indeces of the <see cref="GridPoint"/> to a single inclusive lower bound.
        /// </summary>
        /// <param name="lowerBound">Inclusive lower bound.</param>
        public void ClampLower(int lowerBound)
        {
            Row = Math.Max(lowerBound, Row);
            Column = Math.Max(lowerBound, Column);
        }


        /// <summary>
        /// Clamps the indeces of the <see cref="GridPoint"/> from below inclusively.
        /// </summary>
        /// <param name="lowerBounds"><see cref="GridPoint"/> containing the inclusive lower bounds for 
        /// <see cref="Row"/> and <see cref="Column"/> respectively.</param>
        public void ClampLower(GridPoint lowerBounds)
        {
            Row = Math.Max(lowerBounds.Row, Row);
            Column = Math.Max(lowerBounds.Column, Column);
        }


        /// <summary>
        /// Clamps both indeces of the <see cref="GridPoint"/> to a single inclusive upper bound.
        /// </summary>
        /// <param name="upperBound">Inclusive upper bound.</param>
        public void ClampUpper(int upperBound)
        {
            Row = Math.Min(upperBound, Row);
            Column = Math.Min(upperBound, Column);
        }


        /// <summary>
        /// Clamps the indeces of the <see cref="GridPoint"/> to a inclusive upper bounds.
        /// </summary>
        /// <param name="upperBounds"><see cref="GridPoint"/> containing the upper bound 
        /// for <see cref="Row"/> and <see cref="Column"/>.</param>
        public void ClampUpper(GridPoint upperBounds)
        {
            Row = Math.Min(upperBounds.Row, Row);
            Column = Math.Min(upperBounds.Column, Column);
        }


        public static GridPoint operator -(GridPoint a, GridPoint b) =>
            new GridPoint(a.Row - b.Row, a.Column - b.Column);


        public static GridPoint operator -(GridPoint a, int b) =>
            new GridPoint(a.Row - b, a.Column - b);


        public static GridPoint operator +(GridPoint a, GridPoint b) =>
            new GridPoint(a.Row + b.Row, a.Column + b.Column);

        public static GridPoint operator +(GridPoint a, int b) =>
            new GridPoint(a.Row + b, a.Column + b);


        public static GridPoint operator *(GridPoint a, GridPoint b) =>
            new GridPoint(a.Row * b.Row, a.Column * b.Column);


        public static GridPoint operator *(GridPoint a, MonoGame.Extended.Size x) =>
            new GridPoint(a.Row * x.Height, a.Column * x.Width);


        public static GridPoint operator *(GridPoint a, int x) =>
            new GridPoint(a.Row * x, a.Column * x);


        public static GridPoint operator /(GridPoint a, GridPoint b) =>
            new GridPoint(a.Row / b.Row, a.Column / b.Column);


        public static GridPoint operator /(GridPoint a, int x) =>
            new GridPoint(a.Row / x, a.Column / x);


        public static GridPoint operator %(GridPoint a, GridPoint b) =>
            new GridPoint(a.Row % b.Row, a.Column % b.Column);


        public static GridPoint operator %(GridPoint a, int x) =>
            new GridPoint(a.Row % x, a.Column % x);


        public static bool operator ==(GridPoint a, GridPoint b) =>
            a.Row == b.Row && a.Column == b.Column;


        public static bool operator !=(GridPoint a, GridPoint b) =>
            a.Row != b.Row || a.Column != b.Column;


        public override string ToString()
        {
            return "(" + Row + "," + Column + ")";
        }


        /// <summary>
        /// A <see cref="GridPoint"/> with zero valued indeces.
        /// </summary>
        public static GridPoint Zero
        {
            get { return new GridPoint(0, 0); }
        }

        /// <summary>
        /// Returns whether a <see cref="GridPoint"/> object's indeces are greater than or equal to zero and 
        /// less than or equal to upper limits.
        /// </summary>
        /// <param name="p"><see cref="GridPoint"/> to evaluate whether is in the range.</param>
        /// <param name="rowBound">Upper bound on the <see cref="Row"/> of <paramref name="p"/>.</param>
        /// <param name="columnBound">Upper bound on the <see cref="Column"/> of <paramref name="p"/>.</param>
        /// <returns></returns>
        public static bool InRange(GridPoint p, int rowBound, int columnBound)
        {
            return p.Row >= 0 && p.Column >= 0 && p.Row <= rowBound && p.Column <= columnBound;
        }


        public override bool Equals(object? obj)
        {
            if (obj == null) return false;

            GridPoint other = (GridPoint)obj!;
            return Row == other.Row && Column == other.Column;
        }


        public override int GetHashCode()
        {
            return Row.GetHashCode() ^ Column.GetHashCode();
        }
    }
}
