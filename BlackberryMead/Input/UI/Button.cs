using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Text.Json.Serialization;
using Size = BlackberryMead.Utility.Size;

namespace BlackberryMead.Input.UI
{
    /// <summary>
    /// Clickable UIElement that raises an event upon click.
    /// </summary>
    public class Button : UIComponent
    {
        /// <summary>
        /// Event raised when the button is clicked
        /// </summary>
        public event EventHandler OnClick;

        /// <summary>
        /// Name of the action that must be triggered to .<br/>
        /// Default value "Select".
        /// </summary>
        [JsonInclude]
        public string ButtonDownAction { get; protected set; } = "Drag";

        /// <summary>
        /// Name of the action that must be triggered to .<br/>
        /// Default value "Select".
        /// </summary>
        [JsonInclude]
        public string ButtonUpAction { get; protected set; } = "Select";

        /// <summary>
        /// Source rectangle of the sprite of this.
        /// </summary>
        protected Rectangle sourceRect;

        /// <summary>
        /// Source rectangle of the sprite of this when it is being clicked
        /// </summary>
        protected Rectangle clickedSourceRect;

        /// <summary>
        /// Whether this is currently being clicked on.
        /// If true, the mouse is being held down on this.
        /// </summary>
        protected bool isBeingClicked = false;


        /// <summary> 
        /// Create a new Button
        /// </summary>
        /// <inheritdoc/>
        [JsonConstructor]
        public Button(Size Dimensions, Alignment VerticalAlign, Alignment HorizontalAlign,
            int Scale, int VerticalOffset, int HorizontalOffset) :
            base(Dimensions, VerticalAlign, HorizontalAlign, Scale, VerticalOffset, HorizontalOffset)
        {

        }


        public override void Update(InputState input)
        {
            if (Rect.Contains(input.MousePosition))
            {
                // If left mouse is down, set isBeingClicked to true to display the clicked sprite
                if (input[ButtonDownAction])
                    isBeingClicked = true;
                else if (input[ButtonUpAction])
                {
                    OnClickEvent(new EventArgs());
                    isBeingClicked = false;
                }
                else
                    isBeingClicked = false;
            }
        }


        /// <summary>
        /// Publishes the ClickEvent of this.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnClickEvent(EventArgs e)
        {
            // Microsoft wants me to make a temporary copy (.net guidelines)
            OnClick?.Invoke(this, e);
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            if (isBeingClicked)
                //spriteBatch.Draw(Spritesheet, Rect, clickedSourceRect, Color.White);
                spriteBatch.FillRectangle(Rect, Color.Red * Opacity, 0);
            else
                //spriteBatch.Draw(Spritesheet, Rect, sourceRect, Color.White);
                spriteBatch.FillRectangle(Rect, Color.White * Opacity, 0);

            base.Draw(spriteBatch);
        }
    }
}
