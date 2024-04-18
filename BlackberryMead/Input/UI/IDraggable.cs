using System.Collections.Generic;

namespace BlackberryMead.Input.UI
{
    /// <summary>
    /// Provides a framework for allowing an object to be dragged across the screen. 
    /// </summary>
    /// <remarks>
    /// Intended for use with children classes of <see cref="UIComponent"/>.
    /// </remarks>
    public interface IDraggable
    {
        /// <summary>
        /// Whether the window is being dragged or not.
        /// </summary>
        bool IsDragging { get; set; }

        /// <summary>
        /// Name of the action representing dragging of the <see cref="IDraggable"/>.
        /// </summary>
        string DragAction { get; }

        /// <summary>
        /// Rectangle regions that can be used to drag the window.
        /// </summary>
        /// <remarks>
        /// When the mouse is outside these regions, the window will not drag.
        /// </remarks>
        List<UIRectangle> DragRegions { get; }


        /// <summary>
        /// Updates whether the <see cref="IDraggable"/> is being dragged. If it is,
        /// calls <see cref="OnDrag(InputState)"/>.
        /// </summary>
        /// <param name="input"><see cref="InputState"/> of the current update.</param>
        public virtual void UpdateDrag(InputState input)
        {
            if (!input[DragAction])
            {
                IsDragging = false;
                return;
            }

            foreach (UIRectangle r in DragRegions)
            {
                if (r.Contains(input.MousePosition))
                {
                    IsDragging = true;
                    break;
                }
            }

            if (IsDragging)
                OnDrag(input);
        }


        /// <summary>
        /// Performs logic each update when the <see cref="IDraggable"/> is being dragged.
        /// </summary>
        /// <param name="input"><see cref="InputState"/> of the current update.</param>
        abstract void OnDrag(InputState input);
    }
}
