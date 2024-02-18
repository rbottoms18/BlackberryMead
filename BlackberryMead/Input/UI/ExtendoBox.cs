using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace BlackberryMead.Input.UI
{
    /// <summary>
    /// <see cref="UIGroup"/> in which Components are defined relative to each other in a vertical
    /// list rather than relative to an absolute position. Automatically resizes to encapsulate 
    /// its components.
    /// </summary>
    internal class ExtendoBox : UIComponent
    {
        /// <summary>
        /// List of components in this.
        /// </summary>
        [JsonInclude]
        public Dictionary<string, PaddedComponent> Components { get; protected set; }

        /// <summary>
        /// Class that defines a Component and blank space padding on its top and bottom for
        /// use in Json deserialization.
        /// </summary>
        internal class PaddedComponent
        {
            public UIComponent Component { get; set; }
            public int PadTop { get; set; }
            public int PadBottom { get; set; }

            [JsonConstructor]
            public PaddedComponent(UIComponent Component, int PadTop, int PadBottom)
            {
                this.Component = Component;
                this.PadTop = PadTop;
                this.PadBottom = PadBottom;
            }
        }


        /// <summary>
        /// Create a new ExtendoBox
        /// </summary>
        /// <param name="Components">Components and the padding between them. All components added to this
        /// will have VerticalAlign and HorizontalAlign set to Alignment.None.</param>
        /// <inheritdoc cref="UIComponent(Size, Alignment, Alignment, int, int, int)"/>
        [JsonConstructor]
        public ExtendoBox(Dictionary<string, PaddedComponent> Components, Size Dimensions,
            Alignment VerticalAlign, Alignment HorizontalAlign,
            int Scale, int VerticalOffset, int HorizontalOffset) :
            base(Dimensions, VerticalAlign, HorizontalAlign, Scale, VerticalOffset, HorizontalOffset)
        {
            this.Components = Components;

            // Align padded components
            int yOffset = 0;
            int maxX = 0;
            foreach (PaddedComponent c in Components.Values)
            {
                // Set Alignment to None to keep order
                c.Component.VerticalAlign = Alignment.None;
                c.Component.HorizontalAlign = Alignment.None;

                // Apply padding to offsets
                c.Component.VerticalOffset = yOffset + c.PadTop;
                yOffset += c.Component.Dimensions.Height + c.PadBottom;

                // Compare maxX
                int componentX = c.Component.Dimensions.Width + c.Component.HorizontalOffset;
                if (componentX > maxX)
                    maxX = componentX;
            }

            // Set the dimensions of this
            this.Dimensions = new Size(maxX, yOffset);
        }


        public override void Update(InputState input)
        {
            foreach (PaddedComponent c in Components.Values)
            {
                c.Component.Update(input);
            }
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (PaddedComponent c in Components.Values)
            {
                c.Component.Draw(spriteBatch);
            }
            base.Draw(spriteBatch);
        }


        public override void Realign(Rectangle boundingRect)
        {
            base.Realign(boundingRect);

            foreach (PaddedComponent component in Components.Values)
            {
                component.Component.Realign(Rect);
            }
        }


        public override Dictionary<string, UIComponent> GetChildren()
        {
            Dictionary<string, UIComponent> children = new() { };
            for (int i = 0; i < Components.Count; i++)
            {
                // Add the element at index i
                children.Add(Components.Keys.ElementAt(i), Components[Components.Keys.ElementAt(i)].Component);
                // Add all of that element's children
                children = children.Concat(Components.Values.ElementAt(i).Component.GetChildren()).ToDictionary(
                    kvp => kvp.Key, kvp => kvp.Value);
            }
            return children;
        }
    }
}
