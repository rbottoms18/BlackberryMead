using BlackberryMead.Utility.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System.Collections.Generic;
using Size = BlackberryMead.Utility.Size;

namespace BlackberryMead.Input.UI
{
    /// <summary>
    /// A UI Component window that can be dragged by the player to reposition it
    /// on the screen.
    /// </summary>
    [OptInJsonSerialization]
    public class DragWindow : Window
    {
        public override List<string> Actions => new List<string> { "Drag" };

        /// <summary>
        /// Rectangle regions that can be used to drag the window.
        /// When the mouse is outside these regions, the window will not drag.
        /// </summary>
        private List<UIRectangle> dragRegions = new List<UIRectangle>();

        [JsonOptIn]
        public bool ShowDragRegions { get; set; } = false;

        /// <summary>
        /// Whether the window is being dragged by the player or not.
        /// </summary>
        public bool IsDragging { get; protected set; }

        /// <summary>
        /// Previous mouse position for computing delta mouse position
        /// </summary>
        private Point prevMousePos;


        /// <summary>
        /// Create a new DragWindow.
        /// </summary>
        /// <inheritdoc cref="Window.Window(Dictionary{string, UIComponent}, List{string}, Size, 
        /// Rectangle, Alignment, Alignment, int, int, int)"/>
        public DragWindow(Dictionary<string, UIComponent> Components,
            List<string> IncludeActions,
            Rectangle BackgroundSourceRect, UILayout Layout) :
            base(Components, IncludeActions, BackgroundSourceRect, Layout)
        {
            dragRegions = new List<UIRectangle>();
            foreach (UIComponent c in Components.Values)
            {
                if (c is UIRectangle)
                    dragRegions.Add((UIRectangle)c);
            }
        }


        public override void Update(InputState input)
        {
            if (!input["Drag"])
                IsDragging = false;
            else
            {
                // If mouse is down, drag
                foreach (UIRectangle r in dragRegions)
                {
                    if (r.Contains(input.MousePosition))
                    {
                        IsDragging = true;
                        break;
                    }
                }
            }

            if (IsDragging)
            {
                // Move the window by the change in the mouse position
                Point deltaMouse = input.MousePosition - prevMousePos;
                Origin = Origin + deltaMouse;
                Rect = new Rectangle(Origin, Dimensions);

                foreach (UIComponent c in Components.Values)
                    c.Realign(Rect);
            }

            // Update each compontent contained in the window
            base.Update(input);

            // Must be last thing in Update
            // Set prev mouse position to current
            prevMousePos = input.MousePosition;
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (ShowDragRegions)
            {
                foreach (UIRectangle r in dragRegions)
                {
                    spriteBatch.FillRectangle(r.Rect, Color.Blue, 0);
                }
            }
        }


        public override void Realign(Rectangle boundingRect)
        {
            base.Realign(boundingRect);
        }
    }
}
