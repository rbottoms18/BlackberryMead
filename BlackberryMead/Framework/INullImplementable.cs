namespace BlackberryMead.Utility
{
    /// <summary>
    /// Marks an object as having a Null Object Pattern implementation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface INullImplementable<T>
    {
        /// <summary>
        /// Returns true if the object is null, false if not.
        /// </summary>
        abstract bool IsNull();

        /// <summary>
        /// Null object implementation of type <typeparamref name="T"/>
        /// </summary>
        public static abstract T Null { get; }
    }
}
