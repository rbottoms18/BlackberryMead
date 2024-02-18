namespace BlackberryMead.Utility
{
    /// <summary>
    /// Marks an object as having a Null Object Pattern implimentation.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface INullImplimentable<T>
    {
        /// <summary>
        /// Returns true if the object is null, false if not.
        /// </summary>
        abstract bool IsNull();

        /// <summary>
        /// Gets the Null Object of type <typeparamref name="T"/>
        /// </summary>
        static abstract T GetNull();
    }
}
