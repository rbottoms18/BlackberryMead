#nullable enable
using BlackberryMead.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace BlackberryMead.Input.UI
{
    /// <summary>
    /// A Group that acts as a self contained menu. When visible adds Actions
    /// of its Children to the <see cref="UserInterface.CurrentWindowActions"/>.
    /// </summary>
    [Serializable]
    public class Window : Group
    {
        /// <summary>
        /// Source rectangle of the background image of this.
        /// </summary>
        [JsonInclude]
        public Rectangle BackgroundSourceRect { get; protected set; }

        /// <summary>
        /// List of actions manually included to this.
        /// </summary>
        [JsonInclude]
        public List<string> IncludeActions { get; protected set; } = new();

        /// <summary>
        /// Whether the bounding Rectangle of this will automatically be adjusted to match
        /// the <see cref="UserInterface"/> viewport when resized.
        /// </summary>
        /// <remarks>
        /// <see cref="UIComponent.Dimensions"/> if set will override <see cref="MatchViewport"/>.
        /// </remarks>
        [JsonInclude]
        public bool MatchViewport { get; init; }

        public override List<string> Actions => actions;

        ///<inheritdoc cref = "Actions"/>
        private List<string> actions = new();


        /// <summary>
        /// Creates a new UIGroup.
        /// </summary>
        /// <inheritdoc/>
        [JsonConstructor]
        public Window(Dictionary<string, UIComponent> Components, List<string> IncludeActions, Size Dimensions,
            Rectangle BackgroundSourceRect, Alignment VerticalAlign, Alignment HorizontalAlign,
            int Scale, int VerticalOffset, int HorizontalOffset) :
            base(Components, false, Dimensions, VerticalAlign, HorizontalAlign, Scale, VerticalOffset, HorizontalOffset)
        {
            IncludeActions ??= new List<string>();
            Components ??= new Dictionary<string, UIComponent>();

            this.Components = Components;
            this.BackgroundSourceRect = BackgroundSourceRect;
            this.IncludeActions = IncludeActions;

            // Add actions of children and IncludeActions to the actions of this.
            List<string> childrenActions = GetChildren().Values.SelectMany(x => x.Actions).ToList();
            this.actions = this.actions.Concat(childrenActions).Distinct().ToList().Concat(IncludeActions).Distinct().ToList();
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsVisible)
                return;

            // Draw background rect
            if (!BackgroundSourceRect.IsEmpty)
                spriteBatch.Draw(Spritesheet, Rect, BackgroundSourceRect, Color.White);

            base.Draw(spriteBatch);
        }


        /// <summary>
        /// Method called when this is begin closed.
        /// </summary>
        public override void OnClose()
        {
            base.OnClose();
            foreach (UIComponent component in Components.Values)
                component.OnClose();
        }
    }
}
