#nullable enable
using BlackberryMead.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace BlackberryMead.Framework
{
    /// <summary>
    /// A state of the game.
    /// </summary>
    public abstract class State
    {
        /// <summary>
        /// Event raised when the state calls the <see cref="StateMachine"/> to change the current state.
        /// </summary>
        public event EventHandler<string>? OnChangeState;

        /// <summary>
        /// Event called to request the game to quit.
        /// </summary>
        public event EventHandler? OnQuit;

        /// <summary>
        /// Graphics device used for drawing assets to the screen
        /// </summary>
        protected GraphicsDevice graphicsDevice;

        /// <summary>
        /// MouseState of the current update tick.
        /// </summary>
        protected MouseState currentMouseState;

        /// <summary>
        /// MouseState of the previous update tick.
        /// </summary>
        protected MouseState prevMouseState;

        /// <summary>
        /// KeyboardState of the current update tick.
        /// </summary>
        protected KeyboardState currentKeyboardState;

        /// <summary>
        /// KeyboardState of the previous update tick.
        /// </summary>
        protected KeyboardState prevKeyboardState;

        /// <summary>
        /// GamePadState of the current update tick.
        /// </summary>
        protected GamePadState currentGamePadState;

        /// <summary>
        /// GamePadState of the previous update tick.
        /// </summary>
        protected GamePadState prevGamePadState;

        /// <summary>
        /// Input of this current tick.
        /// </summary>
        protected InputState input;

        /// <summary>
        /// Actions of the <see cref="State"/>.
        /// </summary>
        protected Dictionary<string, Keybind> actions = new Dictionary<string, Keybind>();

        /// <summary>
        /// SpriteBatch used for drawing to the <see cref="State"/>.
        /// </summary>
        protected SpriteBatch spriteBatch;

        /// <summary>
        /// <see cref="InputManager"/> of the <see cref="State"/>.
        /// </summary>
        private InputManager inputManager;


        /// <summary>
        /// Create a new <see cref="State"/>.
        /// </summary>
        /// <param name="graphicsDevice">Graphics device to draw content</param>
        public State(GraphicsDevice graphicsDevice)
        {
            this.graphicsDevice = graphicsDevice;
            spriteBatch = new SpriteBatch(graphicsDevice);
            inputManager = new InputManager();
        }

        /// <summary>
        /// Update the <see cref="State"/> to preform per-tick logic.
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
            GetInput();
        }

        /// <summary>
        /// Draw the <see cref="State"/>.
        /// </summary>
        /// <param name="gameTime"></param>
        public abstract void Draw(GameTime gameTime);


        /// <summary>
        /// Computes this tick's <see cref="InputState"/>.
        /// </summary>
        protected virtual void GetInput()
        {
            input = inputManager.GetState(actions);
        }


        /// <summary>
        /// Sends a request to change the current <see cref="State"/>.
        /// </summary>
        protected void ChangeState(string newStateName)
        {
            if (OnChangeState != null)
                OnChangeState(this, newStateName);
        }


        /// <summary>
        /// Request to quit the game.
        /// </summary>
        protected void Quit()
        {
            if (OnQuit != null)
                OnQuit(this, null);
        }
    }
}
