using BlackberryMead.Framework;
using BlackberryMead.Utility.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace BlackberryMead.Input.UI
{
    /// <summary>
    /// A <see cref="Group"/> that acts as a self contained menu. When visible adds the actions
    /// of its children to the <see cref="UserInterface.CurrentWindowActions"/>.
    /// </summary>
    [OptInJsonSerialization]
    public class Window : Group
    {
        /// <summary>
        /// Source rectangle of the background image of the <see cref="Group"/>.
        /// </summary>
        [JsonInclude, JsonOptIn]
        public Rectangle BackgroundSourceRect { get; protected set; }

        /// <summary>
        /// List of actions manually included in the <see cref="Group"/>.
        /// </summary>
        [JsonInclude, JsonOptIn]
        public List<string> IncludeActions { get; protected set; } = new();

        /// <summary>
        /// Whether the bounding rectangle of this will automatically be adjusted to match
        /// the <see cref="UserInterface"/> viewport when resized.
        /// </summary>
        /// <remarks>
        /// <see cref="UIComponent.Dimensions"/> if set will override <see cref="MatchViewport"/>.
        /// </remarks>
        [JsonInclude, JsonOptIn]
        public bool MatchViewport { get; init; }

        public override List<string> Actions => actions;

        ///<inheritdoc cref = "Actions"/>
        private List<string> actions = new();


        /// <summary>
        /// Creates a new <see cref="Window"/>.
        /// </summary>
        /// <param name="IncludeActions">Extra actions to be made avaliable when this is open.</param>
        /// <param name="BackgroundSourceRect">Source rectangle of the background sprite.</param>
        /// <param name="Components">Subcomponents (children) of this.</param>
        /// <inheritdoc cref="UIComponent.UIComponent(Size, Alignment, Alignment, int, int, int)"/>
        [JsonConstructor]
        public Window(Dictionary<string, UIComponent> Components, List<string> IncludeActions,
            Rectangle BackgroundSourceRect, UILayout Layout) :
            base(Components, false, Layout)
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


        public override void OnClose()
        {
            base.OnClose();
            foreach (UIComponent component in Components.Values)
                component.OnClose();
        }


        public override void OnOpen()
        {
            base.OnOpen();
            foreach (UIComponent component in Components.Values)
                component.OnOpen();
        }
    }
}
