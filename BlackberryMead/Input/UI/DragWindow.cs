using BlackberryMead.Utility.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System.Collections.Generic;
using Size = BlackberryMead.Framework.Size;

namespace BlackberryMead.Input.UI
{
    /// <summary>
    /// A <see cref="Window"/> that can be dragged to reposition it
    /// on the screen.
    /// </summary>
    [OptInJsonSerialization]
    public class DragWindow : Window, IDraggable
    {
        public override List<string> Actions => new List<string> { "Drag" };

        /// <summary>
        /// Rectangle regions that can be used to drag the <see cref="DragWindow"/>.
        /// </summary>
        /// <remarks>
        /// When the mouse is outside these regions, the window will not drag.
        /// </remarks>
        private List<UIRectangle> dragRegions = new List<UIRectangle>();

        /// <summary>
        /// Whether the <see cref="dragRegions"/> are drawn in <see cref="Draw(SpriteBatch)"/>.
        /// </summary>
        [JsonOptIn]
        public bool ShowDragRegions { get; set; } = false;

        /// <summary>
        /// Whether the <see cref="DragWindow"/> is being dragged or not.
        /// </summary>
        public bool IsDragging { get; set; }

        public string DragAction => "Drag";

        public List<UIRectangle> DragRegions => dragRegions;


        /// <summary>
        /// Create a new <see cref="DragWindow"/>.
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
            ((IDraggable)this).UpdateDrag(input);
            base.Update(input);
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (!ShowDragRegions) return;

            foreach (UIRectangle r in dragRegions)
            {
                spriteBatch.FillRectangle(r.Rect, Color.Blue, 0);
            }
        }


        public override void Realign(Rectangle boundingRect)
        {
            base.Realign(boundingRect);
        }


        public void OnDrag(InputState input)
        {
            Origin += input.MouseDelta;
            Rect = new Rectangle(Origin, Dimensions);

            foreach (UIComponent c in Components.Values)
                c.Realign(Rect);
        }
    }
}
