using Microsoft.Xna.Framework;

namespace BlackberryMead.Maps
{
    /// <summary>
    /// Defines a method to allow an object to be updated each in-game tick.
    /// </summary>
    public interface IUpdatable
    {
        /// <summary>
        /// IUpdatable method that is called every in-game tick to update the object
        /// </summary>
        /// <param name="gameTime">GameTime to keep track of the elapsed time since the previous update.</param>
        /// <param name="map">Map the IUpdatable is in.</param>
        /// <param name="position">Position of the IUpdatable in the map.</param>
        public void Update<T>(GameTime gameTime, Map2D<T> map, GridPoint position) where T : IMapObject<T>;
    }
}
