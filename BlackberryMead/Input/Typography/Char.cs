using BlackberryMead.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace BlackberryMead.Input.Typography
{
    /// <summary>
    /// An alpha-numeric character used as part of a <see cref="Font"/>.
    /// </summary>
    public class Char
    {
        /// <summary>
        /// The alpha-numeric character represented by the <see cref="Char"/>.
        /// </summary>
        public char Character { get; init; }

        /// <summary>
        /// Size of the <see cref="Char"/>.
        /// </summary>
        public Size Size { get; set; }

        /// <summary>
        /// The opacity of the <see cref="Char"/> when drawn to the screen.
        /// </summary>
        public float Opacity { get; set; } = 1f;

        /// <summary>
        /// Offset of the <see cref="Char"/> from text string allignment.
        /// </summary>
        public Point Offset { get; set; }

        /// <summary>
        /// The <see cref="Color"/> of the body of the <see cref="Char"/> when drawn to the screen.
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// The <see cref="Color"/> of the border of the <see cref="Char"/> when drawn to the screen.
        /// </summary>
        public Color BorderColor { get; set; }

        /// <summary>
        /// List of <see cref="CharCopy"/> of the main <see cref="Char"/> sprite to be drawn 
        /// before <see cref="Draw(SpriteBatch, Point)"/>.
        /// </summary>
        public List<CharCopy> PreDraws { get; set; }

        /// <summary>
        /// List of <see cref="CharCopy"/>s of the main <see cref="Char"/> sprite to be drawn after 
        /// <see cref="Draw(SpriteBatch, Point)"/>.
        /// </summary>
        public List<CharCopy> PostDraws { get; set; }

        /// <summary>
        /// Texture of the <see cref="Char"/>.
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        /// Source rectangle of the <see cref="Char"/> from the parent <see cref="Font"/>.
        /// </summary>
        public Rectangle SourceRect { get; set; }

        /// <summary>
        /// Source rectangle of the border rectangle from the parent <see cref="Font"/>.
        /// </summary>
        public Rectangle BorderRect { get; set; }

        /// <summary>
        /// Effects that are reapplied to the <see cref="Char"/> each update.
        /// </summary>
        public List<CharEffect> Effects { get; set; } = new List<CharEffect>();

        /// <summary>
        /// Instructions to draw a copy of the main <see cref="Char"/> sprite.
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
        /// Create a new <see cref="Char"/>.
        /// </summary>
        /// <param name="character">char that this object represents.</param>
        /// <param name="source">Source rectangle of this in the parent <see cref="Font"/> sprite.</param>
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
        /// Updates the <see cref="Char"/>.
        /// </summary>
        /// <remarks>
        /// Applies effects.
        /// </remarks>
        public void Update()
        {
            foreach (CharEffect effect in Effects)
            {
                effect.Apply(this);
            }
        }


        /// <summary>
        /// Draws the <see cref="Char"/>.
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
