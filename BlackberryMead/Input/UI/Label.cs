using BlackberryMead.Input.Typography;
using BlackberryMead.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text.Json.Serialization;
using Color = Microsoft.Xna.Framework.Color;

namespace BlackberryMead.Input.UI
{
    /// <summary>
    /// A UIElement that writes text to the screen.
    /// </summary>
    public class Label : UIComponent
    {
        /// <summary>
        /// Font used to draw text in this.
        /// </summary>
        [JsonInclude]
        public Font Font { get; protected set; }

        /// <summary>
        /// Name of the font used to draw the text of this.
        /// </summary>
        [JsonInclude, JsonRequired]
        public string FontName { get; protected set; }

        /// <summary>
        /// Color used to draw the font.
        /// </summary>
        [JsonInclude]
        public Color Color { get; protected set; }

        /// <summary>
        /// String text of the contents of this.
        /// </summary>
        [JsonInclude]
        public string Text { get; protected set; }

        /// <summary>
        /// Width of a line in the Text of this.
        /// </summary>
        [JsonInclude]
        public int LineWidth { get; protected set; }

        /// <summary>
        /// The text object that is drawn in this.
        /// </summary>
        private Text text { get; set; }


        /// <summary>
        /// Create a new TextBox.
        /// </summary>
        /// <inheritdoc/>
        /// <param name="Text">Text string to be displayed in this.</param>
        /// <param name="FontName">Name of the font used to write the text.</param>
        /// <param name="Color">Color of the text.</param>
        /// <param name="LineWidth">Maximum linewidth of the Text.</param>
        public Label(string Text, Font Font, Color Color, UILayout Layout, int LineWidth = 0) :
            base(Layout)
        {
            this.Text = Text;
            this.Font = Font;
            this.LineWidth = LineWidth;
            if (Color.A == 0)
                Color.A = 255;
            this.Color = Color;
            if (Font is null)
                this.Font = Font.GetFontByName("Iris3");

            text = new Text(Text, Font, this.Color, LineWidth);
            Dimensions = text.Size;
        }


        /// <summary>
        /// Create a new TextBox.
        /// </summary>
        /// <inheritdoc cref="Label.Label(string, Font, Color, int, Alignment, Alignment, int, int, int)"/>
        /// <param name="FontName">Name of the font to be used.</param>
        [JsonConstructor]
        public Label(string Text, string FontName, Color Color, UILayout Layout, int LineWidth = 0) :
            this(Text, Font.GetFontByName(FontName), Color, Layout, LineWidth) {}


        public override void Update(InputState input)
        {
            // Update text for effects
            text.Update();
        }


        public void SetContent(string content)
        {
            Text = content;
            text = new Text(Text, Font, this.Color, LineWidth);
        }


        public override void Realign(Rectangle boundingRect)
        {
            base.Realign(boundingRect);

            text = new Text(Text, Font, this.Color, LineWidth);
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            text.Draw(spriteBatch, Rect.Location);
        }
    }
}
