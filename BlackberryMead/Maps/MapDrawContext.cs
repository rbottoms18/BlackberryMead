using BlackberryMead.Framework;
using BlackberryMead.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlackberryMead.Maps
{
    /// <summary>
    /// Implimentation of <see cref="IDrawContext"/> that includes a TileDimension of a map.
    /// </summary>
    public class MapDrawContext : IDrawContext
    {
        public SpriteBatch SpriteBatch { get; set; }

        public Point Position { get; set; }

        public Vector2 PositionV { get; set; }

        public Color Color { get; set; }

        public float Opacity { get; set; }

        /// <summary>
        /// Dimensions of a tile in the map.
        /// </summary>
        public Size TileDim { get; set; }


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
