using Microsoft.Xna.Framework.Graphics;

namespace BlackberryMead.Input.UI
{
    /// <summary>
    /// Interface that allows members to render inside <see cref="RenderTarget2D"/>
    /// before being drawn.
    /// </summary>
    public interface IRenderable
    {
        /// <summary>
        /// Initializes the rendering objects of this.
        /// </summary>
        /// <param name="graphicsDevice">GraphicsDevice for rendering.</param>
        void Initialize(GraphicsDevice graphicsDevice);

        /// <summary>
        /// Renders the contents of this.
        /// </summary>
        /// <remarks>
        /// Call before drawing the object.
        /// </remarks>
        /// <param name="spriteBatch"></param>
        void Render(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice);
    }
}
