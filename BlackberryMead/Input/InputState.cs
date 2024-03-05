using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;

///<summary>
///x
///</summary>
namespace BlackberryMead.Input
{
    /// <summary>
    /// A utility class that stores the state of game <c>actions</c>.<br/>
    /// An <c>action</c> is a string that corresponds to a named <see cref="Keybinds.Keybind"/>.<br/>
    /// To say that an <c>action</c> was "triggered" is to say that its associated <see cref="Keybinds.Keybind"/> was satisfied
    /// by player input.
    /// </summary>
    public struct InputState
    {
        /// <summary>
        /// List of action names.
        /// </summary>
        public List<string> ActionNames;

        /// <summary>
        /// Position of the mouse on the screen.
        /// </summary>
        public Point MousePosition { get; set; }

        /// <summary>
        /// Change in the Scroll Wheel Value of the mouse.
        /// </summary>
        public int ScrollWheelDelta { get; set; }

        /// <summary>
        /// Dictionary of action names and their associated <see cref="Keybind"/>.
        /// </summary>
        public Dictionary<string, Keybind> ActionBinds { get; set; }

        /// <summary>
        /// Dictionary of action names and whether they were triggered based on the current state.
        /// </summary>
        public Dictionary<string, bool> ActionStates { get; set; }


        /// <summary>
        /// Create a new InputState.
        /// </summary>
        public InputState((MouseState currentState, MouseState prevState) mouseState,
            (KeyboardState currentState, KeyboardState prevState) keyboardState,
            (GamePadState currentState, GamePadState prevState) gamePadState,
            Dictionary<string, Keybind> actions)
        {
            actions ??= new Dictionary<string, Keybind>();
            ActionBinds = actions;
            ActionNames = actions.Keys.ToList();
            ActionStates = actions.ToDictionary(kvp => kvp.Key, kvp => false);

            MousePosition = mouseState.currentState.Position;
            ScrollWheelDelta = mouseState.currentState.ScrollWheelValue - mouseState.prevState.ScrollWheelValue;

            foreach ((string name, Keybind keybind) in ActionBinds)
            {
                if (Keybind.EvaluateKeybind(keybind, mouseState, keyboardState, gamePadState))
                    ActionStates[name] = true;
            }
        }


        /// <summary>
        /// Creates a new InputState from a collection of actionBinds and actionStates.
        /// </summary>
        /// <param name="actionBinds"></param>
        /// <param name="actionStates"></param>
        InputState(Dictionary<string, Keybind> actionBinds, Dictionary<string, bool> actionStates,
            Point mousePosition, int ScrollWheelDelta)
        {
            this.ActionBinds = actionBinds;
            this.ActionStates = actionStates;
            this.ActionNames = actionBinds.Keys.ToList();
            MousePosition = mousePosition;
            this.ScrollWheelDelta = ScrollWheelDelta;
        }


        /// <summary>
        /// Gets whether the action with the given name was triggered.
        /// </summary>
        /// <param name="actionName">Name of the action to check the state of.</param>
        /// <returns></returns>
        public bool IsActionTriggered(string actionName)
        {
            if (ActionStates.TryGetValue(actionName, out bool value))
            {
                return value;
            }
            return false;
        }


        /// <summary>
        /// Gets whether the action with the given name was triggered.
        /// </summary>
        /// <param name="actionName">Name of the action to check the status of.</param>
        public bool this[string actionName]
        {
            get { return IsActionTriggered(actionName); }
        }


        /// <summary>
        /// Returns a copy of this.
        /// </summary>
        /// <returns>A deep <see cref="InputState"/> copy of this.</returns>
        public InputState ToNew()
        {
            return new InputState(this.ActionBinds, this.ActionStates, this.MousePosition, this.ScrollWheelDelta);
        }


        /// <summary>
        /// Preforms an inner join on a dictionary of named <see cref="Keybind"/>s and a list of named actions.
        /// </summary>
        /// <remarks>
        /// An Inner Join returns elements that are in both collections.
        /// </remarks>
        /// <param name="keybindSpace">Dictionary of action names and their associated <see cref="Keybind"/>.</param>
        /// <param name="actions">List of action names to retrieve a <see cref="Keybind"/> for.</param>
        /// <returns>A dictionary with action name keys from <paramref name="actions"/> that have a <see cref="Keybind"/>
        /// in <paramref name="keybindSpace"/>.</returns>
        public static Dictionary<string, Keybind> InnerJoin(Dictionary<string, Keybind> keybindSpace, List<string> actions)
        {
            return keybindSpace.Where(s => actions.Contains(s.Key)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
    }
}