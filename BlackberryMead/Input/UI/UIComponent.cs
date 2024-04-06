#nullable enable
using BlackberryMead.Framework;
using BlackberryMead.Utility;
using BlackberryMead.Utility.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Point = Microsoft.Xna.Framework.Point;

namespace BlackberryMead.Input.UI
{
    /// <summary>
    /// Visual structure that represents an interactable element of a UserInterface.
    /// </summary>
    [OptInJsonSerialization]
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
        [JsonIgnore]
        public Texture2D? Spritesheet { get; set; }

        /// <summary>
        /// Name of the Spritesheet of this. If left empty,
        /// the containing <see cref="UserInterface.Spritesheet"/> will be used instead.
        /// </summary>
        [JsonOptIn]
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
        public int Scale { get; protected set; } = 1;

        /// <summary>
        /// Dimensions of the UIComponent.
        /// </summary>
        public Size Dimensions { get; set; }

        /// <summary>
        /// Vertical offset of the UIElement from its Origin.
        /// </summary>
        public int VerticalOffset { get; set; }

        /// <summary>
        /// Horizontal offset of the UIElement from its Origin.
        /// </summary>
        public int HorizontalOffset { get; set; }

        /// <summary>
        /// Vertical allignment of the UIElement against its Origin.
        /// </summary>
        public Alignment VerticalAlign { get; set; }

        /// <summary>
        /// Horizontal allignment of the UIElement against its Origin.
        /// </summary>
        public Alignment HorizontalAlign { get; set; }

        /// <summary>
        /// Enables the drawing of the bounding box.
        /// </summary>
        [JsonOptIn]
        public bool ShowBorder { get; set; }

        /// <summary>
        /// List of actions subscribed to by this. Must be overriden in derived class.
        /// </summary>
        public virtual List<string> Actions { get => new(); }

        /// <summary>
        /// Layout settings of this.
        /// </summary>
        [JsonOptIn]
        public UILayout Layout { get; set; }

        /// <summary>
        /// Border width of the lines drawn by <see cref="ShowBorder"/>.
        /// </summary>
        protected static int borderWidth = 3;


        /// <summary>
        /// Creates a new UIComponent.
        /// Sets origin based on alignment conditions and initializes the bounding rectangle.
        /// </summary>
        /// <param name="Layout">Layout settings for the component.</param>
        [JsonConstructor]
        public UIComponent(UILayout Layout)
        {
            this.Layout = Layout;
            Scale = this.Layout.Scale > 0 ? this.Layout.Scale : 1;
            if (this.Layout.Dimensions != Size.Zero)
                Dimensions = this.Layout.Dimensions * Scale;
            VerticalAlign = this.Layout.VerticalAlign;
            HorizontalAlign = this.Layout.HorizontalAlign;
            VerticalOffset = this.Layout.VerticalOffset * Scale;
            HorizontalOffset = this.Layout.HorizontalOffset * Scale;
            Rect = new Rectangle(Origin, Dimensions);
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
        /// Preforms logic when the this' parent <see cref="Window"/> is closed.
        /// </summary>
        public virtual void OnClose() { }


        /// <summary>
        /// Preforms logic when this' parent <see cref="Window"/> is closed.
        /// </summary>
        public virtual void OnOpen() { }


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


        /// <summary>
        /// Returns whether the given point is inside this.
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public virtual bool Contains(Point p)
        {
            return Rect.Contains(p);
        }


        public static T ToNew<T>(T obj, ContentManager contentManager) where T : UIComponent
        {
            JsonSerializerOptions options = new JsonSerializerOptions()
            {
                AllowTrailingCommas = true,
                IncludeFields = true,
                Converters = { new JsonStringEnumConverter() },
                TypeInfoResolver = new DefaultJsonTypeInfoResolver()
                {
                    Modifiers =
                        {
                            JsonExtensions.AddNativePolymorphicTypeInfo<UIComponent>,
                            JsonExtensions.OptInSerialization<OptInJsonSerialization, JsonOptIn>
                        }
                }
            };
            T newObj = Json.Deserialize<T>(Json.Serialize(obj, options), options);
            newObj.LoadContent(contentManager);
            return newObj;
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
        public NullUIElement() : base(new UILayout(Size.Zero, Alignment.Top, Alignment.Left, 1, 0, 0))
        { }


        public override void Draw(SpriteBatch spriteBatch) { }


        public override void Update(InputState input) { }
    }
}
