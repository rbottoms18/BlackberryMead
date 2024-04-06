using BlackberryMead.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace BlackberryMead.Input.Typography
{
    /// <summary>
    /// An alpha-numeric character used as part of a Font.
    /// </summary>
    public class Char
    {
        /// <summary>
        /// The alpha-numeric character represented by this.
        /// </summary>
        public char Character { get; init; }

        /// <summary>
        /// Size of the Character.
        /// </summary>
        public Size Size { get; set; }

        /// <summary>
        /// The opacity of this Char when drawn to the screen.
        /// </summary>
        public float Opacity { get; set; } = 1f;

        /// <summary>
        /// Offset of the Char from text string allignment.
        /// </summary>
        public Point Offset { get; set; }

        /// <summary>
        /// The Color of the body of this Char when drawn to the screen.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// The color of the border of this Char when drawn to the screen.
        /// </summary>
        public Color BorderColor { get; set; }

        /// <summary>
        /// List of <see cref="CharCopy"/>s of the main Char sprite to be drawn 
        /// before <see cref="Draw(SpriteBatch, Point)"/>.
        /// </summary>
        public List<CharCopy> PreDraws { get; set; }

        /// <summary>
        /// List of <see cref="CharCopy"/>s of the main Char sprite to be drawn after 
        /// <see cref="Draw(SpriteBatch, Point)"/>.
        /// </summary>
        public List<CharCopy> PostDraws { get; set; }

        /// <summary>
        /// Texture of this.
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        /// Source rectangle of this from the parent font.
        /// </summary>
        public Rectangle SourceRect { get; set; }

        /// <summary>
        /// Source rectangle of the border rectangle from the parent font.
        /// </summary>
        public Rectangle BorderRect { get; set; }

        /// <summary>
        /// Effects that are reapplied to this each update tick.
        /// </summary>
        public List<CharEffect> Effects { get; set; } = new List<CharEffect>();

        /// <summary>
        /// Instructions to draw a copy of the main Char sprite.
        /// </summary>
        public struct CharCopy
        {
            Color Color;
            float Opacity;
            int HOffset;
            int VOffset;

            public CharCopy(Color color, float opacity, int hOffset, int vOffset)
            {
                Color = color;
                Opacity = opacity;
                HOffset = hOffset;
                VOffset = vOffset;
            }
        }


        /// <summary>
        /// Create a new Character
        /// </summary>
        /// <param name="character">char that this object represents.</param>
        /// <param name="source">Source rectangle of this in the parent font sprite.</param>
        /// <param name="size">Size of the rectangle that circumscribes this.</param>
        public Char(char character, CharInfoContainer source, Texture2D texture, Size size)
        {
            Character = character;
            Size = size;
            Texture = texture;
            Color = Color.White;
            Opacity = 1f;

            // Process info container
            SourceRect = source.CharacterRect;
            BorderRect = source.BorderRect;
            Offset = source.Offset;
        }


        /// <summary>
        /// Updates the Char.
        /// </summary>
        public void Update()
        {
            foreach (CharEffect effect in Effects)
            {
                effect.Apply(this);
            }
        }


        /// <summary>
        /// Draws the Char.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="position">Position to draw this at.</param>
        public void Draw(SpriteBatch spriteBatch, Point position)
        {
            Rectangle rect = new Rectangle(position + Offset, Size);
            spriteBatch.Draw(Texture, rect, SourceRect, Color * Opacity);
            spriteBatch.Draw(Texture, rect, BorderRect, BorderColor * Opacity);
            //Shapes.DrawBorder(rect, Color.Red, 1, spriteBatch);
        }
    }
}
