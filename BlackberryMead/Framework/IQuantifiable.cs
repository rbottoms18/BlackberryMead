namespace BlackberryMead.Framework
{
    /// <summary>
    /// Defines a property that assigns a numerical count to an object.
    /// </summary>
    public interface IQuantifiable
    {
        /// <summary>
        /// Value this can be quantified as.
        /// </summary>
        public abstract int Count { get; }
    }
}
