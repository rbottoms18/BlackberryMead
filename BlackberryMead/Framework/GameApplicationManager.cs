using BlackberryMead.Utility;
using Microsoft.Xna.Framework;
using System;

namespace BlackberryMead.Framework
{
    /// <summary>
    /// Implementation of <see cref="IGameApplicationService"/>. <br/>
    /// 
    /// </summary>
    public class GameApplicationManager : IGameApplicationService
    {
        /// <summary>
        /// Game this manages.
        /// </summary>
        private Microsoft.Xna.Framework.Game game;

        /// <summary>
        /// GraphicsDeviceManager of the game.
        /// </summary>
        private GraphicsDeviceManager graphicsDeviceManager;

        public GameApplicationManager(Game game, GraphicsDeviceManager graphicsDeviceManager)
        {
            this.game = game;
            this.graphicsDeviceManager = graphicsDeviceManager;
        }

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
    }
}
