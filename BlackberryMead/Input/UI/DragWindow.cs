using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using RonansGame.Utility;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Size = BlackberryMead.Utility.Size;

namespace BlackberryMead.Input.UI
{
    /// <summary>
    /// A UI Component window that can be dragged by the player to reposition it
    /// on the screen.
    /// </summary>
    public class DragWindow : Window
    {
        public override List<string> Actions => new List<string> { "Drag" };

        /// <summary>
        /// Rectangle regions that can be used to drag the window.
        /// When the mouse is outside these regions, the window will not drag.
        /// </summary>
        [JsonInclude]
        public List<DragRegion> DragRegions = new List<DragRegion>();

        /// <summary>
        /// Whether the window is being dragged by the player or not.
        /// </summary>
        private bool isDragging;

        /// <summary>
        /// Previous mouse position for computing delta mouse position
        /// </summary>
        private Point prevMousePos;


        /// <summary>
        /// UIComponent that marks a region of the DragWindow where the DragWindow can be selected to drag from.
        /// </summary>
        // I just want this so it will realgin when the drag window is moved. Simple implementation.
        public class DragRegion : UIComponent
        {
            public DragRegion(UILayout Layout) :
                base(Layout)
            {}

            public override void Update(InputState input) { }
        }


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
            DragRegions = new List<DragRegion>();
            foreach(UIComponent c in Components.Values) 
            {
                if (c is DragRegion)
                    DragRegions.Add((DragRegion)c);
            }
        }


        public override void Update(InputState input)
        {
            if (!input[GameActions.Drag])
                isDragging = false;
            else
            {
                // If mouse is down, drag
                foreach (DragRegion r in DragRegions)
                {
                    if (r.Contains(input.MousePosition))
                    {
                        isDragging = true;
                        break;
                    }
                }
            }

            if (isDragging)
            {
                // Move the window by the change in the mouse position
                Point deltaMouse = input.MousePosition - prevMousePos;
                Origin = Origin + deltaMouse;
                Rect = new Rectangle(Origin, Dimensions);

                Realign(Rect);
            }

            // Must be last thing in Update
            // Set prev mouse position to current
            prevMousePos = input.MousePosition;
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.FillRectangle(Rect, Color.Red, 0);
            foreach (DragRegion r in DragRegions)
            {
                spriteBatch.FillRectangle(r.Rect, Color.Blue, 0);
            }
            base.Draw(spriteBatch);
        }


        public override void Realign(Rectangle boundingRect)
        {
            base.Realign(boundingRect);
        }
    }
}
