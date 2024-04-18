using BlackberryMead.Framework;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BlackberryMead.Input.Typography
{
    /// <summary>
    /// A collection of sprites that represent alpha-numeric characters.
    /// </summary>
    public class Font
    {
        /// <summary>
        /// Dictionary of <see cref="Font"/> by name.
        /// </summary>
        public static Dictionary<string, Font> FontDict = new();

        /// <summary>
        /// Texture from which the <see cref="Font"/> textures are pulled from.
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        /// Path of the texture for the <see cref="Font"/>.
        /// </summary>
        public string TexturePath { get; set; }

        /// <summary>
        /// Rectangle that encapsulates <see cref="Font"/> textures inside the main texture.
        /// </summary>
        public Rectangle SourceRect { get; set; }

        /// <summary>
        /// Spacing between letters of the <see cref="Font"/>.
        /// </summary>
        public int LetterSpacing { get; set; }

        /// <summary>
        /// Size of the <see cref="Font"/> texture.
        /// </summary>
        [JsonInclude]
        public Size TextureSize { get; init; }

        /// <summary>
        /// Dictionary of <see cref="Char"/> info indexed by char in the <see cref="Font"/>.
        /// the texture.
        /// </summary>
        [JsonInclude]
        public Dictionary<char, CharInfoContainer> CharacterRectDict { get; init; } = new Dictionary<char, CharInfoContainer>();

        /// <summary>
        /// List of characters avaliable in the <see cref="Font"/>.
        /// </summary>
        [JsonInclude]
        public List<char> Characters { get; init; }

        /// <summary>
        /// Height in pixels from the top (Origin) of a character sprite rectangle to the baseline.
        /// </summary>
        /// <remarks>
        /// The baseline is the line on which font characters rest.
        /// </remarks>
        [JsonInclude]
        public int Baseline { get; init; }

        /// <summary>
        /// Size of the <see cref="Font"/>.
        /// </summary>
        public int FontSize { get; init; }

        /// <summary>
        /// Color of the border sprite.
        /// </summary>
        public Color BorderColor { get; set; }


        /// <summary>
        /// Gets the <see cref="Font"/> with the given name.
        /// </summary>
        /// <param name="name">Name of the Font.</param>
        /// <returns>The <see cref="Font"/> with the given name in <see cref="FontDict"/>.</returns>
        public static Font GetFontByName(string name)
        {
            try
            {
                return FontDict[name];
            }
            catch
            {
                throw new System.Exception("Font not found");
            }
        }


        /// <summary>
        /// Creates a new empty <see cref="Font"/>.
        /// </summary>
        /// <param name="TexturePath">Location of the <see cref="Font"/> texture.</param>
        /// <param name="SourceRect">Source rectangle of the <see cref="Font"/> inside the texture.</param>
        /// <param name="LetterSpacing">Spacing between letters.</param>
        /// <param name="FontSize">Size of the characters in the <see cref="Font"/>.</param>
        /// <param name="Characters">List of chars found in the <see cref="Font"/>.</param>
        /// <param name="CharacterRectDict">Dictionary of source rectangles of <see cref="Char"/> indexed
        /// by their corresponding alpha-numeric char.</param>
        [JsonConstructor]
        public Font(string TexturePath, Rectangle SourceRect, int LetterSpacing, int FontSize,
            List<char> Characters, Dictionary<char, CharInfoContainer> CharacterRectDict)
        {
            this.TexturePath = TexturePath;
            this.SourceRect = SourceRect;
            this.LetterSpacing = LetterSpacing;
            this.FontSize = FontSize;
            this.Characters = Characters;
            this.CharacterRectDict = CharacterRectDict;
        }


        /// <summary>
        /// Get the source rectangle of the given char.
        /// </summary>
        /// <param name="c">char to get the source rectangle of</param>
        /// <returns>Representative <see cref="Char"/> of char 'c' in the <see cref="Font"/>.</returns>
        public Char this[char c]
        {
            get
            {
                if (CharacterRectDict.TryGetValue(c, out CharInfoContainer charInfo))
                {
                    Size size = new Size(charInfo.CharacterRect.Size);
                    return new Char(c, charInfo, Texture, size * FontSize);
                }
                return new Char(c, new CharInfoContainer(Rectangle.Empty, Rectangle.Empty, Point.Zero),
                    Texture, Size.Zero);
            }
        }


        /// <summary>
        /// Returns a list of <see cref="Char"/> objects created from characters in a given string.
        /// </summary>
        /// <param name="s">String to get <see cref="Char"/> objects from.</param>
        public List<Char> this[string s]
        {
            get
            {
                List<Char> _ = new();
                foreach (char c in s)
                {
                    _.Add(this[c]);
                }
                return _;
            }
        }
    }
}
