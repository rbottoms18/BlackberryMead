using BlackberryMead.Collections;
using BlackberryMead.Framework;
using BlackberryMead.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json.Serialization;

namespace BlackberryMead.Maps
{
    public class Map2D<T> where T : IMapObject<T>
    {
        /// <summary>
        /// Number of Rows in this.
        /// </summary>
        [JsonInclude]
        public int Rows { get; init; }

        /// <summary>
        /// Number of Columns in this.
        /// </summary>
        [JsonInclude]
        public int Columns { get; init; }

        /// <summary>
        /// Size of this in tiles.
        /// </summary>
        [JsonIgnore]
        public GridPoint Size { get { return new GridPoint(Rows, Columns); } }

        /// <summary>
        /// Width of a tile.
        /// </summary>
        [JsonInclude]
        public int TileWidth { get; init; }

        /// <summary>
        /// Height of a tile.
        /// </summary>
        [JsonInclude]
        public int TileHeight { get; init; }

        /// <summary>
        /// Dimensions of a tile.
        /// </summary>
        [JsonIgnore]
        public Size TileDim { get { return new Size(TileWidth, TileHeight); } }

        /// <summary>
        /// <see cref="Layer2D{T}"/> that constitute this.
        /// </summary>
        [JsonInclude]
        public Layer2D<T>[] Layers { get; init; }

        /// <summary>
        /// 2d <see cref="bool"/> array containing passability information for each location on the map.
        /// </summary>
        /// <remarks>
        /// True if location is passable, false if impassible.
        /// </remarks>
        [JsonInclude]
        public bool[][] CollisionLayer { get; set; }

        /// <summary>
        /// Returns a <see cref="Vector2"/> of the coordinates of the top right corner of the map.
        /// </summary>
        public Vector2 TopRight
        {
            get { return new Vector2(Columns * TileWidth, 0); }
        }

        /// <summary>
        /// Returns a <see cref="Vector2"/> of the coordinates of the top left corner of the map.
        /// </summary>
        [JsonIgnore]
        public Vector2 TopLeft
        {
            get { return Vector2.Zero; }
        }

        /// <summary>
        /// Returns a <see cref="Vector2"/> of the coordinates of the bottom right corner of the map.
        /// </summary>
        /// <returns></returns>
        [JsonIgnore]
        public Vector2 BottomRight
        {
            get { return new Vector2(Columns * TileWidth, Rows * TileHeight); }
        }

        /// <summary>
        /// Returns a <see cref="Vector2"/> of the coordinates of the bottom left corner of the map.
        /// </summary>
        [JsonIgnore]
        public Vector2 BottomLeft
        {
            get { return new Vector2(0, Rows * TileHeight); }
        }

        /// <summary>
        /// TileSet texture from which <see cref="Tile"/> textures are sourced.
        /// </summary>
        [JsonIgnore]
        public Texture2D TileSet
        { get; set; }

        /// <summary>
        /// Whether gridlines are drawn for each tile in the map.
        /// </summary>
        [JsonIgnore]
        public bool DrawGridlines { get; set; }

        /// <summary>
        /// Scale multiplier to all spatial-related values. Affects tiles, tile positions.
        /// </summary>
        protected virtual int scale { get { return 1; } }

        /// <summary>
        /// Set of <see cref="IAnimate"/> objects in the map.
        /// </summary>
        protected HashSet<IAnimate> animateObjs = new();

        /// <summary>
        /// Size of a square collision region on the map.
        /// </summary>
        protected readonly int RegionSize = 5;

        /// <summary>
        /// <see cref="HashGrid{T}"/> holding the location of <see cref="IAnimate"/> objects on the map.
        /// </summary>
        protected HashGrid<IAnimate> animateGrid;

        /// <summary>
        /// Utility object for comparing values of <see cref="GridPoint"/>s.
        /// </summary>
        private static GridPointComparer gpComparer { get; set; } = new GridPointComparer();

        /// <summary>
        /// BlendState for drawing shadows
        /// </summary>
        private static BlendState blend = new BlendState
        {
            AlphaBlendFunction = BlendFunction.ReverseSubtract,
            AlphaSourceBlend = Blend.One,
            AlphaDestinationBlend = Blend.One,
        };


        /// <summary>
        /// Create a new empty Map.
        /// Base constructor does not initialize <see cref="Layers"/>"
        /// </summary>
        /// <inheritdoc cref="Map(MonoGame.Extended.Size, int, int, Layer2D[], bool[][])"/>
        public Map2D(int Rows, int Columns, int TileWidth, int TileHeight)
        {
            this.TileWidth = TileWidth * scale;
            this.TileHeight = TileHeight * scale;
            this.Rows = Rows;
            this.Columns = Columns;

            Layers = new Layer2D<T>[0];

            animateObjs = new HashSet<IAnimate>();
            animateGrid = new HashGrid<IAnimate>(Rows, Columns);
            CollisionLayer = ArrayHelper.NewFilledJaggedArray(Rows, Columns, true);
            animateGrid = new HashGrid<IAnimate>((Rows / RegionSize) + 1, (Columns / RegionSize) + 1);
        }


        /// <summary>
        /// Create a new Map.
        /// </summary>
        /// <param name="TileDim"></param>
        /// <param name="Rows"></param>
        /// <param name="Columns"></param>
        /// <param name="Layers"></param>
        /// <remarks>Use this constructor for deserialization.</remarks>
        [JsonConstructor]
        public Map2D(int Rows, int Columns, int TileWidth, int TileHeight, Layer2D<T>[] Layers, bool[][] CollisionLayer)
        {
            this.TileWidth = TileWidth * scale;
            this.TileHeight = TileHeight * scale;
            this.Rows = Rows;
            this.Columns = Columns;
            this.Layers = Layers;
            foreach (Layer2D<T> layer in Layers)
            {
                layer.TileWidth = this.TileWidth;
                layer.TileHeight = this.TileHeight;
            }
            animateGrid = new HashGrid<IAnimate>((Rows / RegionSize) + 1, (Columns / RegionSize) + 1);

            // Construct the CollisionLayer by 'pinging' each tile on the layers to see if there
            // is an entity hitbox there.
            this.CollisionLayer = CollisionLayer;
            foreach (Layer2D<T> layer in Layers)
            {
                for (int row = 0; row < layer.Values.Length; row++)
                {
                    for (int col = 0; col < layer.Values[0].Length; col++)
                    {
                        List<GridPoint> hitboxes = layer.Values[row][col].Hitboxes;
                        foreach (GridPoint hitbox in hitboxes)
                        {
                            CollisionLayer[row + hitbox.Row][col + hitbox.Column] = false;
                        }
                    }
                }
            }
        }


        /// <summary>
        /// Updates the map every in-game tick.
        /// </summary>
        public void Update(GameTime gameTime)
        {
            foreach (var layer in Layers)
            {
                for (int row = 0; row < layer.Rows; row++)
                {
                    for (int col = 0; col < layer.Columns; col++)
                    {
                        if (layer[row, col] is IUpdatable updatable)
                            updatable.Update(gameTime, this, new GridPoint(row, col));
                    }
                }
            }

            // Update IAnimates
            for (int i = 0; i < animateObjs.Count; i++)
                UpdateAnimate(gameTime, animateObjs.ElementAt(i));
        }


        /// <summary>
        /// Set an <see cref="T"/> on the map.
        /// </summary>
        /// <param name="layerIndex">Index of the layer to set a <see cref="T"/> to.</param>
        /// <param name="pos">Position to set the <see cref="T"/> to.</param>
        /// <param name="obj">Object of type <see cref="T"/> to add to the map.</param>
        public virtual void Set(int layerIndex, T obj, GridPoint pos)
        {
            foreach (GridPoint p in Layers[layerIndex].Set(obj, pos))
                CollisionLayer[p.Row][p.Column] = false;
        }


        /// <summary>
        /// Remove a <typeparamref name="T"/> from the map.
        /// </summary>
        /// <param name="layer">Layer from which to remove the <see cref="T"/> from.</param>
        /// <param name="pos">Position at which to remove the <see cref="T"/>.</param>
        public virtual void Remove(Layer2D<T> layer, GridPoint pos)
        {
            if (!layer.IsObjectAt(pos))
                return;

            // Set gridpoints that entity spans as passable.
            foreach (GridPoint p in layer.Remove(pos))
                CollisionLayer[p.Row][p.Column] = true;
        }


        /// <summary>
        /// Adds an IAnimate sprite to the map.
        /// </summary>
        /// <param name="animate"><see cref="IAnimate"/> to add to the map.</param>
        public virtual void AddAnimate(IAnimate animate)
        {
            animateObjs.Add(animate);
            animateGrid.Add(animate, new GridPoint((int)(animate.Position.Y) / (TileHeight * RegionSize),
                (int)(animate.Position.X) / (TileWidth * RegionSize)));
        }


        /// <summary>
        /// Removes an IAnimate object from the map
        /// </summary>
        /// <param name="animate"><see cref="IAnimate"/> to remove from the map.</param>
        public virtual void RemoveAnimate(IAnimate animate)
        {
            animateObjs.Remove(animate);
            animateGrid.Remove(animate, GetReigons(animate.Hitbox));
        }

        /// <summary>
        /// Returns the index of the tile with the given position vector.
        /// </summary>
        /// <param name="position">Position to get the tile at.</param>
        /// <returns></returns>
        public virtual GridPoint GetTileIndexAtPosition(Vector2 position)
        {
            int x = (int)Math.Floor(position.X / TileWidth);
            int y = (int)Math.Floor(position.Y / TileHeight);
            return new GridPoint(y, x);
        }


        /// <summary>
        /// Returns whether the GridPoint 'p' is within the GridPoint bounds of the map.
        /// </summary>
        /// <param name="p">Point to check whether is in the bounds of the map.</param>
        /// <returns></returns>
        public virtual bool IsInBounds(GridPoint p)
        {
            return (p.Column >= 0 && p.Column < Columns && p.Row >= 0 && p.Row < Rows);
        }


        /// <summary>
        /// Returns whether the tile at 'p' is passable or not.
        /// </summary>
        /// <param name="p">Point to check whether is passable or not.</param>
        /// <returns></returns>
        public virtual bool IsPassable(GridPoint p)
        {
            return CollisionLayer[p.Row][p.Column];
        }


        /// <summary>
        /// Gets an ordered list of objects that intersect with the given coordinate, from front to back.
        /// </summary>
        /// <param name="pos">Position to slice the map at.</param>
        public virtual List<T> Slice(GridPoint pos)
        {
            List<T> objects = new List<T>();
            // Objects to World layers
            for (int i = 0; i < Layers.Length; i++)
            {
                if (Layers[i].IsObjectAt(pos))
                    objects.Add(Layers[i].ObjectAt(pos));
            }
            return objects;
        }


        /// <summary>
        /// Gets an ordered list of objects of type <typeparamref name="U"/> that intersect with the given coordinate
        /// </summary>
        /// <typeparam name="U">Type of object to slice for.</typeparam>
        /// <param name="pos">Position to slice at.</param>
        /// <returns>All objects of type <typeparamref name="U"/> that intersect at <paramref name="pos"></paramref>.</returns>
        public virtual List<U> Slice<U>(GridPoint pos)
        {
            List<T> objects = Slice(pos);
            List<U> returns = new List<U>();
            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i] is U t)
                    returns.Add(t);
            }
            return returns;
        }


        /// <summary>
        /// Gets a list of all the <see cref="IAnimate"/> objects that intersect with the
        /// tile index <paramref name="pos"/>
        /// </summary>
        /// <param name="pos">Position to slice at.</param>
        /// <returns>Unordered list of all <see cref="IAnimate"/> objects at <paramref name="pos"/>.</returns>
        public virtual List<IAnimate> SliceAnimates(GridPoint pos)
        {
            List<IAnimate> animates = new();
            foreach (IAnimate animate in animateGrid[pos / RegionSize])
            {
                Rectangle tileRect = new Rectangle((pos * TileDim).ToPoint(), TileDim);
                if (tileRect.Intersects(animate.Hitbox))
                    animates.Add(animate);
            }
            return animates;
        }


        /// <summary>
        /// Updates the movement of an IAnimate object.
        /// Preforms checks for collisions with other IAnimate objects,
        /// and for movement in the vertical and horizontal directions.
        /// </summary>
        /// <param name="animateObj"><see cref="IAnimate"/> to update.</param>
        protected virtual void UpdateAnimate(GameTime gameTime, IAnimate animateObj)
        {
            animateObj.Update(gameTime, this, GetTileIndexAtPosition(animateObj.Position));

            if (animateObj.Velocity == Vector2.Zero)
                return; // Comment this line out to have collisions checked every tick regardless of vel

            Vector2 velocity = animateObj.Velocity;
            List<GridPoint> prevReigons = GetReigons(animateObj.Hitbox);

            ///
            /// RUN MOVEMENT
            /// Movement is handled in 3 steps: colliding with other animates,
            /// then global impassible tiles in the vertical then horizontal directions.
            /// 

            HashSet<IAnimate> collidingAnims = animateGrid.GetSetUnion(prevReigons);
            collidingAnims.Remove(animateObj);
            bool collided = false;
            foreach (IAnimate collidingAnim in collidingAnims)
            {
                if (velocity.X > 0 && animateObj.IsTouchingLeft(velocity, collidingAnim.Hitbox))
                {
                    collided = true;
                    velocity.X = collidingAnim.Hitbox.Left - animateObj.Hitbox.Right;
                }
                else if (animateObj.IsTouchingRight(velocity, collidingAnim.Hitbox))
                {
                    collided = true;
                    velocity.X = collidingAnim.Hitbox.Right - 1 - animateObj.Hitbox.Left;
                }
                if (velocity.Y > 0 && animateObj.IsTouchingTop(velocity, collidingAnim.Hitbox))
                {
                    collided = true;
                    velocity.Y = collidingAnim.Hitbox.Top - (animateObj.Hitbox.Bottom - 1);
                }
                else if (animateObj.IsTouchingBottom(velocity, collidingAnim.Hitbox))
                {
                    collided = true;
                    velocity.Y = (collidingAnim.Hitbox.Bottom - 1) - animateObj.Hitbox.Top;
                }

                if (collided)
                {
                    ///
                    /// Haven't implimented the collision methods yet
                    ///
                    //    animateObj.Collide(collidingAnim);
                }
            }

            //// Vertical movement
            int yLine = (velocity.Y > 0) ? animateObj.Hitbox.Bottom : animateObj.Hitbox.Top;
            if (((int)(animateObj.Hitbox.Bottom + velocity.Y) / TileHeight != (animateObj.Hitbox.Bottom - 1) / TileHeight) ||
                ((int)(animateObj.Hitbox.Top + velocity.Y) / TileHeight != animateObj.Hitbox.Top / TileHeight))
            {
                for (int col = (animateObj.Hitbox.Left) / TileWidth; col <= (animateObj.Hitbox.Right - 1) / TileWidth; col++)
                {
                    GridPoint p = new GridPoint((int)(yLine + animateObj.Velocity.Y) / TileHeight, col);

                    if (IsPassable(p))
                        continue;

                    velocity.Y = velocity.Y > 0 ? (TileHeight * p.Row) - animateObj.Hitbox.Bottom :
                        TileHeight * (p.Row + 1) - animateObj.Hitbox.Top;
                    break;
                }
            }
            animateObj.Velocity = new Vector2(0, velocity.Y);
            animateObj.Move();

            // Horizontal movement
            int xLine = (velocity.X > 0) ? animateObj.Hitbox.Right : animateObj.Hitbox.Left;
            if (((int)(animateObj.Hitbox.Right + velocity.X) / TileWidth != (animateObj.Hitbox.Right - 1) / TileWidth) ||
                ((int)(animateObj.Hitbox.Left + velocity.X) / TileWidth != animateObj.Hitbox.Left / TileWidth))
            {
                for (int row = animateObj.Hitbox.Top / TileHeight; row <= (animateObj.Hitbox.Bottom - 1) / TileHeight; row++)
                {
                    GridPoint p = new GridPoint(row, (int)(xLine + velocity.X) / TileWidth);

                    if (IsPassable(p))
                        continue;

                    velocity.X = velocity.X > 0 ? (TileWidth * p.Column) - animateObj.Hitbox.Right :
                        TileWidth * (p.Column + 1) - animateObj.Hitbox.Left;
                    break;
                }
            }
            animateObj.Velocity = new Vector2(velocity.X, 0);
            animateObj.Move();

            List<GridPoint> currentReigons = GetReigons(animateObj.Hitbox);
            UpdateCollisionReigon(animateObj, currentReigons, prevReigons);

            DoCollisions(animateObj);
        }


        /// <summary>
        /// Collides the IAnimate animateObj with all objects and tiles in the span of its hitbox
        /// </summary>
        /// <param name="animateObj"><see cref="IAnimate"/> to do collisions for.</param>
        protected virtual void DoCollisions(IAnimate animateObj)
        {
            HashSet<GridPoint> collisionCoords = new HashSet<GridPoint>();
            for (int row = (animateObj.Hitbox.Top - 1) / TileHeight; row <= (animateObj.Hitbox.Bottom) / TileHeight; row++)
            {
                for (int col = (animateObj.Hitbox.Left - 1) / TileWidth; col <= (animateObj.Hitbox.Right) / TileWidth; col++)
                {
                    collisionCoords.Add(new GridPoint(row, col));
                }
            }

            // Remove corners
            if ((animateObj.Hitbox.Top - 1) / TileHeight != (animateObj.Hitbox.Top) / TileHeight)
            {
                if ((animateObj.Hitbox.Left - 1) / TileWidth != (animateObj.Hitbox.Left) / TileWidth)
                    collisionCoords.Remove(new GridPoint((animateObj.Hitbox.Top - 1) / TileHeight,
                        (animateObj.Hitbox.Left - 1) / TileWidth));
                if ((animateObj.Hitbox.Right - 1) / TileWidth != (animateObj.Hitbox.Right) / TileWidth)
                    collisionCoords.Remove(new GridPoint((animateObj.Hitbox.Top - 1) / TileHeight,
                        (animateObj.Hitbox.Right) / TileWidth));
            }
            if ((animateObj.Hitbox.Bottom - 1) / TileHeight != (animateObj.Hitbox.Bottom) / TileHeight)
            {
                if ((animateObj.Hitbox.Left - 1) / TileWidth != (animateObj.Hitbox.Left) / TileWidth)
                    collisionCoords.Remove(new GridPoint((animateObj.Hitbox.Bottom) / TileHeight,
                        (animateObj.Hitbox.Left - 1) / TileWidth));
                if ((animateObj.Hitbox.Right - 1) / TileWidth != (animateObj.Hitbox.Right) / TileWidth)
                    collisionCoords.Remove(new GridPoint((animateObj.Hitbox.Bottom) / TileHeight,
                        (animateObj.Hitbox.Right) / TileWidth));
            }

            foreach (GridPoint p in collisionCoords)
            {
                // Only colliding with things in the objects layer?
                for (int i = 3; i >= 0; i--)
                {
                    if (!(Layers[i].ObjectAt(p) is ICollidable collisionObj)) continue;
                    collisionObj.Collide(animateObj);
                    Debug.Print("Collided with + " + collisionObj.ToString() + " @ (" + p.Row + "," + p.Column + ")");
                }
            }
        }


        /// <summary>
        /// Updates the collisions reigons of an <see cref="IAnimate"/> object based on its past and current location.
        /// </summary>
        /// <param name="animateObj"><see cref="IAnimate"/> to update collision reigons for.</param>
        /// <param name="prevReigons">Previous regions <paramref name="animateObj"/> was in.</param>
        protected virtual void UpdateCollisionReigon(IAnimate animateObj, List<GridPoint> currentReigons, List<GridPoint> prevReigons)
        {
            // Remove obj from old reigons
            foreach (GridPoint r in prevReigons.Except(currentReigons.Intersect(prevReigons, gpComparer), gpComparer))
                animateGrid.Remove(animateObj, r);
            // Add obj to new reigons
            foreach (GridPoint r in currentReigons.Except(currentReigons.Intersect(prevReigons, gpComparer), gpComparer))
            {
                animateGrid.Add(animateObj, r);
                Debug.Print("Added to " + r);
            }
        }


        /// <summary>
        /// Gets the Collision regions that contain the Rectangle 'r'
        /// </summary>
        /// <param name="r">Rectangle to get collision regions that intersect with.</param>
        /// <returns>List of collision region indeces in units <see cref="GridPoint"/> that 
        /// intersect with <paramref name="r"/>.</returns>
        protected virtual List<GridPoint> GetReigons(Rectangle r)
        {
            List<GridPoint> regions = new List<GridPoint>
            {
                new GridPoint(r.Top / (TileHeight * RegionSize), r.Left / (TileWidth * RegionSize)),
                new GridPoint(r.Bottom / (TileHeight * RegionSize), r.Left / (TileWidth * RegionSize)),
                new GridPoint(r.Top / (TileHeight * RegionSize), r.Right / (TileWidth * RegionSize)),
                new GridPoint(r.Bottom /(TileHeight * RegionSize), r.Right /(TileWidth * RegionSize))
            };

            // Get distinct entries
            return regions.Distinct(gpComparer).ToList();
        }
    }
}
