using Microsoft.Xna.Framework.Graphics;

namespace BlackberryMead.Input.UI
{
    /// <summary>
    /// Provides methods to allow rendering inside a <see cref="RenderTarget2D"/> before being drawn.
    /// </summary>
    public interface IRenderable
    {
        /// <summary>
        /// Initializes the rendering objects of the <see cref="IRenderable"/>.
        /// </summary>
        /// <param name="graphicsDevice">GraphicsDevice for rendering.</param>
        void Initialize(GraphicsDevice graphicsDevice);

        /// <summary>
        /// Renders the contents of the <see cref="IRenderable"/>.
        /// </summary>
        /// <remarks>
        /// Call before drawing the <see cref="IRenderable"/>.
        /// </remarks>
        /// <param name="spriteBatch"></param>
        void Render(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice);
    }
}
