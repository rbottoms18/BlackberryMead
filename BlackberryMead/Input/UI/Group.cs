using BlackberryMead.Framework;
using BlackberryMead.Utility;
using BlackberryMead.Utility.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace BlackberryMead.Input.UI
{
    /// <summary>
    /// A collection of <see cref="UIComponent"/> that act as one unit.
    /// </summary>
    [OptInJsonSerialization]
    public class Group : UIComponent
    {
        /// <summary>
        /// Dictionary of components in the group. Keys are component
        /// names/structures and values are UIComponents.
        /// </summary>
        [JsonOptIn, JsonInclude]
        public Dictionary<string, UIComponent> Components { get; set; } = new Dictionary<string, UIComponent>();

        /// <summary>
        /// Changes the dimensions of this to exactly encompass <see cref="Components"/>.
        /// </summary>
        [JsonOptIn]
        public bool AutoResize { get; protected set; } = false;

        /// <summary>
        /// Create a new Group.
        /// </summary>
        /// <param name="Components">Subcomponents of this.</param>
        /// <inheritdoc cref="UIComponent(Size, Alignment, Alignment, int, int, int)"/>
        [JsonConstructor]
        public Group(Dictionary<string, UIComponent> Components, bool AutoResize, UILayout Layout) :
            base(Layout)
        {
            Components ??= new Dictionary<string, UIComponent>();
            this.Components = Components;
            this.AutoResize = AutoResize;

            if (AutoResize)
                FitDimensions();
        }


        public override void Realign(Rectangle boundingRect)
        {
            base.Realign(boundingRect);

            if (AutoResize)
                FitDimensions();

            foreach (UIComponent component in Components.Values)
            {
                component.Realign(Rect);
            }
        }


        /// <summary>
        /// Updates this. Updates all of its subcomponents.
        /// </summary>
        /// <param name="input">Input of the current tick.</param>
        public override void Update(InputState input)
        {
            foreach (UIComponent component in Components.Values)
            {
                if (component.IsVisible)
                    component.Update(input);
            }
        }


        /// <summary>
        /// Draw this to the current rendertarget. Draws all of its subcomponents.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!IsVisible)
                return;

            if (ShowBorder)
                Shapes.DrawBorder(Rect, Color.Red, borderWidth, spriteBatch);

            // Draw each of its components
            foreach (UIComponent component in Components.Values)
            {
                if (component.IsVisible)
                    component.Draw(spriteBatch);
            }
        }


        /// <summary>
        /// Gets a dictionary of all UIElement children contained in this with element names as keys.
        /// </summary>
        /// <remarks>
        /// Does not include itself as a child.
        /// </remarks>
        /// <returns>List of children UIElements contained in this UIGroup</returns>
        public override Dictionary<string, UIComponent> GetChildren()
        {
            Dictionary<string, UIComponent> children = new() { };
            for (int i = 0; i < Components.Count; i++)
            {
                // Add the element at index i
                children.Add(Components.Keys.ElementAt(i), Components[Components.Keys.ElementAt(i)]);
                // Add all of that element's children
                children = children.Concat(Components.Values.ElementAt(i).GetChildren()).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            }
            return children;
        }


        /// <summary>
        /// Gets the Child element with the given name from in this.
        /// </summary>
        /// <typeparam name="T">Type of the child component where <typeparamref name="T"/> : <see cref="UIComponent"/>.</typeparam> 
        /// <param name="name">Name of the child component.</param>
        /// <param name="child">The child component of type <typeparamref name="T"/>.</param>
        /// <returns>True if a component of the given type with the given name exists inside this, false if not.</returns>
        public bool GetChild<T>(string name, out T? child) where T : UIComponent
        {
            if (GetChildren().TryGetValue(name, out var element))
            {
                if (element is T t)
                {
                    child = t;
                    return true;
                }
            }
            child = default;
            return false;
        }


        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);
            foreach (UIComponent c in Components.Values)
            {
                c.LoadContent(content);
            }
        }


        /// <summary>
        /// Resizes the <see cref="UIComponent.Dimensions"/> of this to exactly fit <see cref="Components"/>.
        /// </summary>
        private void FitDimensions()
        {
            Size dim = new Size();
            foreach (UIComponent component in Components.Values)
            {
                int componentWidth = component.Dimensions.Width + component.HorizontalOffset;
                if (componentWidth > dim.Width)
                    dim.Width = componentWidth;

                int componentHeight = component.Dimensions.Height + component.VerticalOffset;
                if (componentHeight > dim.Height)
                    dim.Height = componentHeight;
            }
            this.Dimensions = dim;
        }
    }
}
