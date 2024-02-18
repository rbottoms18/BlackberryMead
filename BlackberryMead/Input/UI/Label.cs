using BlackberryMead.Input.Typography;
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
        /// Create a new TextBox. The <paramref name="Dimensions"/> of this will be set to the dimensions of
        /// the <see cref="Text"/> it displays.
        /// </summary>
        /// <inheritdoc/>
        /// <param name="Text">Text string to be displayed in this.</param>
        /// <param name="FontSize">Size of the font used to write the text.</param>
        /// <param name="FontName">Name of the font used to write the text.</param>
        public Label(string Text, string FontName, Size Dimensions, int LineWidth, Color Color,
            Alignment VerticalAlign, Alignment HorizontalAlign, int Scale, int VerticalOffset, int HorizontalOffset) :
            base(Dimensions, VerticalAlign, HorizontalAlign, Scale, VerticalOffset, HorizontalOffset)
        {
            this.Text = Text;
            this.FontName = FontName;
            this.LineWidth = LineWidth;
            if (Color.A == 0)
                Color.A = 255;
            this.Color = Color;

            FontName ??= "Iris3";
            this.Font = Font.GetFontByName(FontName);

            if (this.Font == null)
            {
                this.Font = Font.GetFontByName(FontName);
            }

            text = new Text(Text, Font, this.Color, LineWidth);
            this.Dimensions = text.Size;
        }


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
