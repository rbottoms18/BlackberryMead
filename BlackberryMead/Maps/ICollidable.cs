using System;
using System.Collections.Generic;

namespace BlackberryMead.Maps
{
    /// <summary>
    /// Defines methods to enable an object as collidable on a <see cref="Map2D{T}"/>.
    /// </summary>
    public interface ICollidable
    {
        /// <summary>
        /// <see cref="Dictionary{TKey, TValue}"/> of actions to take on collision based on the type of object being collided with.
        /// Must be instantiated.
        /// </summary>
        public Dictionary<Type, Action> CollisionDict { get; }


        /// <summary>
        /// Method ran when another <see cref="ICollidable"/> object collides with the <see cref="ICollidable"/>.
        /// </summary>
        /// <param name="collidingObj"></param>
        public void Collide(ICollidable collidingObj)
        {
            if (!CollisionDict.TryGetValue(collidingObj.GetType(), out Action collideFunction))
                return;
            collideFunction.Invoke();
        }
    }
}
