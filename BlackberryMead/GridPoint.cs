#nullable enable
using Microsoft.Xna.Framework;
using System;
using System.Text.Json.Serialization;
using Point = Microsoft.Xna.Framework.Point;

namespace BlackberryMead
{
    /// <summary>
    /// A point struct that represents a position in a grid.
    /// </summary>
    public struct GridPoint
    {
        /// <summary>
        /// Row in the grid.
        /// </summary>
        [JsonInclude, JsonRequired]
        public int Row { get; set; }

        /// <summary>
        /// Column in the grid.
        /// </summary>
        [JsonInclude, JsonRequired]
        public int Column { get; set; }


        /// <summary>
        /// Create a new GridPoint.
        /// </summary>
        /// <param name="row">Row of the grid.</param>
        /// <param name="column">Column of the grid.</param>
        public GridPoint(int row, int column)
        {
            Row = row;
            Column = column;
        }


        /// <summary>
        /// Converts this to a <see cref="Microsoft.Xna.Framework.Vector2"/>.
        /// </summary>
        public Vector2 ToVector2()
        {
            return new Vector2(Column, Row);
        }


        /// <summary>
        /// Converts this to a <see cref="Microsoft.Xna.Framework.Point"/>.
        /// </summary>
        public Point ToPoint()
        {
            return new Point(Column, Row);
        }


        /// <summary>
        /// Clamps this values between an inclusive upper and lower limit.
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
        /// Clamps this between inclusive upper and lower bounds for both row and column.
        /// </summary>
        /// <param name="lowerBounds"></param>
        /// <param name="upperBounds"></param>
        public void Clamp(GridPoint lowerBounds, GridPoint upperBounds)
        {
            ClampLower(lowerBounds);
            ClampUpper(upperBounds);
        }


        /// <summary>
        /// Clamps this values with an inclusive lower bound.
        /// </summary>
        /// <param name="lowerBound">Inclusive lower bound.</param>
        public void ClampLower(int lowerBound)
        {
            Row = Math.Max(lowerBound, Row);
            Column = Math.Max(lowerBound, Column);
        }


        /// <summary>
        /// Clamps this from below inclusively against values in another GridPoint.
        /// </summary>
        /// <param name="lowerBounds">Inclusive lower bounds for Row and Column respectively.</param>
        public void ClampLower(GridPoint lowerBounds)
        {
            Row = Math.Max(lowerBounds.Row, Row);
            Column = Math.Max(lowerBounds.Column, Column);
        }


        /// <summary>
        /// Clamps this values with an inclusive upper bound.
        /// </summary>
        /// <param name="upperBound">Inclusive upper bound.</param>
        public void ClampUpper(int upperBound)
        {
            Row = Math.Min(upperBound, Row);
            Column = Math.Min(upperBound, Column);
        }


        /// <summary>
        /// Clamps this from above inclusively against values in another GridPoint.
        /// </summary>
        /// <param name="upperBounds"></param>
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


        public static GridPoint Zero
        {
            get { return new GridPoint(0, 0); }
        }

        /// <summary>
        /// Returns whether a <see cref="GridPoint"/>'s components are greater than or equal to zero and 
        /// less than or equal to upper limits.
        /// </summary>
        /// <param name="p">Point to evaluate whether is in the range.</param>
        /// <param name="rowBound">Upper bound on the row of <paramref name="p"/>.</param>
        /// <param name="columnBound">Upper bound on the column of <paramref name="p"/>.</param>
        /// <returns></returns>
        public static bool InRange(GridPoint p, int rowBound, int columnBound)
        {
            return p.Row >= 0 && p.Column >= 0 && p.Row <= rowBound && p.Column <= columnBound;
        }


        public override bool Equals(object? obj)
        {
            if (obj == null) return false;

            GridPoint other = (GridPoint)obj!;
            return (Row == other.Row && Column == other.Column);
        }


        public override int GetHashCode()
        {
            return Row.GetHashCode() ^ Column.GetHashCode();
        }
    }
}
