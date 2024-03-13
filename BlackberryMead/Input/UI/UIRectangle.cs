using BlackberryMead.Utility.Serialization;
using System.Text.Json.Serialization;

namespace BlackberryMead.Input.UI
{
    /// <summary>
    /// UIComponent that wraps a rectangle.
    /// </summary>
    [OptInJsonSerialization]
    public class UIRectangle : UIComponent
    {

        [JsonConstructor]
        public UIRectangle(UILayout Layout) :
            base(Layout)
        { }


        public override void Update(InputState input) { }
    }
}
