namespace BlackberryMead.Input.UI
{
    /// <summary>
    /// A set of spacing and alignment instructions for a grid.
    /// </summary>
    public struct UIGridLayout
    {
        /// <summary>
        /// Dimensions of the grid slots.
        /// </summary>
        public Utility.Size GridSize { get; set; }

        /// <summary>
        /// Number of slots.
        /// </summary>
        public int NumSlots { get; set; }

        /// <summary>
        /// Number of slots in a row.
        /// </summary>
        public int RowLength { get; set; } = 5;

        /// <summary>
        /// Vertical spacing between slots.
        /// </summary>
        public int VerticalSpacing { get; set; }

        /// <summary>
        /// Horizontal spacing between slots.
        /// </summary>
        public int HorizontalSpacing { get; set; }

        /// <summary>
        /// Vertical offset from the Origin to which the first slot is drawn to.
        /// </summary>
        public int GridVerticalOffset { get; set; }

        /// <summary>
        /// Horizontal offset from the Origin to which the first slot is drawn to.
        /// </summary>
        public int GridHorizontalOffset { get; set; }

        /// <summary>
        /// Create a new layout for a <see cref="ItemGrid"/>.
        /// </summary>
        /// <param name="GridSize">Dimensions of the grid slots.</param>
        /// <param name="NumSlots">Number of slots.</param>
        /// <param name="RowLength">Number of slots in a row.</param>
        /// <param name="VerticalSpacing">Vertical spacing between slots.</param>
        /// <param name="HorizontalSpacing">Horizontal spacing between slots.</param>
        /// <param name="GridVerticalOffset">Vertical offset from the Origin to which the first slot is drawn to.</param>
        /// <param name="GridHorizontalOffset">Horizontal offset from the Origin to which the first slot is drawn to.</param>
        public UIGridLayout(Utility.Size GridSize, int NumSlots, int RowLength, int VerticalSpacing,
        int HorizontalSpacing, int GridVerticalOffset, int GridHorizontalOffset)
        {
            this.GridSize = GridSize;
            this.NumSlots = NumSlots;
            this.RowLength = RowLength;
            this.VerticalSpacing = VerticalSpacing;
            this.HorizontalSpacing = HorizontalSpacing;
            this.GridVerticalOffset = GridVerticalOffset;
            this.GridHorizontalOffset = GridHorizontalOffset;
        }


        /// <summary>
        /// Returns a copy of the <see cref="UIGridLayout"/>.
        /// </summary>
        /// <returns></returns>
        public UIGridLayout ToNew()
        {
            return new UIGridLayout(GridSize, NumSlots, RowLength,
                VerticalSpacing, HorizontalSpacing, GridVerticalOffset, GridHorizontalOffset);
        }
    }
}
