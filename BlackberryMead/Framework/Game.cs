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
        protected GraphicsDeviceManager graphics;

        /// <summary>
        /// Service provider that provides Game Services to all classes.
        /// </summary>
        public static new GameServiceContainer Services { get; private set; }

        public Game() : base()
        {
            graphics = new GraphicsDeviceManager(this);

            //// Initialize services
            Services = new GameServiceContainer();
            Services.AddService(typeof(ContentManager), this.Content);
            Services.AddService(typeof(IGameApplicationService), new GameApplicationManager(this, graphics));

        }


        protected override void Initialize()
        {
            base.Initialize();
        }


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
