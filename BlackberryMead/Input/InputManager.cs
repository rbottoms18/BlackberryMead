using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace BlackberryMead.Input
{
    /// <summary>
    /// Helper class that manages current and previous states for Mouse, Keyboard, and GamePad.
    /// </summary>
    public class InputManager
    {
        /// <summary>
        /// <see cref="MouseState"/> of the current update tick.
        /// </summary>
        public MouseState CurrentMouseState;

        /// <summary>
        /// <see cref="MouseState"/> of the previous update tick.
        /// </summary>
        public MouseState PrevMouseState;

        /// <summary>
        /// <see cref="KeyboardState"/> of the current update tick.
        /// </summary>
        public KeyboardState CurrentKeyboardState;

        /// <summary>
        /// <see cref="KeyboardState"/> of the previous update tick.
        /// </summary>
        public KeyboardState PrevKeyboardState;

        /// <summary>
        /// <see cref="GamePadState"/> of the current update tick.
        /// </summary>
        public GamePadState CurrentGamePadState;

        /// <summary>
        /// <see cref="GamePadState"/> of the previous update tick.
        /// </summary>
        public GamePadState PrevGamePadState;


        /// <summary>
        /// Create a new empty <see cref="InputManager"/>.
        /// </summary>
        public InputManager() { }


        /// <summary>
        /// Updates the state of the <see cref="InputManager"/>.
        /// </summary>
        /// <param name="actions">Actions that can be taken on this update.</param>
        /// <remarks>
        /// Updates the previous and current Mouse, Keyboard, and Gamepad states. Computes
        /// whether each action in <paramref name="actions"/> has had its <see cref="Keybind"/> satisfied.
        /// </remarks>
        /// <returns></returns>
        public InputState GetState(Dictionary<string, Keybind> actions)
        {
            // Set current states to previous
            PrevMouseState = CurrentMouseState;
            PrevKeyboardState = CurrentKeyboardState;
            PrevGamePadState = CurrentGamePadState;

            // Get new current states
            CurrentMouseState = Mouse.GetState();
            CurrentKeyboardState = Keyboard.GetState();
            CurrentGamePadState = GamePad.GetState(0);

            return new InputState((CurrentMouseState, PrevMouseState),
            (CurrentKeyboardState, PrevKeyboardState),
            (CurrentGamePadState, PrevGamePadState), actions);
        }
    }
}
