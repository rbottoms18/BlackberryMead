using System;
using System.Drawing;
using System.Text.Json.Serialization;

namespace BlackberryMead.Framework
{
    /// <summary>
    /// Class that represents a two dimensional size with a Width and Height.
    /// </summary>
    public struct Size : IEquatable<Size>
    {
        /// <summary>
        /// A Size with <see cref="Width"/> and <see cref="Height"/> 0.
        /// </summary>
        public static Size Zero => new Size(0, 0);

        [JsonInclude]
        public int Width { get; set; }

        [JsonInclude]
        public int Height { get; set; }


        [JsonConstructor]
        public Size(int Width, int Height)
        {
            this.Width = Width;
            this.Height = Height;
            Point p = new Point(Width, Height);
        }


        public Size(MonoGame.Extended.Size size)
        {
            Width = size.Width;
            Height = size.Height;
        }


        // +
        public static Size operator +(Size a, Size b) =>
            new Size(a.Width + b.Width, a.Height + b.Height);

        public static Size operator +(Size a, Point b) =>
            new Size(a.Width + b.X, a.Height + b.Y);

        public static Size operator +(Size s, int x) =>
            new Size(s.Width + x, s.Height + x);


        // -

        public static Size operator -(Size a, Size b) =>
            new Size(a.Width - b.Width, a.Height - b.Height);

        public static Size operator -(Size a, Point b) =>
            new Size(a.Width - b.X, a.Height - b.Y);

        public static Size operator -(Size s, int x) =>
            new Size(s.Width - x, s.Height - x);


        // *

        public static Size operator *(Size s, float x) =>
            new Size((int)(s.Width * x), (int)(s.Height * x));


        // /

        public static Size operator /(Size s, float x) =>
            new Size((int)(s.Width / x), (int)(s.Height / x));

        // ==

        public bool Equals(Size other)
        {
            return other.Width == Width && other.Height == Height;
        }

        public override bool Equals(object obj)
        {
            return obj is Size && Equals((Size)obj);
        }

        public override int GetHashCode()
        {
            return Width.GetHashCode() * 397 ^ Height.GetHashCode();
        }

        public static bool operator ==(Size a, Size b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Size a, Size b)
        {
            return !(a == b);
        }

        // Implicit

        public static implicit operator Microsoft.Xna.Framework.Point(Size s)
        {
            return new Microsoft.Xna.Framework.Point(s.Width, s.Height);
        }


        public static implicit operator MonoGame.Extended.Size(Size s)
        {
            return new MonoGame.Extended.Size(s.Width, s.Height);
        }
    }
}
