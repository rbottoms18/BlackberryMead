using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace BlackberryMead.Framework
{
    /// <summary>
    /// Game class that adds functionality to <see cref="Microsoft.Xna.Framework.Game.Services"/>.
    /// </summary>
    public class Game : Microsoft.Xna.Framework.Game
    {
        /// <summary>
        /// <see cref="GraphicsDeviceManager"/> of the game.
        /// </summary>
        protected GraphicsDeviceManager graphics;

        /// <summary>
        /// Service Provider that provides Game Services to all classes.
        /// </summary>
        public static new GameServiceContainer Services { get; private set; }


        /// <summary>
        /// Create a new <see cref="Game"/>.
        /// </summary>
        /// <remarks>
        /// Initializes <see cref="ContentManager"/> and <see cref="IGameApplicationService"/> services.
        /// </remarks>
        public Game() : base()
        {
            graphics = new GraphicsDeviceManager(this);

            // Initialize services.
            Services = new GameServiceContainer();
            Services.AddService(typeof(ContentManager), this.Content);
            Services.AddService(typeof(IGameApplicationService), new GameApplicationManager(this, graphics));

        }


        /// <summary>
        /// Initialize the <see cref="Game"/>.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }


        /// <summary>
        /// Load the <see cref="Game"/> content.
        /// </summary>
        /// <remarks>
        /// Initializes the <see cref="GraphicsDevice"/> service.
        /// </remarks>
        protected override void LoadContent()
        {
            base.LoadContent();

            // Register Content service
            Services.AddService(typeof(GraphicsDevice), GraphicsDevice);
        }


        //protected override void Update(GameTime gameTime)
        //{
        //    base.Update(gameTime);
        //}


        //protected override void Draw(GameTime gameTime)
        //{
        //    base.Draw(gameTime);
        //}
    }
}
