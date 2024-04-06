using BlackberryMead.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlackberryMead.Framework
{
    /// <summary>
    /// Default implementation of a <see cref="IDrawContext"/>.
    /// </summary>
    internal class DrawContext : IDrawContext
    {
        public SpriteBatch SpriteBatch { get; set; }

        public Point Position {get; set;}

        public Rectangle Rect { get; set; }

        public Size Size { get; set; }

        public Vector2 PositionV {get; set;}

        public Color Color { get; set; }

        public float Opacity { get; set; }


        /// <summary>
        /// Creates a new <see cref="DrawContext"/>.
        /// </summary>
        /// <remarks>
        /// When using this constructor, <see cref="Size"/> and <see cref="Rect"/> are not set.
        /// </remarks>
        /// <param name="spriteBatch">SpriteBatch to draw with.</param>
        /// <param name="position">Position to draw to.</param>
        /// <param name="color">Color of the object.</param>
        /// <param name="opacity">Opacity of the object.</param>
        public DrawContext(SpriteBatch spriteBatch, Point position, Color color, float opacity = 1f) 
        {
            SpriteBatch = spriteBatch;
            Position = position;
            PositionV = position.ToVector2();
            Color = color;
            Opacity = opacity;
        }


        /// <summary>
        /// Creates a new <see cref="DrawContext"/>.
        /// </summary>
        /// <remarks>
        /// When using this constructor, <see cref="Size"/> and <see cref="Rect"/> are not set.
        /// </remarks>
        /// <param name="spriteBatch">SpriteBatch to draw with.</param>
        /// <param name="position">Position to draw to.</param>
        /// <param name="color">Color of the object.</param>
        /// <param name="opacity">Opacity of the object.</param>
        public DrawContext(SpriteBatch spriteBatch, Vector2 position, Color color, float opacity = 1f)
        {
            SpriteBatch = spriteBatch;
            Position = position.ToPoint();
            PositionV = position;
            Color = color;
            Opacity = opacity;
        }


        /// <summary>
        /// Create a new <see cref="DrawContext"/>.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch to draw with.</param>
        /// <param name="rect">Rectangle the object is drawn to.</param>
        /// <param name="color">Color of the object.</param>
        /// <param name="opacity">Opacity of the object.</param>
        public DrawContext(SpriteBatch spriteBatch, Rectangle rect, Color color, float opacity = 1f)
        {
            SpriteBatch = spriteBatch;
            Rect = rect;
            Position = Rect.Location;
            PositionV = Rect.Location.ToVector2();
            Size = new Size(Rect.Size);
            Color = color;
            Opacity = opacity;
        }
    }
}
