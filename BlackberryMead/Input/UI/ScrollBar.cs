using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Size = BlackberryMead.Utility.Size;

namespace BlackberryMead.Input.UI
{
    /// <summary>
    /// A UIElement with a dragable component on a track that determines a value between 0 and 1.
    /// </summary>
    public class ScrollBar : UIComponent
    {
        /// <summary>
        /// Value of the scroll bar constrained between 0 and 1.
        /// </summary>
        public float Value { get; protected set; }

        /// <summary>
        /// Number of decimal places Value will be rounded to.
        /// </summary>
        [JsonInclude]
        public int Precision { get; protected set; }

        /// <summary>
        /// Default Value of this.
        /// </summary>
        [JsonInclude]
        public float DefaultValue { get; protected set; }

        /// <summary>
        /// Dimensions of the track.
        /// </summary>
        [JsonInclude]
        public Size TrackDimensions { get; protected set; }

        /// <summary>
        /// Offset of the track from the Origin of this.
        /// </summary>
        [JsonInclude]
        public Point TrackOffset { get; protected set; }

        /// <summary>
        /// Size of the scroll knob.
        /// </summary>
        [JsonInclude]
        public Size KnobSize { get; protected set; }

        /// <summary>
        /// Possible directions for scrolling of the knob.
        /// </summary>
        public enum ScrollDirections
        {
            Vertical,
            Horizontal
        }

        /// <summary>
        /// 
        /// </summary>
        public float ScrollWheelFactor { get; set; } = 1f;

        /// <summary>
        /// Direction that the knob can be dragged along.
        /// </summary>
        [JsonInclude]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ScrollDirections ScrollDirection { get; protected set; }

        /// <summary>
        /// The name of the action that enables dragging of the scroll bar.
        /// </summary>
        public string ScrollActionName { get; set; } = "Drag";

        public override List<string> Actions => new() { ScrollActionName };

        /// <summary>
        /// Track that the scroll knob can be dragged on.
        /// </summary>
        protected Rectangle track;

        /// <summary>
        /// Length of the track.
        /// </summary>
        // Separate field to avoid having to recalculate it at every call
        protected int trackLength;

        /// <summary>
        /// Knob that can be dragged to change the value
        /// </summary>
        protected Rectangle knob;

        /// <summary>
        /// Whether the knob is currently being dragged or not
        /// </summary>
        protected bool isDragging;

        /// <summary>
        /// Mouse position of the previous tick.
        /// </summary>
        // For calculating mouse delta
        protected Point prevMousePos;


        /// <summary>
        /// Create a new ScrollBar.
        /// </summary>
        /// <param name="KnobSize"></param>
        /// <param name="ScrollDirection">Direction in which the scroll bar scrolls. </param>
        /// <param name="Precision">Number of decimal precision of <paramref name="Value"></paramref>.</param>
        /// <param name="DefaultValue">Default value of <paramref name="Value"></paramref>.</param>
        /// <inheritdoc/>
        public ScrollBar(Size TrackDimensions, Point TrackOffset, Size KnobSize, ScrollDirections ScrollDirection,
            UILayout Layout, int Precision = 2, float DefaultValue = 0, string ScrollActionName = "Drag") :
            base(Layout)
        {
            this.track = new Rectangle(Origin + TrackOffset, TrackDimensions);
            this.TrackOffset = TrackOffset;
            this.TrackDimensions = TrackDimensions * Layout.Scale;
            this.KnobSize = KnobSize * Layout.Scale;
            this.ScrollDirection = ScrollDirection;
            this.Precision = Precision;
            this.DefaultValue = DefaultValue;
            this.ScrollActionName = ScrollActionName;

            // Set track length
            if (ScrollDirection == ScrollDirections.Vertical)
            {
                trackLength = track.Bottom - track.Top - KnobSize.Height;
            }
            else if (ScrollDirection == ScrollDirections.Horizontal)
            {
                trackLength = track.Right - track.Left - KnobSize.Width;
            }

            // Initialize knob
            knob = new Rectangle(track.X, track.Y, KnobSize.Width, KnobSize.Height);
        }


        public override void Update(InputState input)
        {
            // If knob is clicked and mouse is down, drag
            if (knob.Contains(input.MousePosition) && input[ScrollActionName])
                isDragging = true;
            // Only stop dragging if the mouse is released, regardless of whether the mouse
            // left the knob or not
            else if (!input[ScrollActionName])
            {
                isDragging = false;
            }

            if (isDragging)
            {
                Point delta = Point.Zero;

                if (ScrollDirection == ScrollDirections.Vertical)
                {
                    if (knob.Top >= track.Top && knob.Bottom <= track.Bottom)
                        delta.Y = (input.MousePosition.Y - prevMousePos.Y);
                }
                else if (ScrollDirection == ScrollDirections.Horizontal)
                {
                    if (knob.Left >= track.Left && knob.Right <= track.Right)
                        delta.X = (input.MousePosition.X - prevMousePos.X);
                }
                knob.Location += delta;
            }
            else if (input.ScrollWheelDelta != 0 && !isDragging)
            {
                knob.Location += new Point(0, (int)(-1 * Math.Sign(input.ScrollWheelDelta) * ScrollWheelFactor));
            }

            // Clamp the knob to the track
            knob.Location = new Point(Math.Clamp(knob.Location.X, track.Left, track.Right - knob.Width),
                Math.Clamp(knob.Location.Y, track.Top, track.Bottom - knob.Height));

            // Update value
            Value = (ScrollDirection == ScrollDirections.Vertical) ?
                (float)Math.Round((knob.Top - track.Top) / (trackLength * 1.0), Precision) :
                (float)Math.Round((knob.Left - track.Left) / (trackLength * 1.0), Precision);

            // Debug.Print(Value + "");

            // !Must be last to update!
            prevMousePos = input.MousePosition;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            // Draw track
            spriteBatch.FillRectangle(track, Color.Red, 0);

            // Draw knob
            spriteBatch.FillRectangle(knob, Color.Green, 0);
        }


        public override void Realign(Rectangle boundingRect)
        {
            base.Realign(boundingRect);

            // Reallign based on new Origin
            this.track = new Rectangle(Origin + TrackOffset, TrackDimensions);

            // Rescale knob
            knob = new Rectangle(track.X, track.Y, KnobSize.Width, KnobSize.Height);

            // Reset knob position based on current value!
        }
    }
}
