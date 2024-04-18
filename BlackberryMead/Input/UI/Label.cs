using BlackberryMead.Input.Typography;
using BlackberryMead.Utility.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text.Json.Serialization;
using Color = Microsoft.Xna.Framework.Color;

namespace BlackberryMead.Input.UI
{
    /// <summary>
    /// A <see cref="UIComponent"/> that writes a <see cref="Typography.Text"/> to the screen.
    /// </summary>
    [OptInJsonSerialization]
    public class Label : UIComponent
    {
        /// <summary>
        /// <see cref="Font"/> used to draw the <see cref="Typography.Text"/>.
        /// </summary>
        public Font Font { get; protected set; }

        /// <summary>
        /// Name of the <see cref="Typography.Font"/> used to draw the <see cref="Typography.Text"/>.
        /// </summary>
        [JsonInclude, JsonRequired, JsonOptIn]
        public string FontName { get; protected set; }

        /// <summary>
        /// Color of the <see cref="Typography.Font"/>.
        /// </summary>
        [JsonInclude]
        public Color Color { get; protected set; }

        /// <summary>
        /// String text of the body of the <see cref="Label"/>.
        /// </summary>
        [JsonInclude]
        public string Text { get; protected set; }

        /// <summary>
        /// Width of a line in the <see cref="Typography.Text"/> of this.
        /// </summary>
        [JsonInclude]
        public int LineWidth { get; protected set; }

        /// <summary>
        /// The <see cref="Typography.Text"/> object that is drawn in 
        /// the <see cref="Label"/>.
        /// </summary>
        private Text text { get; set; }


        /// <summary>
        /// Create a new <see cref="Label"/>.
        /// </summary>
        /// <inheritdoc/>
        /// <param name="Text">Text string to be displayed in this.</param>
        /// <param name="FontName">Name of the <see cref="Typography.Font"/> used to write the text.</param>
        /// <param name="Color">Color of the text.</param>
        /// <param name="LineWidth">Maximum linewidth of the <see cref="Typography.Text"/>.</param>
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
        /// Create a new <see cref="Label"/>.
        /// </summary>
        /// <inheritdoc cref="Label.Label(string, Font, Color, int, Alignment, Alignment, int, int, int)"/>
        /// <param name="FontName">Name of the <see cref="Typography.Font"/> to be used.</param>
        [JsonConstructor]
        public Label(string Text, string FontName, Color Color, UILayout Layout, int LineWidth = 0) :
            this(Text, Font.GetFontByName(FontName), Color, Layout, LineWidth)
        { }


        public override void Update(InputState input)
        {
            // Update text for effects
            text.Update();
        }


        /// <summary>
        /// Changes the content of the <see cref="Label"/> to a new string.
        /// </summary>
        /// <param name="content"></param>
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
