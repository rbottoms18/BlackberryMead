using BlackberryMead.Framework;
using BlackberryMead.Utility.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace BlackberryMead.Input.UI
{
    /// <summary>
    /// A collection of <see cref="UIComponent"/> objects that can be scrolled through 
    /// vertically and viewed from a windowed display.
    /// </summary>
    [OptInJsonSerialization]
    public class ScrollViewer : UIComponent, IRenderable
    {
        /// <summary>
        /// Rectangle that the contents of the <see cref="ScrollViewer"/> are drawn to.
        /// </summary>
        [JsonInclude]
        public Rectangle ViewPort { get; protected set; }

        /// <summary>
        /// Dimensions of the <see cref="ViewPort"/>.
        /// </summary>
        [JsonInclude, JsonRequired]
        public Size ViewPortDimensions { get; protected set; }

        /// <summary>
        /// <see cref="UI.ScrollBar"/> used to scroll the <see cref="ScrollViewer"/>.
        /// </summary>
        [JsonInclude, JsonRequired]
        public ScrollBar ScrollBar { get; init; }

        /// <summary>
        /// Name of <see cref="ScrollBar"/>.
        /// </summary>
        [JsonInclude, JsonRequired]
        public string ScrollBarName { get; protected set; }

        /// <summary>
        /// <see cref="UIComponent"/> contained in this that can be scrolled through.
        /// </summary>
        [JsonInclude]
        public Dictionary<string, UIComponent> Components { get; protected set; } = new Dictionary<string, UIComponent>();

        /// <summary>
        /// Spacing between components in <see cref="Components"/>.
        /// </summary>
        [JsonInclude]
        public int Spacing { get; protected set; }

        /// <summary>
        /// Render target that the <see cref="Components"/> are drawn to.
        /// </summary>
        protected RenderTarget2D renderTarget;

        /// <summary>
        /// Size of <see cref="renderTarget"/>.
        /// </summary>
        protected Size RenderTargetDim
        {
            get { return x; }
            set { x = value; }
        }

        // What does x do???
        protected Size x;

        /// <summary>
        /// Value of the <see cref="UI.ScrollBar"/> on the previous update tick.
        /// </summary>
        protected float prevScrollValue;

        /// <summary>
        /// SpriteBatch for drawing to the render target.
        /// </summary>
        protected SpriteBatch spriteBatch;

        /// <summary>
        /// Rectangle of same dimensions as <see cref="ViewPort"/> that serves as the 
        /// source rectangle when sampling from the render target.
        /// </summary>
        protected Rectangle window;

        /// <summary>
        /// Number of pixels that the render target is longer than the view port.
        /// </summary>
        /// <remarks>
        /// How much the render target can be scrolled down.
        /// </remarks>
        protected int scrollFreedom;

        protected int Shift { get { return (int)(ScrollBar.Value * scrollFreedom); } }

        /// <summary>
        /// Create a new <see cref="ScrollViewer"/>.
        /// </summary>
        /// <param name="ViewPortDimensions"></param>
        /// <param name="ScrollBar"></param>
        /// <param name="Dimensions">Dimensions of the UIComponent's bounding rectangle.</param>
        /// <param name="VerticalAlign">Vertical allignment of the component inside its parent element.</param>
        /// <param name="HorizontalAlign">Horizontal allignment of the component inside its parent element.</param>
        /// <param name="VerticalOffset">Vertical offset of the component from its vertical allignment.</param>
        /// <param name="HorizontalOffset">Horizontal offset of the component from its horizontal allignment.</param>
        public ScrollViewer(Dictionary<string, UIComponent> Components, Size ViewPortDimensions,
            ScrollBar ScrollBar, UILayout Layout) :
            base(Layout)
        {
            // Set values
            this.ScrollBar = ScrollBar;
            this.ViewPortDimensions = ViewPortDimensions * Scale;
            this.Components = Components;

            // Realign ScrollBar to this
            ScrollBar.Realign(Rect);

            // Initialize window
            window = new Rectangle(Point.Zero, this.ViewPortDimensions);
            // If scroll bar value is pre-set, shift
            // **

            // Initialize ViewPort
            ViewPort = new Rectangle(Origin, this.ViewPortDimensions);

            // Calculate the size of the render target by finding the maximum of all the elements
            Size renderTargetDim = new(this.ViewPortDimensions.Width, 0);

            if (Components.Count == 0)
            {
                renderTargetDim = new Size(1, 1);
            }
            else
            {
                foreach (UIComponent element in Components.Values)
                {
                    if (renderTargetDim.Height < element.VerticalOffset)
                        // Add a little extra padding to avoid bleed
                        renderTargetDim.Height = element.VerticalOffset + element.Rect.Height + 10;
                }
            }
            RenderTargetDim = renderTargetDim;
            scrollFreedom = Math.Max(0, RenderTargetDim.Height - this.ViewPortDimensions.Height);
            ScrollBar.ScrollWheelFactor = scrollFreedom / 50;

            // Align all the components inside the render target
            foreach (UIComponent element in Components.Values)
            {
                element.Realign(new Rectangle(Point.Zero, RenderTargetDim));
            }
        }


        public override void Update(InputState input)
        {
            // Update the scroll bar
            ScrollBar.Update(input);

            // Shift the viewport by the scroll bar value if changed
            if (prevScrollValue != ScrollBar.Value)
            {
                window = new Rectangle(new Point(0, Shift), ViewPort.Size);
            }

            // Create a copy of input and adjust the MousePosition to translate inside the scroll viewer
            InputState scrollViewerInput = input.ToNew();
            scrollViewerInput.MousePosition = input.MousePosition - ViewPort.Location + new Point(0, Shift);

            // Update components inside the render target
            foreach (UIComponent element in Components.Values)
            {
                element.Update(scrollViewerInput);
            }

            // !Must be the last thing to be updated!
            prevScrollValue = ScrollBar.Value;
        }


        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.Draw(renderTarget, ViewPort, window, Color.White);

            ScrollBar.Draw(spriteBatch);
            //Shapes.DrawBorder(Rect, Color.Black, 1, spriteBatch);
            //Shapes.DrawBorder(ViewPort, Color.Red, 1, spriteBatch);
        }


        public override void Realign(Rectangle boundingRect)
        {
            // Reallign this inside the bounding rect
            base.Realign(boundingRect);

            // Reallign ViewPort
            ViewPort = new Rectangle(Origin, ViewPortDimensions);

            // Reallign scroll bar inside this's rect
            ScrollBar.Realign(Rect);
        }


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
            children.Add(ScrollBarName, ScrollBar);
            return children;
        }


        public override void LoadContent(ContentManager content)
        {
            base.LoadContent(content);
            foreach (UIComponent component in Components.Values)
            {
                component.LoadContent(content);
            }
        }


        public void Initialize(GraphicsDevice graphicsDevice)
        {
            renderTarget = new RenderTarget2D(graphicsDevice, RenderTargetDim.Width, RenderTargetDim.Height);
        }


        public void Render(SpriteBatch spriteBatch, GraphicsDevice graphicsDevice)
        {
            graphicsDevice.SetRenderTarget(renderTarget);
            graphicsDevice.Clear(Color.Transparent);

            spriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                DepthStencilState.None,
                RasterizerState.CullCounterClockwise,
                null, null);

            // Draw elements here
            foreach (UIComponent element in Components.Values)
            {
                element.Draw(spriteBatch);
            }

            spriteBatch.End();

            graphicsDevice.SetRenderTarget(null);
        }
    }
}
