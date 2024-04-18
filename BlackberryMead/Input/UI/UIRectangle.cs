using BlackberryMead.Utility.Serialization;
using System.Text.Json.Serialization;

namespace BlackberryMead.Input.UI
{
    /// <summary>
    /// <see cref="UIComponent"/> that wraps a describes a simple rectangle.
    /// </summary>
    /// <remarks>
    /// Rectangle is encapsulated in <see cref="UIComponent.Rect"/>.
    /// </remarks>
    [OptInJsonSerialization]
    public class UIRectangle : UIComponent
    {
        /// <summary>
        /// Create a new <see cref="UIRectangle"/>.
        /// </summary>
        /// <param name="Layout">Layout settings.</param>
        [JsonConstructor]
        public UIRectangle(UILayout Layout) :
            base(Layout)
        { }


        public override void Update(InputState input) { }
    }
}
