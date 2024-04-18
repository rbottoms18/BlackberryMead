using Microsoft.Xna.Framework;
using System.Text.Json.Serialization;

namespace BlackberryMead.Input.Typography
{
    /// <summary>
    /// Container class that holds the source rectangles required for a <see cref="Char"/>. Used for wrapping
    /// of objects in Json.
    /// </summary>
    public class CharInfoContainer
    {
        /// <summary>
        /// Source rectangle for the main character portion of the sprite.
        /// </summary>
        public Rectangle CharacterRect;

        /// <summary>
        /// Source rectangle for the border of the <see cref="Char"/>.
        /// </summary>
        public Rectangle BorderRect;

        /// <summary>
        /// Offset of the <see cref="Char"/> from its perscribed position in the <see cref="Text"/>.
        /// </summary>
        public Point Offset;


        /// <summary>
        /// Create a new <see cref="CharInfoContainer"/>
        /// </summary>
        /// <param name="CharacterRect">Rectangle of the <see cref="Char"/>.</param>
        /// <param name="BorderRect">Border rectangle of the <see cref="Char"/>.</param>
        /// <param name="Offset">Offset of the <see cref="Char"/>.</param>
        [JsonConstructor]
        public CharInfoContainer(Rectangle CharacterRect, Rectangle BorderRect, Point Offset)
        {
            this.CharacterRect = CharacterRect;
            this.BorderRect = BorderRect;
            this.Offset = Offset;
        }
    }
}
