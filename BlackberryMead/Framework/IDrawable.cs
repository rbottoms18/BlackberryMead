using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlackberryMead.Framework
{
    /// <summary>
    /// Marks an object as able to be drawn to the screen.
    /// </summary>
    public interface IDrawable<T> where T : IDrawContext
    {
        /// <summary>
        /// Draws the <see cref="IDrawable"/> to the screen.
        /// </summary>
        /// <param name="spriteBatch"><see cref="SpriteBatch"/> used for drawing.</param>
        /// <param name="position">Position to draw the <see cref="IDrawable"/> to.</param>
        /// <param name="color">Color override of the draw call.</param>
        /// <param name="opacity">Opacity of the <see cref="IDrawable"/>.</param>
        abstract void Draw(T context);
    }
}
