using BlackberryMead.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BlackberryMead.Input.Typography
{
    /// <summary>
    /// A collection of images that represent alpha-numeric characters.
    /// </summary>
    public class Font
    {
        /// <summary>
        /// Dictionary of fonts by font name
        /// </summary>
        public static Dictionary<string, Font> FontDict = new();

        /// <summary>
        /// Texture from which the font textures are pulled from
        /// </summary>
        public Texture2D Texture { get; set; }

        /// <summary>
        /// Path of the texture for the font
        /// </summary>
        public string TexturePath { get; set; }

        /// <summary>
        /// Rectangle that encapsulates font textures inside the main texture
        /// </summary>
        public Rectangle SourceRect { get; set; }

        /// <summary>
        /// Spacing between letters of the font
        /// </summary>
        public int LetterSpacing { get; set; }

        /// <summary>
        /// Size of the Font's texture.
        /// </summary>
        [JsonInclude]
        public Size TextureSize { get; init; }

        /// <summary>
        /// Dictionary of Character info indexed by char in the font.
        /// the texture.
        /// </summary>
        [JsonInclude]
        public Dictionary<char, CharInfoContainer> CharacterRectDict { get; init; } = new Dictionary<char, CharInfoContainer>();

        /// <summary>
        /// List of characters avaliable in this font.
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
        /// Size of the font.
        /// </summary>
        public int FontSize { get; init; }

        /// <summary>
        /// Color of the border sprite.
        /// </summary>
        public Color BorderColor { get; set; }


        /// <summary>
        /// Gets the font with the given name
        /// </summary>
        /// <param name="name">Name of the Font.</param>
        /// <returns></returns>
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
        /// Creates a new empty font
        /// </summary>
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
        /// Get the source rectangle of the given character.
        /// </summary>
        /// <param name="c">Character to get the source rectangle of</param>
        /// <returns>Source rectangle of char 'c' in Texture</returns>
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
