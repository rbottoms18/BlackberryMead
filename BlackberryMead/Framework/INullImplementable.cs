﻿namespace BlackberryMead.Framework
{
    /// <summary>
    /// Defines a Null-Object Pattern implementation for objects of type <typeparamref name="T"/>.
    /// </summary>
    public interface INullImplementable<T>
    {
        /// <summary>
        /// Null-Object implementation of type <typeparamref name="T"/>.
        /// </summary>
        public static abstract T Null { get; }


        /// <summary>
        /// Returns <see langword="true"/> if the object is null; otherwise, <see langword="false"/>.
        /// </summary>
        public abstract bool IsNull { get; }
    }
}
