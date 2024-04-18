using BlackberryMead.Utility.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Text.Json.Serialization;

namespace BlackberryMead.Input.UI
{
    /// <summary>
    /// <see cref="UIComponent"/> that draws a static image to the <see cref="UserInterface"/>.
    /// </summary>
    [OptInJsonSerialization]
    public class UIImage : UIComponent
    {
        /// <summary>
        /// Name of the SpriteSheet the image is from.
        /// </summary>
        [JsonInclude, JsonRequired]
        public string TextureName { get; init; }

        /// <summary>
        /// Source rectangle of the <see cref="UIImage"/>. 
        /// </summary>
        [JsonInclude]
        public Rectangle SourceRect { get; init; }

        /// <summary>
        /// Texture to draw from.
        /// </summary>
        private Texture2D texture;


        /// <summary>
        /// Create a new <see cref="UIImage"/>.
        /// </summary>
        /// <param name="TextureName">Name of the texture the <see cref="UIImage"/>
        /// pulls from.</param>
        /// <param name="SourceRect">Source rectangle of the image in the texture.</param>
        /// <param name="Layout">Layout settings.</param>
        public UIImage(string TextureName, Rectangle SourceRect, UILayout Layout) :
            base(Layout)
        {
            this.TextureName = TextureName;
            this.SourceRect = SourceRect;
        }


        public override void Update(InputState input) { }


        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.Draw(texture, Rect, SourceRect, Color.White);
        }


        public override void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>(@"SpriteSheets\" + TextureName);
            base.LoadContent(content);
        }
    }
}
