using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace BlackberryMead.Utility
{
    /// <summary>
    /// Helper class containing various drawing functions.
    /// </summary>
    public static class Shapes
    {
        /// <summary>
        /// Draws a border around a rectangle with a given color and thickness.
        /// </summary>
        /// <param name="rectangle">Rectangle to draw a border around.</param>
        /// <param name="color">Color of the border.</param>
        /// <param name="borderWidth">Width of the border in pixels.</param>
        /// <param name="spriteBatch">SpriteBatch used for drawing.</param>
        public static void DrawBorder(Rectangle rectangle, Color color, int borderWidth, SpriteBatch spriteBatch)
        {
            // Top
            spriteBatch.FillRectangle(new Rectangle(rectangle.Left, rectangle.Top, rectangle.Right - rectangle.Left, borderWidth),
                color, 0);
            // Bottom
            spriteBatch.FillRectangle(new Rectangle(rectangle.Left, rectangle.Bottom - borderWidth, rectangle.Right - rectangle.Left, borderWidth),
                color, 0);
            // Left
            spriteBatch.FillRectangle(new Rectangle(rectangle.Left, rectangle.Top, borderWidth, rectangle.Bottom - rectangle.Top),
                color, 0);
            // Right
            spriteBatch.FillRectangle(new Rectangle(rectangle.Right - borderWidth, rectangle.Top, borderWidth, rectangle.Bottom - rectangle.Top),
                color, 0);
        }
    }
}
