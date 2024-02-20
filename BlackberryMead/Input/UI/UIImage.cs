using BlackberryMead.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Text.Json.Serialization;

namespace BlackberryMead.Input.UI
{
    /// <summary>
    /// <see cref="UIComponent"/> that draws a static image to the <see cref="UserInterface"/>.
    /// </summary>
    public class UIImage : UIComponent
    {
        /// <summary>
        /// Name of the SpriteSheet the image is from.
        /// </summary>
        [JsonInclude, JsonRequired]
        public string TextureName { get; init; }

        /// <summary>
        /// Source rectangle of the image. 
        /// </summary>
        [JsonInclude]
        public Rectangle SourceRect { get; init; }

        /// <summary>
        /// Texture to draw from.
        /// </summary>
        private Texture2D texture;


        public UIImage(string TextureName, Rectangle SourceRect, Size Dimensions,
            Alignment VerticalAlign, Alignment HorizontalAlign, int Scale, int VerticalOffset,
            int HorizontalOffset) :
            base(Dimensions, VerticalAlign, HorizontalAlign, Scale, VerticalOffset, HorizontalOffset)
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
