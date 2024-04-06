using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlackberryMead.Framework
{
    /// <summary>
    /// Interface that holds necessary values for drawing an <see cref="IDrawable{T}"/>.
    /// </summary>
    public interface IDrawContext
    {
        /// <summary>
        /// Spritebatch to draw with.
        /// </summary>
        SpriteBatch SpriteBatch { get; }

        /// <summary>
        /// Position to draw to.
        /// </summary>
        Point Position { get; }

        /// <summary>
        /// Position <see cref="Vector2"/> to draw to.
        /// </summary>
        Vector2 PositionV { get; }

        /// <summary>
        /// Size of the object.
        /// </summary>
        Size Size { get; }

        /// <summary>
        /// Color of the object.
        /// </summary>
        Color Color { get; }

        /// <summary>
        /// Opacity of the object.
        /// </summary>
        float Opacity { get; }

        /// <summary>
        /// Rectangle the object is drawn in.
        /// </summary>
        Rectangle Rect { get; }
    }
}
