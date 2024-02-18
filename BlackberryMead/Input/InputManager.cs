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
        /// MouseState of the current update tick.
        /// </summary>
        public MouseState CurrentMouseState;

        /// <summary>
        /// MouseState of the previous update tick.
        /// </summary>
        public MouseState PrevMouseState;

        /// <summary>
        /// KeyboardState of the current update tick.
        /// </summary>
        public KeyboardState CurrentKeyboardState;

        /// <summary>
        /// KeyboardState of the previous update tick.
        /// </summary>
        public KeyboardState PrevKeyboardState;

        /// <summary>
        /// GamePadState of the current update tick.
        /// </summary>
        public GamePadState CurrentGamePadState;

        /// <summary>
        /// GamePadState of the previous update tick.
        /// </summary>
        public GamePadState PrevGamePadState;

        public InputManager() { }

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
