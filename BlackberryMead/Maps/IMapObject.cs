using BlackberryMead.Utility;

namespace BlackberryMead.Maps
{
    /// <summary>
    /// An <see cref="INullImplementable{T}"/>, <see cref="IDrawable{T}"/> where T is <see cref="MapDrawContext"/>, 
    /// <see cref="ISpanning"/> object
    /// used in a <see cref="Map2D{T}"/>.
    /// </summary>
    /// <typeparam name="T">Type of the inherited class.</typeparam>
    /// <remarks>
    /// To inherit from <see cref="IMapObject{T}"/>, use the type of the inheriting class in place of T.
    /// </remarks>
    /// <example>
    /// public class Foo : IMapObject(Foo)
    /// </example>
    public interface IMapObject<T> : INullImplementable<T>, IDrawable<MapDrawContext>, ISpanning
    {
    }
}
