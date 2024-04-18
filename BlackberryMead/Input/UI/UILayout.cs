using BlackberryMead.Framework;
using System.Text.Json.Serialization;

namespace BlackberryMead.Input.UI
{
    /// <summary>
    /// A set of parameters that determine a layout of a <see cref="UIComponent"/>.
    /// </summary>
    public struct UILayout
    {
        /// <summary>
        /// Factor to which this is scaled.
        /// </summary>
        /// <remarks>
        /// Affects dimensions but does not affect horizontal and vertical offsets.
        /// </remarks>
        public int Scale { get; set; } = 1;

        /// <summary>
        /// Dimensions of the <see cref="UIComponent"/>.
        /// </summary>
        public Size Dimensions { get; set; }

        /// <summary>
        /// Vertical offset of the <see cref="UIComponent"/> from <see cref="UIComponent.Origin"/>.
        /// </summary>
        public int VerticalOffset { get; set; }

        /// <summary>
        /// Horizontal offset of the <see cref="UIComponent"/> from it <see cref="UIComponent.Origin"/>.
        /// </summary>
        public int HorizontalOffset { get; set; }

        /// <summary>
        /// Vertical allignment of the <see cref="UIComponent"/> against its
        /// <see cref="UIComponent.Origin"/>.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Alignment VerticalAlign { get; set; } = Alignment.Top;

        /// <summary>
        /// Horizontal allignment of the <see cref="UIComponent"/> against its 
        /// <see cref="UIComponent.Origin"/>.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Alignment HorizontalAlign { get; set; } = Alignment.Left;


        /// <summary>
        /// Create a new layout for a <see cref="UIComponent"/>.
        /// </summary>
        /// <param name="Dimensions">Dimensions of the bounding rectangle.</param>
        /// <param name="VerticalAlign">Vertical alignment inside the parent element.</param>
        /// <param name="HorizontalAlign">Horizontal alignment inside the parent element.</param>
        /// <param name="VerticalOffset">Vertical offset from the vertical alignment.</param>
        /// <param name="HorizontalOffset">Horizontal offset from the horizontal alignment.</param>
        public UILayout(Size Dimensions, Alignment VerticalAlign, Alignment HorizontalAlign,
        int Scale, int VerticalOffset, int HorizontalOffset)
        {
            this.Scale = Scale > 0 ? Scale : 1;
            this.Dimensions = Dimensions;
            this.VerticalAlign = VerticalAlign;
            this.HorizontalAlign = HorizontalAlign;
            this.VerticalOffset = VerticalOffset;
            this.HorizontalOffset = HorizontalOffset;
        }


        /// <summary>
        /// Create a new <see cref="UILayout"/> for a <see cref="UIComponent"/> 
        /// using default settings.
        /// </summary>
        public UILayout() { }
    }
}
