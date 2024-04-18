using BlackberryMead.Utility.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Text.Json.Serialization;

namespace BlackberryMead.Input.UI
{
    /// <summary>
    /// A <see cref="UIComponent"/> that can be enabled or disabled ("checked").
    /// </summary>
    [OptInJsonSerialization]
    public class Checkbox : UIComponent
    {
        /// <summary>
        /// Whether the <see cref="Checkbox"/> is currently checked or not.
        /// </summary>
        [JsonInclude]
        public bool IsChecked { get; protected set; }

        /// <summary>
        /// Source rectangle of the sprite when the <see cref="Checkbox"/> is checked.
        /// </summary>
        [JsonInclude]
        public Rectangle CheckedSpriteSourceRect { get; protected set; }

        /// <summary>
        /// Source rectangle of the sprite when the <see cref="Checkbox"/> is unchecked.
        /// </summary>
        [JsonInclude]
        public Rectangle UncheckedSpriteSourceRect { get; protected set; }

        /// <summary>
        /// Event published when the Checked status of the <see cref="Checkbox"/> changes.
        /// </summary>
        public event EventHandler<bool> OnModified;

        /// <summary>
        /// Name of the action that must be triggered to check the <see cref="Checkbox"/>.
        /// </summary>
        /// <remarks>
        /// Default value "Select".
        /// </remarks>
        [JsonInclude]
        public string Action { get; protected set; } = "Select";


        /// <summary>
        /// Create a new <see cref="Checkbox"/>.
        /// </summary>
        /// <inheritdoc/>
        /// <param name="IsChecked">Whether the <see cref="Checkbox"/> is checked or not.</param>
        /// <param name="CheckedSpriteSourceRect">Source rectangle of the sprite when 
        /// the <see cref="Checkbox"/> is checked.</param>
        /// <param name="UncheckedSpriteSourceRect">Source rectangle of the sprite when 
        /// the <see cref="Checkbox"/> is unchecked.</param>
        [JsonConstructor]
        public Checkbox(bool IsChecked, Rectangle CheckedSpriteSourceRect,
            Rectangle UncheckedSpriteSourceRect, UILayout Layout) :
            base(Layout)
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
