using Microsoft.Xna.Framework;

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
        /// Source rectangle for the border of the character.
        /// </summary>
        public Rectangle BorderRect;


        /// <summary>
        /// Create a new CharInfoContainer
        /// </summary>
        /// <param name="characterRect"></param>
        /// <param name="borderRect"></param>
        public CharInfoContainer(Rectangle characterRect, Rectangle borderRect)
        {
            CharacterRect = characterRect;
            BorderRect = borderRect;
        }
    }
}
