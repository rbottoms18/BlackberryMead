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
    /// Object that represents an element of a <see cref="UserInterface"/>.
    /// </summary>
    [OptInJsonSerialization]
    public abstract class UIComponent
    {
        /// <summary>
        /// An empty <see cref="UIComponent"/>.
        /// </summary>
        public static NullUIElement Null = new();

        /// <summary>
        /// The opacity that the <see cref="UIComponent"/> is drawn with when it is disabled..
        /// </summary>
        public const float DisabledOpacity = 0.5f;

        /// <summary>
        /// Spritesheet of the <see cref="UIComponent"/>.
        /// </summary>
        [JsonIgnore]
        public Texture2D? Spritesheet { get; set; }

        /// <summary>
        /// Name of the Spritesheet of the <see cref="UIComponent"/>. 
        /// </summary>
        /// <remarks>If left empty,
        /// the containing <see cref="UserInterface.Spritesheet"/> will be used instead.
        /// </remarks>
        [JsonOptIn]
        public string SpritesheetName { get; set; } = "";

        /// <summary>
        /// Rectangle that the <see cref="UIComponent"/> is encapsulated in and drawn to.
        /// </summary>
        public virtual Rectangle Rect { get; protected set; }

        /// <summary>
        /// Origin of the <see cref="UIComponent"/> insides its parent collection's Rectangle.
        /// </summary>
        public virtual Point Origin { get; protected set; }

        /// <summary>
        /// Whether the <see cref="UIComponent"/> is currently visible or not.
        /// </summary>
        public bool IsVisible = true;

        /// <summary>
        /// Whether the <see cref="UIComponent"/> can be interacted with.
        /// </summary>
        public bool Enabled = true;

        /// <summary>
        /// The draw opacity the <see cref="UIComponent"/> is drawn with.
        /// </summary>
        public float Opacity = 1f;

        /// <summary>
        /// Factor to which the <see cref="UIComponent"/> is scaled in the <see cref="UserInterface"/>.
        /// </summary>
        /// <remarks>
        /// Affects dimensions but does not affect horizontal and vertical offsets.
        /// </remarks>
        public int Scale { get; protected set; } = 1;

        /// <summary>
        /// Dimensions of the <see cref="UIComponent"/>.
        /// </summary>
        public Size Dimensions { get; set; }

        /// <summary>
        /// Vertical offset of the <see cref="UIComponent"/> from its <see cref="Origin"/>.
        /// </summary>
        public int VerticalOffset { get; set; }

        /// <summary>
        /// Horizontal offset of the <see cref="UIComponent"/> from its <see cref="Origin"/>.
        /// </summary>
        public int HorizontalOffset { get; set; }

        /// <summary>
        /// Vertical allignment of the <see cref="UIComponent"/> against its <see cref="Origin"/>.
        /// </summary>
        public Alignment VerticalAlign { get; set; }

        /// <summary>
        /// Horizontal allignment of the <see cref="UIComponent"/> against its <see cref="Origin"/>.
        /// </summary>
        public Alignment HorizontalAlign { get; set; }

        /// <summary>
        /// If true, draws the bounding box in <see cref="Draw(SpriteBatch)"/>.
        /// </summary>
        [JsonOptIn]
        public bool ShowBorder { get; set; }

        /// <summary>
        /// List of actions subscribed to by the <see cref="UIComponent"/>.
        /// </summary>
        /// <remarks>
        /// Must be overriden in derived class.
        /// </remarks>
        public virtual List<string> Actions { get => new(); }

        /// <summary>
        /// Layout settings of the <see cref="UIComponent"/>.
        /// </summary>
        [JsonOptIn]
        public UILayout Layout { get; set; }

        /// <summary>
        /// Border width of the lines drawn by <see cref="ShowBorder"/>.
        /// </summary>
        protected static int borderWidth = 3;


        /// <summary>
        /// Creates a new <see cref="UIComponent"/>.
        /// Sets origin based on alignment conditions and initializes the bounding rectangle.
        /// </summary>
        /// <param name="Layout">Layout settings for the <see cref="UIComponent"/>.</param>
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
        /// Aligns the origin of the <see cref="UIComponent"/> inside a rectangle.
        /// </summary>
        /// <param name="boundingRect">Rectangle to align against.</param>
        /// <param name="vAllign">Vertical alignment.</param>
        /// <param name="hAllign">Horizontal alignment.</param>
        /// <param name="dim">Dimensions of the <see cref="UIComponent"/> rectangle.</param>
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
        /// Realign the <see cref="UIComponent"/> inside a rectangle.
        /// </summary>
        public virtual void Realign(Rectangle boundingRect)
        {
            // Set origin based on alligments
            Origin = Align(boundingRect, Dimensions, this.VerticalAlign, this.HorizontalAlign, this.VerticalOffset,
                this.HorizontalOffset);

            Rect = new Rectangle(Origin, Dimensions);
        }


        /// <summary>
        /// Preforms update logic for the <see cref="UIComponent"/>.
        /// </summary>
        public abstract void Update(InputState input);


        /// <summary>
        /// Draws the <see cref="UIComponent"/>.
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
        /// Preforms logic when the <see cref="UIComponent"/>'s parent <see cref="Window"/> is closed.
        /// </summary>
        public virtual void OnClose() { }


        /// <summary>
        /// Preforms logic when teh <see cref="UIComponent"/>'s parent <see cref="Window"/> is opened.
        /// </summary>
        public virtual void OnOpen() { }


        /// <summary>
        /// Gets a list of all <see cref="UIComponent"/> children contained in this.
        /// </summary>
        /// <remarks>
        /// Does not include itself as a child.
        /// </remarks>
        /// <returns>List of children UIElements contained in this UIElement.
        /// Must be overridden if the Component contains subcomponents.</returns>
        public virtual Dictionary<string, UIComponent> GetChildren()
        {
            return new Dictionary<string, UIComponent> { };
        }


        /// <summary>
        /// Disables the <see cref="UIComponent"/> and makes it uninteractable.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Disable(object sender, EventArgs e)
        {
            Enabled = false;
            Opacity = DisabledOpacity;
        }

        /// <summary>
        /// Enables the <see cref="UIComponent"/> and makes it interactable.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Enable(object sender, EventArgs e)
        {
            Enabled = true;
            Opacity = 1f;
        }


        /// <summary>
        /// Loads content required by the <see cref="UIComponent"/>.
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
        /// Returns whether the given point is inside the <see cref="UIComponent"/>.
        /// </summary>
        /// <param name="p">Point to evaluate.</param>
        /// <returns>Whether <paramref name="p"/> is contained inside the <see cref="UIComponent"/>.</returns>
        public virtual bool Contains(Point p)
        {
            return Rect.Contains(p);
        }


        /// <summary>
        /// Creates a deep copy of the <see cref="UIComponent"/>.
        /// </summary>
        /// <remarks>
        /// Serializes the <see cref="UIComponent"/> to Json and deserializes back to a new object.
        /// </remarks>
        /// <typeparam name="T">Type of <see cref="UIComponent"/> to copy.</typeparam>
        /// <param name="obj"><see cref="UIComponent"/> to copy.</param>
        /// <param name="contentManager"><see cref="ContentManager"/> required to call 
        /// <see cref="LoadContent(ContentManager)"/> on the copy.</param>
        /// <returns>A deep copy of <paramref name="obj"/>.</returns>
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
}
