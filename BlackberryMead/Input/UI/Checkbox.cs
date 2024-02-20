using BlackberryMead.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Text.Json.Serialization;

namespace BlackberryMead.Input.UI
{
    /// <summary>
    /// A UIElement that can be enabled or disabled ("checked").
    /// </summary>
    public class Checkbox : UIComponent
    {
        /// <summary>
        /// Whether the checkbox is currently checked or not
        /// </summary>
        [JsonInclude]
        public bool IsChecked { get; protected set; }

        /// <summary>
        /// Source rectangle of the sprite when this is checked.
        /// </summary>
        [JsonInclude]
        public Rectangle CheckedSpriteSourceRect { get; protected set; }

        /// <summary>
        /// Source rectangle of the sprite when this is unchecked.
        /// </summary>
        [JsonInclude]
        public Rectangle UncheckedSpriteSourceRect { get; protected set; }

        /// <summary>
        /// Event published when the Checked status of the Checkbox changes.
        /// </summary>
        public event EventHandler<bool> OnModified;

        /// <summary>
        /// Name of the action that must be triggered to check this. <br/>
        /// Default value "Select".
        /// </summary>
        [JsonInclude]
        public string Action { get; protected set; } = "Select";


        /// <summary>
        /// Create a new Checkbox
        /// </summary>
        /// <inheritdoc/>
        /// <param name="IsChecked">Whether this is checked or not.</param>
        /// <param name="CheckedSpriteSourceRect">Source rectangle of the sprite when this is checked.</param>
        /// <param name="UncheckedSpriteSourceRect">Source rectangle of the sprite when this is unchecked.</param>
        [JsonConstructor]
        public Checkbox(bool IsChecked, Rectangle CheckedSpriteSourceRect,
            Rectangle UncheckedSpriteSourceRect, Size Dimensions, Alignment VerticalAlign, Alignment HorizontalAlign,
            int Scale, int VerticalOffset, int HorizontalOffset) :
            base(Dimensions, VerticalAlign, HorizontalAlign, Scale, VerticalOffset, HorizontalOffset)
        {
            this.IsChecked = IsChecked;
            this.CheckedSpriteSourceRect = CheckedSpriteSourceRect;
            this.UncheckedSpriteSourceRect = UncheckedSpriteSourceRect;
        }


        public override void Update(InputState input)
        {
            if (Rect.Contains(input.MousePosition) && input[Action])
            {
                IsChecked = !IsChecked;
                OnModified(this, IsChecked);
            }
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (IsChecked)
                spriteBatch.Draw(Spritesheet, Rect, CheckedSpriteSourceRect, Color.White * Opacity);
            else
                spriteBatch.Draw(Spritesheet, Rect, UncheckedSpriteSourceRect, Color.White * Opacity);
        }

    }
}
