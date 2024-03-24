using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlackberryMead.Framework
{
    /// <summary>
    /// Interface that holds necessary values for drawing an <see cref="IDrawable"/>.
    /// </summary>
    public interface IDrawContext
    {
        /// <summary>
        /// SpriteBatch to draw with.
        /// </summary>
        SpriteBatch SpriteBatch { get; }

        /// <summary>
        /// Position to draw to.
        /// </summary>
        Point Position { get; }

        /// <summary>
        /// Position Vector to draw to.
        /// </summary>
        Vector2 PositionV { get; }

        /// <summary>
        /// Color of the object.
        /// </summary>
        Color Color { get; }

        /// <summary>
        /// Opacity of the object.
        /// </summary>
        float Opacity { get; }
    }
}
