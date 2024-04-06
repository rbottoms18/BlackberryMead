using BlackberryMead.Framework;
using System;

namespace BlackberryMead.Input.Typography
{
    // Example of what an effect could do to a Char.
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
