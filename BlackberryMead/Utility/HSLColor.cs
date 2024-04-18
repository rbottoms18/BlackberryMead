using System.Drawing;

namespace BlackberryMead.Utility
{
    /// <summary>
    /// Represents a color in HSL values.
    /// </summary>
    /// <remarks>
    /// Credit Rich Newman.
    /// https://richnewman.wordpress.com/about/code-listings-and-diagrams/hslcolor-class/
    /// </remarks>
    public class HSLColor
    {
        // Private data members below are on scale 0-1
        // They are scaled for use externally based on scale

        private double hue = 1.0;

        private double saturation = 1.0;

        private double luminosity = 1.0;


        /// <summary>
        /// Preset scale value (default was 240??)
        /// </summary>
        //private const double scale = 100.0;

        private const double hueScale = 360.0;

        private const double satValScale = 100.0;


        /// <summary>
        /// Hue field of the <see cref="HSLColor"/>.
        /// </summary>
        public double Hue
        {
            get { return hue * hueScale; }
            set { hue = Clamp(value / hueScale); }
        }


        /// <summary>
        /// Saturation field of the <see cref="HSLColor"/>.
        /// </summary>
        public double Saturation
        {
            get { return saturation * satValScale; }
            set { saturation = Clamp(value / satValScale); }
        }


        /// <summary>
        /// Luminosity field of the <see cref="HSLColor"/>.
        /// </summary>
        public double Luminosity
        {
            get { return luminosity * satValScale; }
            set { luminosity = Clamp(value / satValScale); }
        }


        /// <summary>
        /// Clamps the value between 0 and 1.
        /// </summary>
        /// <param name="value">Value to clamp.</param>
        private double Clamp(double value)
        {
            if (value < 0.0)
                value = 0.0;
            else if (value > 1.0)
                value = 1.0;
            return value;
        }


        public void SetRGB(int red, int green, int blue)
        {
            HSLColor hslColor = (HSLColor)Color.FromArgb(red, green, blue);
            hue = hslColor.hue;
            saturation = hslColor.saturation;
            luminosity = hslColor.luminosity;
        }


        /// <summary>
        /// Create an empty <see cref="HSLColor"/>.
        /// </summary>
        public HSLColor() { }


        /// <summary>
        /// Create a new <see cref="HSLColor"/> from an existing color.
        /// </summary>
        /// <param name="color"></param>
        public HSLColor(Color color)
        {
            SetRGB(color.R, color.G, color.B);
        }


        /// <summary>
        /// Create a new <see cref="HSLColor"/> from RGB.
        /// </summary>
        /// <param name="red"></param>
        /// <param name="green"></param>
        /// <param name="blue"></param>
        public HSLColor(int red, int green, int blue)
        {
            SetRGB(red, green, blue);
        }


        /// <summary>
        /// Create a new <see cref="HSLColor"/> from hue, saturation, and luminosity parameters.
        /// </summary>
        /// <param name="hue"></param>
        /// <param name="saturation"></param>
        /// <param name="luminosity"></param>
        public HSLColor(double hue, double saturation, double luminosity)
        {
            Hue = hue;
            Saturation = saturation;
            Luminosity = luminosity;

        }


        /// <summary>
        /// Create a new <see cref="HSLColor"/> from an RGB array.
        /// </summary>
        /// <param name="rgb"></param>
        public HSLColor(int[] rgb)
        {
            if (rgb.Length >= 3)
                SetRGB(rgb[0], rgb[1], rgb[2]);
        }


        #region Casts to/from System.Drawing.Color

        /// <summary>
        /// Operator that casts from HSLColor to System.Drawing.Color.
        /// </summary>
        /// <param name="hslColor"></param>
        public static implicit operator Color(HSLColor hslColor)
        {
            double r = 0, g = 0, b = 0;
            if (hslColor.luminosity != 0)
            {
                if (hslColor.saturation == 0)
                    r = g = b = hslColor.luminosity;
                else
                {
                    double temp2 = GetTemp2(hslColor);
                    double temp1 = 2.0 * hslColor.luminosity - temp2;

                    r = GetColorComponent(temp1, temp2, hslColor.hue + 1.0 / 3.0);
                    g = GetColorComponent(temp1, temp2, hslColor.hue);
                    b = GetColorComponent(temp1, temp2, hslColor.hue - 1.0 / 3.0);
                }
            }
            return Color.FromArgb((int)(255 * r), (int)(255 * g), (int)(255 * b));
        }

        /// <summary>
        /// Operator that casts from <see cref="HSLColor"/> to <see cref="Microsoft.Xna.Framework.Color"/>.
        /// </summary>
        /// <param name="hslColor"></param>
        public static implicit operator Microsoft.Xna.Framework.Color(HSLColor hslColor)
        {
            Color color = (Color)hslColor;
            return new Microsoft.Xna.Framework.Color(color.R, color.G, color.B);
        }


        private static double GetColorComponent(double temp1, double temp2, double temp3)
        {
            temp3 = MoveIntoRange(temp3);
            if (temp3 < 1.0 / 6.0)
                return temp1 + (temp2 - temp1) * 6.0 * temp3;
            else if (temp3 < 0.5)
                return temp2;
            else if (temp3 < 2.0 / 3.0)
                return temp1 + (temp2 - temp1) * (2.0 / 3.0 - temp3) * 6.0;
            else
                return temp1;
        }


        private static double MoveIntoRange(double temp3)
        {
            if (temp3 < 0.0)
                temp3 += 1.0;
            else if (temp3 > 1.0)
                temp3 -= 1.0;
            return temp3;
        }


        private static double GetTemp2(HSLColor hslColor)
        {
            double temp2;
            if (hslColor.luminosity < 0.5)  //<=??
                temp2 = hslColor.luminosity * (1.0 + hslColor.saturation);
            else
                temp2 = hslColor.luminosity + hslColor.saturation - hslColor.luminosity * hslColor.saturation;
            return temp2;
        }


        public static implicit operator HSLColor(Color color)
        {
            HSLColor hslColor = new HSLColor();
            hslColor.hue = color.GetHue() / 360.0; // we store hue as 0-1 as opposed to 0-360 
            hslColor.luminosity = color.GetBrightness();
            hslColor.saturation = color.GetSaturation();
            return hslColor;
        }

        #endregion
    }
}
