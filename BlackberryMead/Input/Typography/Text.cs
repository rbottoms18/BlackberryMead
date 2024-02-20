using BlackberryMead.Input.UI;
using BlackberryMead.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackberryMead.Input.Typography
{
    /// <summary>
    /// A string of Text that is drawn to the screen.
    /// Applies effects.
    /// </summary>
    public class Text
    {
        /// <summary>
        /// Font color of this.
        /// </summary>
        public Color Color { get; protected set; }

        /// <summary>
        /// String displayed by this object.
        /// </summary>
        public string Value { get; protected set; }

        /// <summary>
        /// Size of the bounding rectangle of this.
        /// </summary>
        public Size Size { get; protected set; }

        /// <summary>
        /// Maximum length of a line of text in pixels.
        /// </summary>
        protected int maxLineLength;

        /// <summary>
        /// Spacing between lines in pixels.
        /// </summary>
        protected int lineSpacing;

        /// <summary>
        /// Font used to drawn the string.
        /// </summary>
        protected readonly Font font;

        /// <summary>
        /// Lines of positions for <see cref="Char"/>s.
        /// </summary>
        protected List<List<Point>> positionLines = new List<List<Point>>();

        /// <summary>
        /// Lines of text <see cref="Char"/>s.
        /// </summary>
        protected List<List<Char>> charLines = new List<List<Char>>();

        /// <summary>
        /// Positions that each corresponding <see cref="Char"/> in <see cref="chars"/> is drawn to.
        /// </summary>
        protected Point[] charPositions;

        /// <summary>
        /// Characters that represent each char in the string <see cref="Value"/>.
        /// </summary>
        protected Char[] chars;

        /// <summary>
        /// Dictionary of text commands and their associated actions.
        /// </summary>
        protected Dictionary<string, Action<List<Char>, string[]>> textCommands = new Dictionary<string, Action<List<Char>, string[]>>()
        {
            { "Color", (chars, args) => {
                if (args.Length == 1)
                {
                    if (colorDict.TryGetValue(args[0], out Color color))
                    {
                        foreach (Char c in chars)
                            c.Color = color;
                    }
                }
                else if (args.Length == 3)
                {
                    foreach (Char c in chars )
                            c.Color = new Color(Convert.ToInt16(args[0]), Convert.ToInt16(args[1]),
                        Convert.ToInt16(args[2]));
                }
            }},
            { "Rumble", (chars, args) => {
                foreach (Char c in chars)
                    c.Effects.Add(new RumbleEffect(Convert.ToInt16(args[0])));
            }},
        };

        /// <summary>
        /// Reference dictionary of known colors.
        /// </summary>
        private static Dictionary<string, Color> colorDict = new Dictionary<string, Color>()
        {
            {"Red", Color.Red},
            {"Blue", Color.Blue},
            {"Green", Color.Green},
            {"Black", Color.Black},
            {"White", Color.White},
            {"Pink", Color.Pink},
            {"Purple", Color.Purple},
            {"Yellow", Color.Yellow},
            {"Orange", Color.MonoGameOrange},
            {"Azure", Color.Azure},
            {"BlueViolet", Color.BlueViolet}
        };

        /// <summary>
        /// Characters that count as punctuation and will not have <see cref="CharEffect"/>s apply to them.
        /// </summary>
        private static List<char> punctuationChars = new List<char>
            { ',', '.', ':', ';', '!', '?', '~', '(', ')' };

        /// <summary>
        /// Time variable for delay printing.
        /// </summary>
        private int t;


        /// <summary>
        /// Creates a new Text from a string.
        /// </summary>
        /// <param name="text">String value to be displayed.</param>
        /// <param name="font">Default font to be used unless otherwise specified.</param>
        /// <param name="horizontalAlign">Alignment of the straight-edge side of the text. If Left, the first letter of each
        /// line will align. If Right, the last letter of each line will align.</param>
        /// <param name="verticalAlign">Vertical alignment of the text. If Top, the text will print above the specificed location.
        /// If Bottom, it will print below the specified location.</param>
        /// <param name="lineSpacing">Spacing between consecutive lines. Default value 0</param>
        /// <param name="maxLineLength">Maximum length of a line in pixels. If <paramref name="maxLineLength"/> is default value 0,
        /// there will be no maximum line length.</param>
        public Text(string text, Font font, Color color, int maxLineLength = 0,
            Alignment horizontalAlign = Alignment.Left, Alignment verticalAlign = Alignment.Bottom,
            int lineSpacing = 0)
        {
            Value = text;
            this.font = font;
            this.maxLineLength = maxLineLength;
            this.lineSpacing = 0;
            Size = Size.Empty;

            Font currentFont;
            List<Point> linePositions = new List<Point>();
            List<Char> lineChars = new List<Char>();

            string[] words = text.Split(' ');
            // Add the space after each character except the last
            for (int w = 0; w < words.Length - 1; w++)
            {
                words[w] += " ";
            }

            for (int i = 0; i < words.Length; i++)
            {
                currentFont = this.font;
                string word = words[i];

                // If the word contains a linebreak character, remove it and force a linebreak
                bool forceLineBreak = false;
                if (word.Contains('>'))
                {
                    word = word.Remove(word.IndexOf('>'), 1);
                    forceLineBreak = true;
                }

                (List<Point> wordPositions, List<Char> wordChars) = ProcessWord(word, currentFont, color);
                var updatedLine = AppendWordToLine(word, wordPositions, wordChars, linePositions, lineChars,
                    currentFont, forceLineBreak);
                linePositions = updatedLine.linePositions;
                lineChars = updatedLine.lineChars;
            }

            // Add the last line
            charLines.Add(lineChars);
            positionLines.Add(linePositions);

            // Check if last line is greater than Size.Width and set Height
            int lineWidth = linePositions.Last().X + lineChars.Last().Size.Width;
            int textHeight = linePositions.Last().Y + lineChars.Last().Size.Height;
            if (lineWidth > Size.Width)
                Size = new Size(lineWidth, textHeight);
            else
                Size = new Size(Size.Width, textHeight);

            // Apply alignment
            Point[] translations = new Point[positionLines.Count];
            ArrayHelper.FillArray(translations, Point.Zero);

            if (horizontalAlign == Alignment.Right ||
                horizontalAlign == Alignment.Center)
            {
                for (int i = 0; i < positionLines.Count; i++)
                {
                    // Normalize the position of the last character in the line so the position of its top right is 0
                    translations[i].X = -1 * positionLines[i].Last().X - charLines[i].Last().Size.Width;
                    if (horizontalAlign == Alignment.Center)
                        translations[i].X = translations[i].X / 2;
                }
            }

            if (verticalAlign == Alignment.Top ||
                verticalAlign == Alignment.Center)
            {
                for (int i = 0; i < positionLines.Count; i++)
                {
                    // Shift characters up so the bottoms rest at the position
                    translations[i].Y = -1 * positionLines.Last().Last().Y - charLines.Last().Last().Size.Height;
                    if (verticalAlign == Alignment.Center)
                        translations[i].Y = translations[i].Y / 2;
                }
            }

            // Enumerate over lines
            for (int i = 0; i < positionLines.Count; i++)
            {
                // Enumerate over chars in line
                for (int j = 0; j < positionLines[i].Count; j++)
                {
                    positionLines[i][j] += translations[i];
                }
            }

            // Flatten lists into arrays
            charPositions = ArrayHelper.Flatten(positionLines).ToArray();
            chars = ArrayHelper.Flatten(charLines).ToArray();
        }


        /// <summary>
        /// Create a list of <see cref="Char"/> based on the characters in a string word.
        /// </summary>
        /// <param name="word"></param>
        /// <param name="font"></param>
        /// <returns>Tuple of the positions of the chars and the chars themselves.</returns>
        private (List<Point> positions, List<Char> characters) ProcessWord(string word, Font font, Color color)
        {
            string[] commands = word.Split('$');

            if (commands.Length != 0)
            {
                word = commands[commands.Length - 1];
            }

            // Preprocess the font command to get the word chars
            if (commands.Contains("Font"))
            {
                // Get font
                List<string> commandList = commands.ToList();
                commandList.Remove("Font");
                commands = commandList.ToArray();
            }

            List<Char> chars = font[word];
            List<Point> charPositions = GetWordCharPositions(word, chars, font);

            foreach (Char c in chars)
            {
                c.Color = color;
                c.BorderColor = font.BorderColor;
            }

            // Remove punctuation
            List<Char> commandApplicableChars = chars.Where(c => !punctuationChars.Contains(c.Character)).ToList();
            for (int j = 1; j < commands.Length - 1; j++)
            {
                string[] _ = commands[j].Split(':');
                string keyword = _[0];
                string[] args = _[1].Split(',');

                // Get effect from the dictionary by the first keyword
                if (textCommands.TryGetValue(_[0], out Action<List<Char>, string[]> method))
                {
                    method.Invoke(commandApplicableChars, args);
                }
            }

            return (charPositions, chars);
        }


        /// <summary>
        /// Gets a list of positions of <see cref="Char"/>s in a word.
        /// </summary>
        /// <param name="word">Word to process.</param>
        /// <param name="font">Font to get <see cref="Char"/>s from.</param>
        /// <returns>Ordered list of <see cref="Char"/> with characters corresponding to <paramref name="word"/>.</returns>
        private List<Point> GetWordCharPositions(string word, List<Char> chars, Font font)
        {
            List<Point> _ = new List<Point>();
            int x = 0;
            for (int k = 0; k < word.Length; k++)
            {
                Char c = chars[k];
                Point p = new(x, 0);
                p += c.Offset;

                // Increase the x-dim start for the next character
                x += c.Size.Width + ((font.LetterSpacing + c.Offset.X) * font.FontSize);

                _.Add(p);
            }
            return _;
        }


        /// <summary>
        /// Appends a "word" of Char rects to a line.
        /// </summary>
        /// <param name="wordPositions">Word to add to <paramref name="linePositions"/>.</param>
        /// <param name="linePositions">Line to add <paramref name="wordPositions"/> to.</param>
        /// <param name="font">Font of <paramref name="wordPositions"/>.</param>
        /// <returns>Updated list of <see cref="Char"/> in the current line.</returns>
        private (List<Point> linePositions, List<Char> lineChars) AppendWordToLine(string word, List<Point> wordPositions,
            List<Char> wordChars, List<Point> linePositions, List<Char> lineChars, Font font, bool forceLineBreak)
        {
            int wordLength = 0;
            if (wordPositions.Count > 0)
            {
                wordLength = wordPositions.Last().X;
                if (wordChars.Last().Character != ' ')
                {
                    wordLength += wordChars.Last().Size.Width;
                }
            }
            int lineLength = linePositions.Count > 0 ? linePositions.Last().X + lineChars.Last().Size.Width : 0;
            int lineY = linePositions.Count > 0 ? linePositions.Last().Y : 0;
            int lineHeight;
            if (lineChars.Count > 0)
            {
                lineHeight = lineChars.Last().Size.Height;
            }
            else
                lineHeight = wordChars[0].Size.Height;

            // If the word length + the length of the current line is greater than the maxLineWidth,
            // finalize the current line and drop this word down to a new line.
            // If the maxLineLength is 0, there will be no max and so continue to append to the current line.
            Point translation = Point.Zero;

            // If a line break is to be forced or if the maxLineLength is not empty
            // and adding the word to the line goes over the maxLineLength, apply a linebreak
            if (forceLineBreak || (maxLineLength != 0 && wordLength + lineLength > maxLineLength))
            {
                translation = new(0, lineY + lineHeight + (lineSpacing * font.FontSize));
                positionLines.Add(linePositions);
                charLines.Add(lineChars);
                linePositions = new List<Point>();
                lineChars = new List<Char>();
            }
            // Append to current line
            else
            {
                translation = new Point(lineLength, lineY);

                // If the line is not new/empty, add the letter spacing between words
                if (lineLength > 0)
                    translation += new Point((int)(font.LetterSpacing * font.FontSize), 0);

                if (wordLength + lineLength > Size.Width)
                    Size = new Size(wordLength + lineLength, Size.Height);
            }

            // Shift the wordPositions by the translation
            for (int r = 0; r < wordPositions.Count; r++)
                wordPositions[r] += translation;

            // Add the word positions/chars to the current line
            linePositions = linePositions.Concat(wordPositions).ToList();
            lineChars = lineChars.Concat(wordChars).ToList();

            return (linePositions, lineChars);
        }


        /// <summary>
        /// Updates the text.
        /// </summary>
        public void Update()
        {
            foreach (Char c in chars)
            {
                c.Update();
            }
        }


        /// <summary>
        /// Draw the entire Text to the screen.
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch used for drawing.</param>
        /// <param name="position">Position of the top left corner of the first char in the Text.</param>
        public void Draw(SpriteBatch spriteBatch, Point position)
        {
            for (int i = 0; i < chars.Length; i++)
            {
                chars[i].Draw(spriteBatch, position + charPositions[i]);
            }
        }


        /// <summary>
        /// Prints each character in succession over multiple calls.
        /// </summary>
        /// <param name="spriteBatch">Spritebatch for drawing.</param>
        /// <param name="position">Position to draw the text at.</param>
        /// <param name="speed">Number of update calls before the next character is drawn.</param>
        public void Print(SpriteBatch spriteBatch, Point position, int speed)
        {
            for (int i = 0; i < (t / speed); i++)
            {
                chars[i].Draw(spriteBatch, position + charPositions[i]);
            }
            if (t / speed < chars.Length)
                t++;
        }
    }
}
