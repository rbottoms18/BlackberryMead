using System;

namespace BlackberryMead.Input.Typography
{
    /// <summary>
    /// A visual effect that can be applied to a <see cref="Char"/>.
    /// </summary>
    public abstract class CharEffect
    {
        /// <summary>
        /// Applies the effect to a <see cref="Char"/>.
        /// </summary>
        public abstract Action<Char> Apply { get; }


        /// <summary>
        /// Creates a new <see cref="CharEffect"/>.
        /// </summary>
        public CharEffect() { }
    }
}
