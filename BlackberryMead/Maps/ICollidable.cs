using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace BlackberryMead.Maps
{
    /// <summary>
    /// Marks the object as being collidable on a <see cref="Map2D{T}"/>.
    /// </summary>
    public interface ICollidable
    {
        /// <summary>
        /// Dictionary of actions to take on collision based on the type of object being collided with.
        /// Must be instantiated.
        /// </summary>
        public Dictionary<Type, Action> CollisionDict { get; }


        /// <summary>
        /// Method ran when a something collides with the object.
        /// </summary>
        /// <param name="collidingObj"></param>
        public void Collide(ICollidable collidingObj)
        {
            if (!CollisionDict.TryGetValue(collidingObj.GetType(), out Action collideFunction))
                return;
            collideFunction.Invoke();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool Intersects(List<Rectangle> hitboxes)
        {
            return false;
        }
    }
}
