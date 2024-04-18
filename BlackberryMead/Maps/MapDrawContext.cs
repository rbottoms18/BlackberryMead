using BlackberryMead.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlackberryMead.Maps
{
    /// <summary>
    /// Implementation of <see cref="IDrawContext"/> that includes a Tile Dimension of a <see cref="Map2D{T}"/>.
    /// </summary>
    public class MapDrawContext : IDrawContext
    {
        public SpriteBatch SpriteBatch { get; set; }

        public Point Position { get; set; }

        public Vector2 PositionV { get; set; }

        public Color Color { get; set; }

        public float Opacity { get; set; }

        /// <summary>
        /// Dimensions of a tile in the <see cref="Map2D{T}"/>.
        /// </summary>
        public Size TileDim { get; set; }

        public Rectangle Rect { get; set; }

        public Size Size { get; set; }


        /// <summary>
        /// Create a new <see cref="MapDrawContext"/>.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch for drawing.</param>
        /// <param name="position">Draw position.</param>
        /// <param name="positionV">Draw position (vector).</param>
        /// <param name="color">Color to draw as.</param>
        /// <param name="opacity">Opacity to draw as.</param>
        /// <param name="tileDim">Dimensions of a tile.</param>
        public MapDrawContext(SpriteBatch spriteBatch, Point position, Vector2 positionV, Color color, float opacity,
            Size tileDim)
        {
            SpriteBatch = spriteBatch;
            Position = position;
            PositionV = positionV;
            Color = color;
            Opacity = opacity;
            TileDim = tileDim;
        }
    }
}
