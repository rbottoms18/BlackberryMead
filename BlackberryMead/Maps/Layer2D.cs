using BlackberryMead.Framework;
using BlackberryMead.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BlackberryMead.Maps
{
    /// <summary>
    /// A 2d array of objects of type <typeparamref name="T"/> and corresponding <c>passability</c>
    /// information.
    /// </summary>
    /// <typeparam name="T">Type of object to be contained in this.</typeparam>
    /// <remarks>
    /// Used as a component in a <see cref="Map2D{T}"/>.
    /// </remarks>
    public class Layer2D<T> where T : IMapObject<T>
    {
        /// <summary>
        /// Name of the layer.
        /// </summary>
        [JsonInclude]
        public string Name { get; set; }

        /// <summary>
        /// Number of rows in this.
        /// </summary>
        [JsonInclude]
        public int Rows { get; protected set; }

        /// <summary>
        /// Number of columns in this.
        /// </summary>
        [JsonInclude]
        public int Columns { get; protected set; }

        /// <summary>
        /// 2d array of objects of type <see cref="T"/> contained in this.
        /// </summary>
        [JsonInclude]
        public T[][] Values { get; protected set; }

        /// <summary>
        /// Width of a tile.
        /// </summary>
        public int TileWidth { get; set; }

        /// <summary>
        /// Height of a tile.
        /// </summary>
        public int TileHeight { get; set; }

        /// <summary>
        /// Value that corresponds to a 'null' pointer.
        /// </summary>
        /// <remarks>
        /// A 'null' pointer is one that does not point to any object, i.e. there is no object at that tile.
        /// </remarks>
        public readonly GridPoint NullPointer = new GridPoint(-1, -1);

        /// <summary>
        /// Grid of <see cref="GridPoint"/> that "point" to a different location in the layer
        /// where the location of the object that spans into the indexed location is set.
        /// </summary>
        /// <remarks>
        /// When an <see cref="ISpanning"/> has a multi-tile span, in order for it to be successfully interacted with
        /// at any tile in it's <see cref="ISpanning.Span"/>, it needs to be set in every tile it spans in the layer. But simply 
        /// setting each tile it spans' object to the <see cref="ISpanning"/> would cause it be drawn 
        /// multiple times at different tiles. Instead, we "point" to where the object is, resulting in it only being set, and
        /// subsequently drawn, once.
        /// </remarks>
        // This does not need to be serialized because it will be reconstructed when pointers are set (also not serialized).
        protected GridPoint[,] pointers;

        /// <summary>
        /// <see cref="INullImplementable{T}"/> null object of type <see cref="T"/>.
        /// </summary>
        protected T Null = T.Null;


        /// <summary>
        /// Create a new Layer.
        /// </summary>
        /// <param name="Rows">Number of rows in the layer.</param>
        /// <param name="Columns">Number of columns in the layer.</param>
        /// <param name="tileDim">Dimensions of a tile in the layer.</param>
        public Layer2D(int Rows, int Columns)
        {
            this.Rows = Rows;
            this.Columns = Columns;

            // Initialize jagged values
            Values = new T[Rows][];
            for (int i = 0; i < Rows; i++)
            {
                Values[i] = new T[Columns];
            }
            pointers = new GridPoint[Rows, Columns];

            // Fill entities with the Null object.
            ArrayHelper.FillArray(Values, Null);

            // Fill pointers with NullPointer reference
            ArrayHelper.FillArray(pointers, NullPointer);
        }


        /// <summary>
        /// Create a new Layer from file.
        /// </summary>
        /// <param name="Rows"></param>
        /// <param name="Columns"></param>
        /// <param name="TileWidth"></param>
        /// <param name="TileHeight"></param>
        /// <param name="Values"></param>
        [JsonConstructor]
        public Layer2D(int Rows, int Columns, T[][] Values)
        {
            this.Rows = Rows;
            this.Columns = Columns;
            this.Values = Values;
            pointers = ArrayHelper.NewFilledArray(Rows, Columns, NullPointer);

            for (int row = 0; row < Values.Length; row++)
            {
                for (int col = 0; col < Values[0].Length; col++)
                {
                    if (Values[row][col].IsNull)
                        continue;
                    GridPoint p = new GridPoint(row, col);
                    Set(Values[row][col], p);
                }
            }
        }


        /// <summary>
        /// Get the <typeparamref name="T"/> at the specified index.
        /// </summary>
        /// <param name="row">Row in the layer.</param>
        /// <param name="col">Column in the layer.</param>
        /// <returns><typeparamref name="T"/> at position (<paramref name="row"/>, <paramref name="col"/>) in this.</returns>
        public T this[int row, int col]
        {
            get
            {
                if (row >= Rows || col >= Columns || row < 0 || col < 0)
                    return Null;

                return ObjectAt(row, col);
            }
        }


        /// <summary>
        /// Get the <typeparamref name="T"/> at the specified index.
        /// </summary>
        /// <param name="p">GridPoint position to get the <typeparamref name="T"/> at.</param>
        /// <returns><typeparamref name="T"/> at position <paramref name="p"/> in this.</returns>
        public T this[GridPoint p]
        {
            get
            {
                if (p.Row >= Rows || p.Column >= Columns || p.Row < 0 || p.Column < 0)
                    return Null;

                return ObjectAt(p);
            }
        }


        /// <summary>
        /// Set pointers for a <typeparamref name="T"/> in the surrounding tiles.
        /// </summary>
        /// <param name="obj"><see cref="T"/> to set pointers for.</param>
        /// <param name="loc">Position in the grid to set <paramref name="obj"/>.</param>
        /// <param name="pointerValue">Value of the pointer to set. <br/>
        /// To set pointers to <paramref name="obj"/>, set <paramref name="pointerValue"/> to be 
        /// equal to <paramref name="loc"/>.<br/>
        /// To remove pointers, set <paramref name="pointerValue"/> equal to <see cref="NullPointer"/></param>
        /// <returns>
        /// List of GridPoints at which tile's <c>passability</c> was changed.
        /// </returns>
        protected virtual List<GridPoint> SetPointers(T obj, GridPoint loc, GridPoint pointerValue)
        {
            List<GridPoint> _ = new List<GridPoint>();

            List<GridPoint> span = obj.GetSpan();
            // Set pointers on all tiles in the span.
            foreach (GridPoint p in span)
            {
                pointers[loc.Row + p.Row, loc.Column + p.Column] = pointerValue;
            }

            // Set tiles as impassible for all hitboxes
            List<GridPoint> hitboxes = obj.Hitboxes;
            foreach (GridPoint p in hitboxes)
            {
                _.Add(new GridPoint(loc.Row + p.Row, loc.Column + p.Column));
            }

            return _;
        }


        /// <summary>
        /// Draws the Layer.
        /// </summary>
        /// <param name="spriteBatch"><see cref="SpriteBatch"/> used for drawing.</param>
        /// <param name="startDraw">Upper left corner of subsection to draw.</param>
        /// <param name="endDraw">Bottom right corner of subsection to draw.</param>
        /// <param name="tileSet"><see cref="Texture2D"/> from which to pull sprites.</param>
        public void Draw(SpriteBatch spriteBatch, GridPoint startDraw, GridPoint endDraw, HashSet<IAnimate> animates = null)
        {
            if (startDraw.Equals(null))
                startDraw = new GridPoint(0, 0);

            for (int row = startDraw.Row; row < endDraw.Row; row++)
            {
                for (int col = startDraw.Column; col < endDraw.Column; col++)
                {
                    if (Values[row][col].IsNull)
                        continue;

                    // Remember that to switch to screen cordinates need to invert row and col order
                    // and to shift the row up by the height of the object - 1 to keep the
                    // position coordinate relatively constant in the draw methods.
                    Point position = new Point(col,
                        row - (Values[row][col].Dimensions.Height - 1)) * new Point(TileWidth, TileHeight);
                    MapDrawContext context = new MapDrawContext(spriteBatch,
                        position, position.ToVector2(), Color.White, 1, new Size(TileWidth, TileHeight));
                    Values[row][col].Draw(context);
                }
            }

            if (animates == null)
                return;

            foreach (IAnimate anim in animates)
            {
                MapDrawContext context = new MapDrawContext(spriteBatch, anim.Position.ToPoint(),
                     anim.Position, Color.White, 1, new Size(TileWidth, TileHeight));
                anim.Draw(context);
            }
        }


        /// <summary>
        /// Gets the <typeparamref name="T"/> at the <see cref="GridPoint"/> position.
        /// </summary>
        /// <param name="position"></param>
        /// <returns><typeparamref name="T"/> at <paramref name="position"/>.</returns>
        public T ObjectAt(GridPoint position)
        {
            return ObjectAt(position.Row, position.Column);
        }


        /// <summary>
        /// Gets the <typeparamref name="T"/> at (<paramref name="row"/>, <paramref name="col"/>).
        /// </summary>
        /// <param name="row">Row of the position.</param>
        /// <param name="col">Column of the position.</param>
        /// <returns><typeparamref name="T"/> at position (<paramref name="row"/>, <paramref name="col"/>).</returns>
        public T ObjectAt(int row, int col)
        {
            if (row > Rows || col > Columns)
                return Null;

            if (!Values[row][col].IsNull)
            {
                return Values[row][col];
            }
            else if (pointers[row, col] != NullPointer)
            {
                return Values[pointers[row, col].Row][pointers[row, col].Column];
            }
            return Null;
        }


        /// <summary>
        /// Returns whether a <typeparamref name="T"/> is at a position.
        /// </summary>
        /// <param name="position">Position to check whether a <typeparamref name="T"/> is.</param>
        /// <returns><see langword="true"/> if there is an <typeparamref name="T"/> at (<paramref name="row"/>, <paramref name="col"/>),
        /// <see langword="false"/> if not.</returns>
        public bool IsObjectAt(int row, int col)
        {
            if (col < Columns && row < Rows)
            {
                if (!Values[row][col].IsNull)
                {
                    return true;
                }
                else if (pointers[row, col] != NullPointer)
                {
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// Returns whether a <typeparamref name="T"/> exists at a <see cref="GridPoint"/> position.
        /// </summary>
        /// <param name="position">Position to check whether an <typeparamref name="T"/> is at.</param>
        /// <returns><see langword="true"/> if there is an <typeparamref name="T"/> at <paramref name="position"/>,
        /// <see langword="false"/> if not.</returns>
        public bool IsObjectAt(GridPoint position)
        {
            return IsObjectAt(position.Row, position.Column);
        }


        /// <summary>
        /// Determines whether a <typeparamref name="U"/> is of a given type.
        /// </summary>
        /// <typeparam name="U">Type of object to check for.</typeparam>
        /// <param name="position">Position of the <typeparamref name="U"/>.</param>
        /// <returns><see langword="true"/> if the <typeparamref name="T"/> at position <paramref name="position"/> is
        /// a <typeparamref name="U"/>, <see langword="false"/> if not.</returns>
        public bool ObjectIs<U>(GridPoint position)
        {
            if (ObjectAt(position) is U)
            {
                return true;
            }
            return false;
        }


        /// <inheritdoc path="EntityIs{U}(GridPoint)"/>
        /// <returns><see langword="true"/> if the <typeparamref name="T"/> at position 
        /// (<paramref name="row"/>, <paramref name="col"/>) is a <typeparamref name="U"/>, 
        /// <see langword="false"/> if not.</returns>
        public bool ObjectIs<U>(int row, int col)
        {
            if (ObjectAt(row, col) is T)
            {
                return true;
            }
            return false;
        }


        /// <summary>
        /// Sets an <typeparamref name="T"/> in this/>.
        /// </summary>
        /// <param name="obj"><typeparamref name="T"/> to set.</param>
        /// <param name="position">Position to set the <typeparamref name="T"/> at.</param>
        /// <returns>A list of points where the <typeparamref name="T"/> was set.</returns>
        public List<GridPoint> Set(T obj, GridPoint position)
        {
            List<GridPoint> _ = new List<GridPoint>();

            if (obj.IsNull)
                return _;

            Values[position.Row][position.Column] = obj;
            _ = SetPointers(Values[position.Row][position.Column], position, position);

            return _;
        }


        /// <summary>
        /// Removes the <typeparamref name="T"/> at the grid position.
        /// </summary>
        /// <param name="position">Position at which to remove the <typeparamref name="T"/>.</param>
        /// <returns>A list of gridpoints where the <typeparamref name="T"/> was removed.
        /// If no object was removed, returns an empty list.</returns>
        public List<GridPoint> Remove(GridPoint position)
        {
            List<GridPoint> _ = new List<GridPoint>();
            T entity = ObjectAt(position);

            if (entity.IsNull)
                return _;

            GridPoint pointer = pointers[position.Row, position.Column];
            Values[pointer.Row][pointer.Column] = Null;
            _ = SetPointers(entity, pointer, NullPointer);

            return _;
        }


        /// <summary>
        /// Directly sets an <typeparamref name="T"/> to this without setting pointers.
        /// </summary>
        /// <param name="obj"><typeparamref name="T"/> to set to the grid.</param>
        /// <param name="row">Row of the position to set the entity at.</param>
        /// <param name="col">Column of the position to set the entity at.</param>
        public void DirectSet(T obj, int row, int col)
        {
            Values[row][col] = obj;
        }


        /// <summary>
        /// Returns whether the GridPoint 'p' is within the GridPoint bounds of the layer.
        /// </summary>
        /// <param name="p">Point to check whether is in bounds.</param>
        /// <returns></returns>
        public virtual bool IsInBounds(GridPoint p)
        {
            return (p.Column >= 0 && p.Column < Columns && p.Row >= 0 && p.Row < Rows);
        }
    }
}
