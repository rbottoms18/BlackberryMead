using System;

namespace BlackberryMead.Input.Typography
{
    public abstract class CharEffect
    {
        /// <summary>
        /// Applies the effect to a <see cref="Char"/>.
        /// </summary>
        public abstract Action<Char> Apply { get; }


        /// <summary>
        /// Creates a new CharEffect.
        /// </summary>
        /// <param name="applyMethod"></param>
        public CharEffect() { }
    }
}
