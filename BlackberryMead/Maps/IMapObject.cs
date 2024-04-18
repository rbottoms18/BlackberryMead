using BlackberryMead.Framework;

namespace BlackberryMead.Maps
{
    /// <summary>
    /// An <see cref="INullImplementable{T}"/>, <see cref="IDrawable{T}"/> where T is <see cref="MapDrawContext"/>, 
    /// <see cref="ISpanning"/> object
    /// used in a <see cref="Map2D{T}"/>.
    /// </summary>
    /// <typeparam name="T">Type of the inherited class.</typeparam>
    /// <remarks>
    /// Encapsulates multiple types required by objects in order to be constructed into a <see cref="Map2D{T}"/>.
    /// </remarks>
    public interface IMapObject<T> : INullImplementable<T>, IDrawable<MapDrawContext>, ISpanning { }
}
