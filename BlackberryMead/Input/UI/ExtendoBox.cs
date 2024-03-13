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
    /// <see cref="UIGroup"/> in which Components are defined relative to each other in a vertical
    /// list rather than relative to an absolute position. Automatically resizes to encapsulate 
    /// its components.
    /// </summary>
    [OptInJsonSerialization]
    public class ExtendoBox : UIComponent
    {
        /// <summary>
        /// List of components in this.
        /// </summary>
        [JsonInclude]
        public Dictionary<string, UIComponent> Components { get; protected set; }

        /// <summary>
        /// Class that defines padding for each child of this.
        /// </summary>
        public class Pad
        {
            public int Top;
            public int Bottom;

            public Pad(int Top, int Bottom)
            {
                this.Top = Top;
                this.Bottom = Bottom;
            }
        }

        /// <summary>
        /// Padding of each child of this. Keys are Component names as in <see cref="Components"/>,
        /// values are objects with a "Top" padding and a "Bottom" padding in pixels.
        /// </summary>
        [JsonInclude]
        public Dictionary<string, Pad> Padding { get; protected set; }


        /// <summary>
        /// Create a new ExtendoBox
        /// </summary>
        /// <param name="Components">Sub-components of thsi. All components added to this
        /// will have VerticalAlign and HorizontalAlign set to Alignment.None.</param>
        /// <param name="Padding">Dictionary of top and bottom padding per component indexed by names
        /// from <see cref="Components"/>.</param>
        /// <inheritdoc cref="UIComponent(Size, Alignment, Alignment, int, int, int)"/>
        [JsonConstructor]
        public ExtendoBox(Dictionary<string, UIComponent> Components, Dictionary<string, Pad> Padding,
            UILayout Layout) :
            base(Layout)
        {
            this.Components = Components;
            this.Padding = Padding;

            // Align padded components
            int yOffset = 0;
            int maxX = 0;
            foreach ((string name, UIComponent component) in Components)
            {
                int topPad = 0;
                int bottomPad = 0;
                // Try and get padding for this component
                if (Padding.TryGetValue(name, out Pad padding))
                {
                    topPad = padding.Top;
                    bottomPad = padding.Bottom;
                }

                // Set Alignment to None to keep order
                component.VerticalAlign = Alignment.None;
                component.HorizontalAlign = Alignment.None;

                // Apply padding to offsets
                component.VerticalOffset = yOffset + topPad;
                yOffset += component.Dimensions.Height + bottomPad;

                // Compare maxX
                int componentX = component.Dimensions.Width + component.HorizontalOffset;
                if (componentX > maxX)
                    maxX = componentX;
            }

            // Set the dimensions of this
            this.Dimensions = new Size(maxX, yOffset);
        }


        public override void Update(InputState input)
        {
            foreach (UIComponent component in Components.Values)
            {
                component.Update(input);
            }
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            foreach (UIComponent component in Components.Values)
            {
                component.Draw(spriteBatch);
            }
            base.Draw(spriteBatch);
        }


        public override void Realign(Rectangle boundingRect)
        {
            base.Realign(boundingRect);

            foreach (UIComponent component in Components.Values)
            {
                component.Realign(Rect);
            }
        }


        public override Dictionary<string, UIComponent> GetChildren()
        {
            Dictionary<string, UIComponent> children = new() { };
            for (int i = 0; i < Components.Count; i++)
            {
                // Add the element at index i
                children.Add(Components.Keys.ElementAt(i), Components[Components.Keys.ElementAt(i)]);
                // Add all of that element's children
                children = children.Concat(Components.Values.ElementAt(i).GetChildren()).ToDictionary(
                    kvp => kvp.Key, kvp => kvp.Value);
            }
            return children;
        }


        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);
            foreach (UIComponent c in Components.Values)
            {
                c.LoadContent(content);
            }
        }
    }
}
