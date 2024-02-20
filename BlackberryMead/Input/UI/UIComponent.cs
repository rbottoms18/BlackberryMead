#nullable enable
using BlackberryMead.Framework;
using BlackberryMead.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Point = Microsoft.Xna.Framework.Point;

namespace BlackberryMead.Input.UI
{
    /// <summary>
    /// Visual structure that represents an interactable element of a UserInterface.
    /// </summary>
    public abstract class UIComponent
    {
        /// <summary>
        /// An Empty UIElement. Use instead of setting a UI element to null.
        /// </summary>
        public static NullUIElement Null = new();

        /// <summary>
        /// The percent opacity of a UIElement that is disabled.
        /// </summary>
        public const float DisabledOpacity = 0.5f;

        /// <summary>
        /// Spritesheet of the UIComponent
        /// </summary>
        public Texture2D? Spritesheet { get; set; }

        /// <summary>
        /// Name of the Spritesheet of this. If left empty,
        /// the containing <see cref="UserInterface.Spritesheet"/> will be used instead.
        /// </summary>
        [JsonInclude]
        public string SpritesheetName { get; set; } = "";

        /// <summary>
        /// Rectangle that the UIComponent is encapsulated in and drawn to
        /// </summary>
        public virtual Rectangle Rect { get; protected set; }

        /// <summary>
        /// Origin of the UIComponent insides its parent collection's Rectangle
        /// </summary>
        public virtual Point Origin { get; protected set; }

        /// <summary>
        /// Whether the UIComponent is currently visible or not
        /// </summary>
        public bool IsVisible = true;

        /// <summary>
        /// Whether this can be interacted with.
        /// </summary>
        public bool Enabled = true;

        /// <summary>
        /// The draw opacity of this.
        /// </summary>
        public float Opacity = 1f;

        /// <summary>
        /// Factor to which this is scaled.
        /// </summary>
        /// <remarks>
        /// Affects dimensions but does not affect horizontal and vertical offsets.
        /// </remarks>
        [JsonInclude]
        public int Scale { get; protected set; } = 1;

        /// <summary>
        /// Dimensions of the UIComponent.
        /// </summary>
        [JsonInclude]
        public Size Dimensions { get; set; }

        /// <summary>
        /// Vertical offset of the UIElement from its Origin.
        /// </summary>
        [JsonInclude]
        public int VerticalOffset { get; set; }

        /// <summary>
        /// Horizontal offset of the UIElement from its Origin.
        /// </summary>
        [JsonInclude]
        public int HorizontalOffset { get; set; }

        /// <summary>
        /// Vertical allignment of the UIElement against its Origin.
        /// </summary>
        [JsonInclude]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Alignment VerticalAlign { get; set; }

        /// <summary>
        /// Horizontal allignment of the UIElement against its Origin.
        /// </summary>
        [JsonInclude]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Alignment HorizontalAlign { get; set; }

        /// <summary>
        /// Enables the drawing of the bounding box.
        /// </summary>
        public bool ShowBorder { get; set; }

        /// <summary>
        /// List of actions subscribed to by this.
        /// </summary>
        public virtual List<string> Actions { get => new(); }

        /// <summary>
        /// Border width of the lines drawn by <see cref="ShowBorder"/>.
        /// </summary>
        protected static int borderWidth = 3;


        /// <summary>
        /// Creates a new UIComponent.
        /// Sets origin based on alignment conditions and initializes the bounding rectangle.
        /// </summary>
        /// <param name="Dimensions">Dimensions of the UIComponent's bounding rectangle.</param>
        /// <param name="VerticalAlign">Vertical alignment of the component inside its parent element.</param>
        /// <param name="HorizontalAlign">Horizontal alignment of the component inside its parent element.</param>
        /// <param name="VerticalOffset">Vertical offset of the component from its vertical alignment.</param>
        /// <param name="HorizontalOffset">Horizontal offset of the component from its horizontal alignment.</param>
        [JsonConstructor]
        public UIComponent(Size Dimensions, Alignment VerticalAlign, Alignment HorizontalAlign,
            int Scale, int VerticalOffset, int HorizontalOffset)
        {
            this.Scale = Scale > 0 ? Scale : 1;
            if (Dimensions != Size.Empty)
                this.Dimensions = Dimensions * this.Scale;
            this.VerticalAlign = VerticalAlign;
            this.HorizontalAlign = HorizontalAlign;
            this.VerticalOffset = VerticalOffset * this.Scale;
            this.HorizontalOffset = HorizontalOffset * this.Scale;
            Rect = new Rectangle(Origin, this.Dimensions);
        }


        /// <summary>
        /// Sets the origin of the UIComponent to align inside a rectangle.
        /// </summary>
        /// <param name="boundingRect">Rectangle to align against.</param>
        /// <param name="vAllign">Vertical alignment.</param>
        /// <param name="hAllign">Horizontal alignment.</param>
        /// <param name="dim">Dimensions of the UIComponent rectangle.</param>
        /// <param name="hOffset">Horizontal offset of the alignment in screen pixels.</param>
        /// <param name="vOffset">Vertical offset of the alignment in screen pixels.</param>
        public static Point Align(Rectangle boundingRect, MonoGame.Extended.Size dim,
            Alignment vAllign, Alignment hAllign, int vOffset = 0, int hOffset = 0)
        {
            Point origin = Point.Zero;

            // Allign vertically
            switch (vAllign)
            {
                // Top
                default:
                    origin.Y = boundingRect.Top;
                    break;

                case Alignment.Center:
                    origin.Y = boundingRect.Top + (boundingRect.Height / 2) - (dim.Height / 2);
                    break;

                case Alignment.Bottom:
                    origin.Y = boundingRect.Bottom - dim.Height;
                    break;

                case Alignment.None:
                    origin.Y = boundingRect.Y;
                    break;
            }

            // Allign horizontally
            switch (hAllign)
            {
                // Left
                default:
                    origin.X = boundingRect.Left;
                    break;

                case Alignment.Center:
                    origin.X = boundingRect.Left + (boundingRect.Width / 2) - (dim.Width / 2);
                    break;

                case Alignment.Right:
                    origin.X = boundingRect.Right - dim.Width;
                    break;

                case Alignment.None:
                    origin.X = boundingRect.X;
                    break;
            }

            // Offset
            origin += new Point(hOffset, vOffset);

            return origin;
        }


        /// <summary>
        /// Realign this inside on a bounding rectangle.
        /// </summary>
        public virtual void Realign(Rectangle boundingRect)
        {
            // Set origin based on alligments
            Origin = Align(boundingRect, Dimensions, this.VerticalAlign, this.HorizontalAlign, this.VerticalOffset,
                this.HorizontalOffset);

            Rect = new Rectangle(Origin, Dimensions);
        }


        /// <summary>
        /// Preforms update logic for this each in-game tick.
        /// </summary>
        public abstract void Update(InputState input);


        /// <summary>
        /// Draws this.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (!IsVisible)
                return;
            if (ShowBorder)
                Shapes.DrawBorder(Rect, Color.Blue, borderWidth, spriteBatch);
        }


        /// <summary>
        /// Preform logic when the this' parent UserInterface is closed.
        /// </summary>
        public virtual void OnClose() { }


        /// <summary>
        /// Gets a list of all UIElement children contained in this. Does not include itself as a child.
        /// </summary>
        /// <returns>List of children UIElements contained in this UIElement.
        /// Must be overridden if the Component contains subcomponents.</returns>
        public virtual Dictionary<string, UIComponent> GetChildren()
        {
            return new Dictionary<string, UIComponent> { };
        }


        /// <summary>
        /// Disables this and makes it uninteractable.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Disable(object sender, EventArgs e)
        {
            Enabled = false;
            Opacity = DisabledOpacity;
        }

        /// <summary>
        /// Enables this and makes it interactable.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Enable(object sender, EventArgs e)
        {
            Enabled = true;
            Opacity = 1f;
        }


        /// <summary>
        /// Loads content required by this.
        /// </summary>
        /// <param name="content"></param>
        public virtual void LoadContent(ContentManager content)
        {
            if (SpritesheetName != "")
            {
                try
                {
                    Spritesheet = content.Load<Texture2D>(SpritesheetName);
                }
                catch { }
            }
        }
    }


    /// <summary>
    /// NOP implimentation of the UIElement. 
    /// </summary>
    public class NullUIElement : UIComponent
    {
        /// <summary>
        /// Create a new NullUIElement
        /// </summary>
        public NullUIElement() : base(Size.Empty, Alignment.Top, Alignment.Left, 1, 0, 0)
        { }


        public override void Draw(SpriteBatch spriteBatch) { }


        public override void Update(InputState input) { }
    }
}
