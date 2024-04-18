namespace BlackberryMead.Framework
{
    /// <summary>
    /// Defines a method that allows an object to be drawn to the screen.
    /// </summary>
    /// <typeparam name="T">Type of <see cref="IDrawContext"/> the inheriting type uses.</typeparam>
    public interface IDrawable<T> where T : IDrawContext
    {
        /// <summary>
        /// Draws the <see cref="IDrawable{T}"/> to the screen.
        /// </summary>
        /// <param name="context"><see cref="IDrawContext"/> used to draw the <see cref="IDrawable{T}"/>.</param>
        abstract void Draw(T context);
    }
}
