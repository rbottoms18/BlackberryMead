﻿using BlackberryMead.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace BlackberryMead.Input.UI
{
    /// <summary>
    /// A collection of <see cref="UIComponent"/> that act as one unit.
    /// </summary>
    public class Group : UIComponent
    {
        /// <summary>
        /// Dictionary of components in the group. Keys are component
        /// names/structures and values are UIComponents.
        /// </summary>
        public Dictionary<string, UIComponent> Components { get; set; } = new Dictionary<string, UIComponent>();

        /// <summary>
        /// Changes the dimensions of this to exactly encompass <see cref="Components"/>.
        /// </summary>
        public bool AutoResize { get; protected set; } = false;

        /// <summary>
        /// Create a new Group.
        /// </summary>
        /// <param name="Components">Subcomponents of this.</param>
        /// <inheritdoc cref="UIComponent(Size, Alignment, Alignment, int, int, int)"/>
        [JsonConstructor]
        public Group(Dictionary<string, UIComponent> Components, bool AutoResize, Size Dimensions,
            Alignment VerticalAlign, Alignment HorizontalAlign,
            int Scale, int VerticalOffset, int HorizontalOffset) :
            base(Dimensions, VerticalAlign, HorizontalAlign, Scale, VerticalOffset, HorizontalOffset)
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
