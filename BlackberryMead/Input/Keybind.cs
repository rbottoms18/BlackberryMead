using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;


namespace BlackberryMead.Input
{
    /// <summary>
    /// A collection of keys that represents an input sequence.
    /// </summary>
    [Serializable]
    public struct Keybind
    {
        /// <summary>
        /// Possible types of actions required to trigger a <see cref="Keybind"/>.
        /// </summary>
        public enum ActionType
        {
            Hold,
            Press
        }

        /// <summary>
        /// An empty <see cref="Keybind"/>. The empty <see cref="Keybind"/> is never satisfied (cannot be pressed).
        /// </summary>
        public static Keybind EmptyBind = new(Keys.None, new List<Keys>(), MouseButton.None, ActionType.Press);

        /// <summary>
        /// Keys that are valid Auxiliary keys.
        /// </summary>
        public static List<Keys> ValidAuxKeys = new() { Keys.LeftShift, Keys.LeftControl, Keys.LeftAlt };

        /// <summary>
        /// Main key that must be pressed to trigger the action.
        /// </summary>
        public Keys Key { get; set; } = Keys.None;

        /// <summary>
        /// Auxiliary keys that must be held while <paramref name="Key"></paramref> 
        /// and/or <paramref name="MouseButton"></paramref> 
        /// is pressed to trigger the action.
        /// </summary>
        public List<Keys> AuxKeys { get; set; } = new List<Keys>();

        /// <summary>
        /// <see cref="MouseButton"/> that must be clicked to trigger the action.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public MouseButton MouseButton { get; set; } = MouseButton.None;

        /// <summary>
        /// The type of input the <see cref="Keybind"/> requires to trigger.
        /// </summary>
        [JsonRequired, JsonInclude, JsonConverter(typeof(JsonStringEnumConverter))]
        public ActionType Type { get; private set; }


        /// <summary>
        /// Creates an empty <see cref="Keybind"/>.
        /// </summary>
        [JsonConstructor]
        public Keybind()
        {
            AuxKeys ??= new List<Keys>();
        }


        /// <summary>
        /// Creates a new <see cref="Keybind"/> with no auxiliary keys.
        /// </summary>
        /// <param name="key">Key that triggers the action.</param>
        /// <param name="type">Type of press that triggers the action.</param>
        public Keybind(Keys key, ActionType type)
        {
            this.Key = key;
            AuxKeys = new List<Keys>();
            MouseButton = MouseButton.None;
            Type = type;
        }


        /// <summary>
        /// Creates a new <see cref="Keybind"/>.
        /// </summary>
        /// <param name="key">Key that triggers the action.</param>
        /// <param name="auxKeys">Keys that must be held in addition to <paramref name="key"/> for the action to trigger.</param>
        /// <param name="type">Type of press that triggers the action.</param>
        public Keybind(Keys key, List<Keys> auxKeys, ActionType type)
        {
            this.Key = key;
            this.AuxKeys = auxKeys;
            MouseButton = MouseButton.None;
            Type = type;
        }


        /// <summary>
        /// Creates a new <see cref="Keybind"/> that only checks for held auxiliary keys with no trigger.
        /// </summary>
        /// <param name="auxKeys">Keys that must be held for the action to trigger.</param>
        /// <param name="type">Type of press that triggers the action.</param>
        public Keybind(List<Keys> auxKeys, ActionType type)
        {
            Key = Keys.None;
            AuxKeys = auxKeys;
            MouseButton = MouseButton.None;
            Type = type;
        }


        /// <summary>
        /// Create a new <see cref="Keybind"/> with a <see cref="MouseButton"/> press.
        /// </summary>
        /// <param name="mouseButton"><see cref="MouseButton"/> that must be pressed to trigger the action.</param>
        /// <param name="type">Type of press that triggers the action.</param>
        public Keybind(MouseButton mouseButton, ActionType type)
        {
            Key = Keys.None;
            AuxKeys = new List<Keys>();
            MouseButton = mouseButton;
            Type = type;
        }


        /// <summary>
        /// Create a new keybinding with a <see cref="MouseButton"/> press and auxiliary keys
        /// </summary>
        /// <param name="mouseButton"><see cref="MouseButton"/> that must be pressed to trigger the action.</param>
        /// <param name="auxKeys">Keys that must be held in addition to <paramref name="mouseButton"/> for the action to trigger.</param>
        /// <param name="type">Type of press that triggers the action.</param>
        public Keybind(MouseButton mouseButton, List<Keys> auxKeys, ActionType type)
        {
            Key = Keys.None;
            AuxKeys = auxKeys;
            MouseButton = mouseButton;
            Type = type;
        }


        /// <summary>
        /// Create a new <see cref="Keybind"/> with key presses and a <see cref="MouseButton"/> press.
        /// </summary>
        /// <param name="key">Key that triggers the action.</param>
        /// <param name="auxKeys">Keys that must be held in addition to <paramref name="mouseButton"/> and 
        /// <paramref name="mouseButton"/> for the action to trigger.</param>
        /// <param name="mouseButton"><see cref="MouseButton"/> that must be pressed to trigger the action.</param>
        /// <param name="type">Type of press that triggers the action.</param>
        public Keybind(Keys key, List<Keys> auxKeys, MouseButton mouseButton, ActionType type)
        {
            Key = key;
            AuxKeys = auxKeys;
            MouseButton = mouseButton;
            Type = type;
        }


        /// <summary>
        /// Evaluates whether a <see cref="Keybind"/> is satisfied.
        /// </summary>
        /// <param name="mouseState">Pair of current and previous <see cref="MouseState"/> objects.</param>
        /// <param name="keyboardState">Pair of current and previous <see cref="KeyboardState"/> objects.</param>
        /// <param name="gamePadState">Pair of current and previous <see cref="GamePadState"/> objects.</param>
        /// <returns></returns>
        public static bool EvaluateKeybind(Keybind keybind, (MouseState currentState, MouseState prevState) mouseState,
            (KeyboardState currentState, KeyboardState prevState) keyboardState,
            (GamePadState currentState, GamePadState prevState) gamePadState)
        {
            // If the keybind is empty, return false
            if (keybind == EmptyBind)
                return false;

            // Check mouse key if using
            if (keybind.MouseButton != MouseButton.None)
            {
                switch (keybind.MouseButton)
                {
                    case MouseButton.Left:
                        if (keybind.Type == Keybind.ActionType.Hold)
                        {
                            if (mouseState.currentState.LeftButton != ButtonState.Pressed) return false;
                        }
                        else if (keybind.Type == Keybind.ActionType.Press)
                        {
                            if (mouseState.currentState.LeftButton != ButtonState.Released ||
                                mouseState.prevState.LeftButton != ButtonState.Pressed) return false;
                        }
                        break;

                    case MouseButton.Right:
                        if (keybind.Type == Keybind.ActionType.Hold)
                        {
                            if (mouseState.currentState.RightButton != ButtonState.Pressed) return false;
                        }
                        else if (keybind.Type == Keybind.ActionType.Press)
                        {
                            if (mouseState.currentState.RightButton != ButtonState.Released ||
                                mouseState.prevState.RightButton != ButtonState.Pressed) return false;
                        }
                        break;

                    case MouseButton.Middle:
                        if (keybind.Type == Keybind.ActionType.Hold)
                        {
                            if (mouseState.currentState.MiddleButton != ButtonState.Pressed) return false;
                        }
                        else if (keybind.Type == Keybind.ActionType.Press)
                        {
                            if (mouseState.currentState.MiddleButton != ButtonState.Released ||
                                mouseState.prevState.MiddleButton != ButtonState.Pressed) return false;
                        }
                        break;
                }
            }

            // Check that AUX keys are held down
            foreach (Keys key in keybind.AuxKeys)
                if (keyboardState.currentState.IsKeyUp(key)) return false;

            // Check the trigger key to the evaluation method (either held or pressed)
            if (keybind.Key != Keys.None)
            {
                if (keybind.Type == ActionType.Hold)
                {
                    if (keyboardState.currentState.IsKeyUp(keybind.Key)) return false;
                }
                else if (keybind.Type == ActionType.Press)
                {
                    if (!keyboardState.currentState.IsKeyUp(keybind.Key) ||
                        !keyboardState.prevState.IsKeyDown(keybind.Key)) return false;
                }
            }

            ///
            /// Insert GamePad logic here
            ///

            // Return true if all previous checks passed
            return true;
        }


        public static bool operator ==(Keybind a, Keybind b)
        {
            if (Object.Equals(a, null) && Object.Equals(b, null))
                return true;

            if (Object.Equals(a, null) || Object.Equals(b, null))
                return false;

            return (a.MouseButton == b.MouseButton) && (a.Key == b.Key) && (a.AuxKeys == b.AuxKeys);
        }


        public static bool operator !=(Keybind a, Keybind b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
