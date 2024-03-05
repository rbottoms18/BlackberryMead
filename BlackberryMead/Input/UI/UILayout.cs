using BlackberryMead.Utility;
using System.Text.Json.Serialization;

namespace BlackberryMead.Input.UI
{
    public class UILayout
    {
        /// <summary>
        /// Factor to which this is scaled.
        /// </summary>
        /// <remarks>
        /// Affects dimensions but does not affect horizontal and vertical offsets.
        /// </remarks>
        [JsonInclude]
        public int Scale { get; set; } = 1;

        /// <summary>
        /// Dimensions of the UIComponent.
        /// </summary>
        [JsonInclude]
        public Size Dimensions { get; set; }

        /// <summary>
        /// Vertical offset of the UIElement from its Origin.
        /// </summary>
        [JsonInclude]
        public int VerticalOffset { get; set; }

        /// <summary>
        /// Horizontal offset of the UIElement from its Origin.
        /// </summary>
        [JsonInclude]
        public int HorizontalOffset { get; set; }

        /// <summary>
        /// Vertical allignment of the UIElement against its Origin.
        /// </summary>
        [JsonInclude]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Alignment VerticalAlign { get; set; } = Alignment.Top;

        /// <summary>
        /// Horizontal allignment of the UIElement against its Origin.
        /// </summary>
        [JsonInclude]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Alignment HorizontalAlign { get; set; } = Alignment.Left;


        /// <summary>
        /// Create a new layout for a <see cref="UIComponent"/>.
        /// </summary>
        /// <param name="Dimensions">Dimensions of the UIComponent's bounding rectangle.</param>
        /// <param name="VerticalAlign">Vertical alignment of the component inside its parent element.</param>
        /// <param name="HorizontalAlign">Horizontal alignment of the component inside its parent element.</param>
        /// <param name="VerticalOffset">Vertical offset of the component from its vertical alignment.</param>
        /// <param name="HorizontalOffset">Horizontal offset of the component from its horizontal alignment.</param>
        public UILayout(Size Dimensions, Alignment VerticalAlign, Alignment HorizontalAlign,
        int Scale, int VerticalOffset, int HorizontalOffset)
        {
            this.Scale = Scale > 0 ? Scale : 1;
            if (Dimensions != Size.Zero)
                this.Dimensions = Dimensions * this.Scale;
            this.VerticalAlign = VerticalAlign;
            this.HorizontalAlign = HorizontalAlign;
            this.VerticalOffset = VerticalOffset * this.Scale;
            this.HorizontalOffset = HorizontalOffset * this.Scale;
        }


        /// <summary>
        /// Create a new layout for a <see cref="UIComponent"/> using default settings.
        /// </summary>
        public UILayout() { }
    }
}
