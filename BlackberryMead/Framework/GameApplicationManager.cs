using Microsoft.Xna.Framework;
using System;

namespace BlackberryMead.Framework
{
    /// <summary>
    /// Implementation of <see cref="IGameApplicationService"/>.
    /// </summary>
    public class GameApplicationManager : IGameApplicationService
    {
        public Size DisplayDim
        {
            get => new Size(graphicsDeviceManager.PreferredBackBufferWidth, graphicsDeviceManager.PreferredBackBufferHeight);
            set
            {
                graphicsDeviceManager.PreferredBackBufferHeight = value.Height;
                graphicsDeviceManager.PreferredBackBufferWidth = value.Width;
                graphicsDeviceManager.ApplyChanges();
            }
        }

        public bool IsMouseVisible { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <summary>
        /// <see cref="Game"/> that is managed by the <see cref="GameApplicationManager"/>.
        /// </summary>
        private Microsoft.Xna.Framework.Game game;

        /// <summary>
        /// <see cref="GraphicsDeviceManager"/> of the <see cref="Game"/>.
        /// </summary>
        private GraphicsDeviceManager graphicsDeviceManager;


        /// <summary>
        /// Create a new <see cref="GraphicsDeviceManager"/>.
        /// </summary>
        /// <param name="game"><see cref="Game"/> to be managed.</param>
        /// <param name="graphicsDeviceManager"><see cref="GraphicsDeviceManager"/> of the <see cref="Game"/>.</param>
        public GameApplicationManager(Game game, GraphicsDeviceManager graphicsDeviceManager)
        {
            this.game = game;
            this.graphicsDeviceManager = graphicsDeviceManager;
        }
    }
}
