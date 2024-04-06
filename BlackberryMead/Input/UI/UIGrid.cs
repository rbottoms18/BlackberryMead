using BlackberryMead.Framework;
using BlackberryMead.Utility.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Size = BlackberryMead.Framework.Size;

namespace BlackberryMead.Input.UI
{
    /// <summary>
    /// A grid in a <see cref="UserInterface"/> that displays an <see cref="IEnumerable{T}"/> collection of
    /// <see cref="IDrawable{T}"/> type <typeparamref name="T"/>. Contains methods for constructing the grid
    /// and for interacting with its objects using the mouse.
    /// </summary>
    [OptInJsonSerialization]
    abstract public class UIGrid<T> : UIComponent where T : INullImplementable<T>, IDrawable<IDrawContext>
    {
        /// <summary>
        /// Layout settings for the grid of <typeparamref name="T"/>.
        /// </summary>
        [JsonInclude, JsonOptIn]
        public UIGridLayout GridLayout { get; protected set; }

        /// <summary>
        /// Name of the action that triggers a click in this.
        /// </summary>
        /// <remarks>
        /// Default value "Select".
        /// </remarks>
        [JsonInclude]
        public string ClickAction { get; protected set; } = "Select";

        /// <summary>
        /// Position of the mouse.
        /// </summary>
        protected Point mousePosition;

        /// <summary>
        /// Index of the slot the mouse is hovering over.
        /// </summary>
        /// <remarks>
        /// If no slot is selected the value will be -1.
        /// </remarks>
        public int HoverIndex { get; set; }

        /// <summary>
        /// Index of the slot the mouse hovered over on the last update.
        /// </summary>
        /// <remarks>
        /// If no slot was selected the value will be -1.
        /// </remarks>
        protected int prevHoverIndex;

        /// <summary>
        /// Object of <typeparamref name="T"/> stored in the mouse.
        /// </summary>
        protected T mouseObject { get; set; } = T.Null;

        /// <summary>
        /// Rectangular slots in the grid.
        /// </summary>
        public Rectangle[] GridSlots { get; set; }

        /// <inheritdoc cref="UIGridLayout.GridSize"/>
        public Size GridSize { get; protected set; }

        /// <inheritdoc cref="UIGridLayout.NumSlots"/>
        public int NumSlots { get; protected set; }

        /// <inheritdoc cref="UIGridLayout.RowLength"/>
        public int RowLength { get; protected set; }

        /// <inheritdoc cref="UIGridLayout.VerticalSpacing"/>
        public int VerticalSpacing { get; protected set; }

        /// <inheritdoc cref="UIGridLayout.HorizontalSpacing"/>
        public int HorizontalSpacing { get; protected set; }

        /// <inheritdoc cref="UIGridLayout.GridVerticalOffset"/>
        public int GridVerticalOffset { get; protected set; }

        /// <inheritdoc cref="UIGridLayout.GridHorizontalOffset"/>
        public int GridHorizontalOffset { get; protected set; }


        protected UIGrid(UILayout Layout) : base(Layout) { }


        /// <summary>
        /// Creates a new <see cref="UIGrid{T}">.
        /// </summary>
        /// <param name="GridLayout">Layout settings for the grid.</param>
        /// <param name="Layout">Layout settings for the component.</param>
        [JsonConstructor]
        public UIGrid(UILayout Layout, UIGridLayout GridLayout) : base(Layout)
        {
            this.GridLayout = GridLayout;

            GridSize = GridLayout.GridSize * Layout.Scale;
            VerticalSpacing = GridLayout.VerticalSpacing * Scale;
            HorizontalSpacing = GridLayout.HorizontalSpacing * Scale;
            GridVerticalOffset = GridLayout.GridVerticalOffset * Scale;
            GridHorizontalOffset = GridLayout.GridHorizontalOffset * Scale;

            NumSlots = GridLayout.NumSlots;
            RowLength = GridLayout.RowLength;

            GridSlots = new Rectangle[NumSlots];
        }


        /// <summary>
        /// Creates and spaces the grid rectangles.
        /// </summary>
        protected virtual void InitializeGrid()
        {
            for (int i = 0; i < NumSlots; i++)
            {
                GridSlots[i] = new Rectangle(
                    Origin.X + GridHorizontalOffset + i % RowLength * GridSize.Width + i % RowLength * HorizontalSpacing,
                    Origin.Y + GridVerticalOffset + i / RowLength * GridSize.Height + i / RowLength * VerticalSpacing,
                    GridSize.Width,
                    GridSize.Height);
            }
        }


        /// <summary>
        /// Updates the <see cref="UIGrid{T}"/>. Computes the <see cref="HoverIndex"/> and updates the hovered slot.
        /// </summary>
        /// <param name="input"><see cref="InputState"/> of the current input.</param>
        public override void Update(InputState input)
        {
            mousePosition = input.MousePosition;

            // Set hover index to null pointer
            prevHoverIndex = HoverIndex;
            HoverIndex = -1;

            for (int i = 0; i < NumSlots; i++)
            {
                if (!GridSlots[i].Contains(input.MousePosition)) continue;

                HoverIndex = i;

                UpdateSelectedSlot(input, i);
            }
        }


        /// <summary>
        /// Updates the slot currently selected by the mouse.
        /// </summary>
        protected abstract void UpdateSelectedSlot(InputState input, int index);


        /// <summary>
        /// Draws the <see cref="UIGrid{T}"/>.
        /// Draws the grid of <typeparamref name="T"/> and the hover over a grid slot.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            DrawGrid(spriteBatch);
            base.Draw(spriteBatch);
            if (HoverIndex != -1)
            {
                DrawHover(spriteBatch);
            }
            DrawMouse(spriteBatch);
        }


        /// <summary>
        /// Draws the <see cref="UIGrid{T}"/>.
        /// </summary>
        /// <remarks>
        /// If <see cref="Values"/> has greater than <see cref="NumSlots"/> elements, only the first
        /// <see cref="NumSlots"/> elements will be drawn.
        /// </remarks>
        /// <param name="spriteBatch"><see cref="SpriteBatch"/> used for drawing.</param>
        protected virtual void DrawGrid(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < NumSlots; i++)
            {
                spriteBatch.FillRectangle(GridSlots[i], Color.White, 0);
            }
        }


        /// <summary>
        /// Draws the <typeparamref name="T"/> contained in the mouse at the mouse position.
        /// </summary>
        /// <param name="spriteBatch"><see cref="SpriteBatch"/> used for drawing.</param>
        protected virtual void DrawMouse(SpriteBatch spriteBatch)
        {
            if (mouseObject.IsNull) return;

            DrawContext context = new DrawContext(spriteBatch, new Rectangle(mousePosition - (GridSize / 2), GridSize), Color.White);
            mouseObject.Draw(context);
        }


        /// <summary>
        /// Draws a special hover effect over the grid slot that the mouse is hovering over.
        /// </summary>
        /// <param name="spriteBatch"></param>
        /// <param name="numSlots"></param>
        protected abstract void DrawHover(SpriteBatch spriteBatch);


        // Resets hover index to -1.
        public override void OnClose()
        {
            base.OnClose();
            HoverIndex = -1;
        }


        public override void Realign(Rectangle boundingRect)
        {
            base.Realign(boundingRect);
            InitializeGrid();
        }
    }
}
