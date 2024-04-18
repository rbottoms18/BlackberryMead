using BlackberryMead.Utility.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Text.Json.Serialization;

namespace BlackberryMead.Input.UI
{
    /// <summary>
    /// Clickable <see cref="UIComponent"/> that raises an event upon click.
    /// </summary>
    [OptInJsonSerialization]
    public class Button : UIComponent
    {
        /// <summary>
        /// Event raised when the <see cref="Button"/> is clicked.
        /// </summary>
        public event EventHandler OnClick;

        /// <summary>
        /// Name of the action that must be triggered to register a
        /// down press on the button.
        /// </summary>
        /// <remarks>
        /// Default value "Select".
        /// </remarks>
        [JsonInclude]
        public string ButtonDownAction { get; protected set; } = "Drag";

        /// <summary>
        /// Name of the action that must be triggered to a release
        /// of the button.
        /// </summary>
        /// <remarks>Default value "Select".</remarks>
        [JsonInclude]
        public string ButtonUpAction { get; protected set; } = "Select";

        /// <summary>
        /// Source rectangle of the sprite of the <see cref="Button"/>.
        /// </summary>
        protected Rectangle sourceRect;

        /// <summary>
        /// Source rectangle of the sprite of the <see cref="Button"/> when it is being clicked.
        /// </summary>
        protected Rectangle clickedSourceRect;

        /// <summary>
        /// Whether the <see cref="Button"/> is currently being clicked on.
        /// </summary>
        protected bool isBeingClicked = false;


        /// <summary> 
        /// Create a new <see cref="Button"/>
        /// </summary>
        /// <inheritdoc/>
        [JsonConstructor]
        public Button(UILayout Layout) :
            base(Layout)
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
        /// Publishes the ClickEvent of the <see cref="Button"/>.
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
