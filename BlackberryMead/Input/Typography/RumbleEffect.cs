using BlackberryMead.Framework;
using System;

namespace BlackberryMead.Input.Typography
{
    /// <summary>
    /// <see cref="CharEffect"/> that causes the <see cref="Char"/> to shake, or "rumble".
    /// </summary>
    internal class RumbleEffect : CharEffect
    {
        /// <summary>
        /// Random number generator for random offsets.
        /// </summary>
        private Random rng;

        /// <summary>
        /// Amplitude of the offset.
        /// </summary>
        private int amplitude;


        /// <summary>
        /// Create a new <see cref="RumbleEffect"/>.
        /// </summary>
        /// <param name="amplitude">Amplitude in pixel of the offset of the <see cref="Char"/> 
        /// from its position.</param>
        public RumbleEffect(int amplitude)
        {
            rng = new Random();
            this.amplitude = amplitude;
        }


        public override Action<Char> Apply => (c) =>
        {
            c.Offset = new Size(amplitude * (rng.Next(0, 3) - 1), amplitude * (rng.Next(0, 3) - 1));
        };
    }
}
