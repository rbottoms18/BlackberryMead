#nullable enable
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackberryMead.Framework
{
    /// <summary>
    /// A class that implements an event-based state machine to control state flow in a <see cref="Game"/>.
    /// </summary>
    public class StateMachine
    {
        /// <summary>
        /// Event that signals for the game application to quit.
        /// </summary>
        public event EventHandler? OnQuit;

        /// <summary>
        /// Current <see cref="State"/> of the game.
        /// </summary>
        private State currentState;

        /// <summary>
        /// Possible <see cref="State"/> objects of the machine.
        /// </summary>
        private Dictionary<string, State> states = new Dictionary<string, State>();


        /// <summary>
        /// Create a new <see cref="StateMachine"/> from an existing set of <see cref="State"/> objects.
        /// </summary>
        /// <param name="states">Dictionary of <see cref="State"/> objects to be added to the <see cref="StateMachine"/>.</param>
        /// <param name="initialCurrentState">Name of the initial <see cref="State"/> of the <see cref="StateMachine"/> 
        /// on initialization.</param>
        /// <exception cref="Exception">Throws an exception when <paramref name="states"/> is either null
        /// or empty. An existing initial state is required.</exception>
        public StateMachine(Dictionary<string, State> states, string initialCurrentState = "")
        {
            if (states.Count == 0 || states == null)
                throw new Exception("States cannot be null or empty.");

            // Add all the states to this.
            foreach ((string name, State s) in states)
            {
                AddState(name, s);
            }

            // If an initial current state was specified.
            if (states.TryGetValue(initialCurrentState, out State? state))
            {
                currentState = state;
            }
            // If no initial state was specified, choose the first entry of states.
            else
            {
                currentState = states.Values.ToList()[0];
            }
        }


        /// <summary>
        /// Updates the current <see cref="State"/>.
        /// </summary>
        /// <param name="gameTime"><see cref="GameTime"/> of the game.</param>
        public virtual void Update(GameTime gameTime)
        {
            currentState.Update(gameTime);
        }


        /// <summary>
        /// Draws the current <see cref="State"/>.
        /// </summary>
        /// <param name="spriteBatch"><see cref="SpriteBatch"/> used for drawing.</param>
        public virtual void Draw(GameTime gameTime)
        {
            currentState.Draw(gameTime);
        }


        /// <summary>
        /// Add a <see cref="State"/> to the <see cref="StateMachine"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="state"></param>
        public void AddState(string name, State state)
        {
            states.Add(name, state);
            // Regester the CallChangeState event with this.
            state.OnChangeState += ChangeState;
            state.OnQuit += Quit;
        }


        /// <summary>
        /// Removes the <see cref="State"/> with the given name from the <see cref="StateMachine"/>.
        /// </summary>
        /// <param name="name">Name of the <see cref="State"/> to remove.</param>
        public void RemoveState(string name)
        {
            if (states.ContainsKey(name))
                states.Remove(name);
        }


        /// <summary>
        /// Change the current <see cref="State"/>.
        /// </summary>
        /// <param name="sender">Object that called for the <see cref="State"/> to be changed.</param>
        /// <param name="stateName">Name of the <see cref="State"/> to change to.</param>
        /// <remarks>
        /// If the named <see cref="State"/> does not exist in this, the current state will not change.
        /// </remarks>
        public void ChangeState(object? sender, string stateName)
        {
            if (states.TryGetValue(stateName, out State? state))
            {
                currentState = state;
            }
        }


        /// <summary>
        /// Sends a request to quit the game.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Quit(object sender, EventArgs e)
        {
            if (OnQuit != null)
                OnQuit(this, null);
        }
    }
}
