using BlackberryMead.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlackberryMead.Input.UI
{
    /// <summary>
    /// NOP implimentation of the UIElement. 
    /// </summary>
    public class NullUIElement : UIComponent
    {
        /// <summary>
        /// Create a new <see cref="NullUIElement"/>.
        /// </summary>
        public NullUIElement() : base(new UILayout(Size.Zero, Alignment.Top, Alignment.Left, 1, 0, 0))
        { }


        public override void Draw(SpriteBatch spriteBatch) { }


        public override void Update(InputState input) { }
    }
}
