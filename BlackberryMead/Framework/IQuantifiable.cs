namespace BlackberryMead.Framework
{
    /// <summary>
    /// Defines a method that assigns a numerical count to a type.
    /// </summary>
    public interface IQuantifiable
    {
        /// <summary>
        /// Value this can be quantified as.
        /// </summary>
        public abstract int Count { get; }
    }
}
