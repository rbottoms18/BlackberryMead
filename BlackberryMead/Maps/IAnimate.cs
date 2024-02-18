using BlackberryMead.Utility;
using Microsoft.Xna.Framework;

namespace BlackberryMead.Maps
{
    /// <summary>
    /// An Interface that marks the object as one that moves about the map ("animate").
    /// Derives from IUpdatable and ICollidable
    /// </summary>
    public interface IAnimate : ICollidable, IDrawable<MapDrawContext>, IUpdatable
    {
        /// <summary>
        /// Velocity of the object.
        /// </summary>
        public Vector2 Velocity { get; set; }

        /// <summary>
        /// Hitboxes of the object.
        /// </summary>
        public Rectangle Hitbox { get; }

        /// <summary>
        /// Position of the object.
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Whether this is touching the left side of <paramref name="collisionObjHitbox"/>.
        /// </summary>
        /// <param name="velocity"></param>
        /// <param name="collisionObjHitbox"></param>
        /// <returns></returns>
        public bool IsTouchingLeft(Vector2 velocity, Rectangle collisionObjHitbox)
        {
            return Hitbox.Right - 1 + velocity.X >= collisionObjHitbox.Left &&
              Hitbox.Right <= collisionObjHitbox.Left &&
              Hitbox.Bottom - 1 > collisionObjHitbox.Top &&
              Hitbox.Top < collisionObjHitbox.Bottom - 1;
        }

        /// <summary>
        /// Whether this is touching the right side of <paramref name="collisionObjHitbox"/>.
        /// </summary>
        /// <param name="velocity"></param>
        /// <param name="collisionObjHitbox"></param>
        /// <returns></returns>
        public bool IsTouchingRight(Vector2 velocity, Rectangle collisionObjHitbox)
        {
            return Hitbox.Left + velocity.X <= collisionObjHitbox.Right - 1 &&
              Hitbox.Left >= collisionObjHitbox.Right - 1 &&
              Hitbox.Bottom - 1 > collisionObjHitbox.Top &&
              Hitbox.Top < collisionObjHitbox.Bottom - 1;
        }

        /// <summary>
        /// Whether this is touching the top side of <paramref name="collisionObjHitbox"/>.
        /// </summary>
        /// <param name="velocity"></param>
        /// <param name="collisionObjHitbox"></param>
        /// <returns></returns>
        public bool IsTouchingTop(Vector2 velocity, Rectangle collisionObjHitbox)
        {
            return Hitbox.Bottom - 1 <= collisionObjHitbox.Bottom - 1 && // if Bottom above Top
              Hitbox.Bottom - 1 + velocity.Y >= collisionObjHitbox.Top && // But Bottom + velocity below Top
              Hitbox.Right - 1 > collisionObjHitbox.Left &&
              Hitbox.Left < collisionObjHitbox.Right - 1;
        }

        /// <summary>
        /// Whether this is touching the bottom side of <paramref name="collisionObjHitbox"/>.
        /// </summary>
        /// <param name="velocity"></param>
        /// <param name="collisionObjHitbox"></param>
        /// <returns></returns>
        public bool IsTouchingBottom(Vector2 velocity, Rectangle collisionObjHitbox)
        {
            return Hitbox.Top + velocity.Y <= collisionObjHitbox.Bottom - 1 &&
              Hitbox.Top >= collisionObjHitbox.Bottom - 1 &&
              Hitbox.Right - 1 > collisionObjHitbox.Left &&
              Hitbox.Left < collisionObjHitbox.Right - 1;
        }


        /// <summary>
        /// Move the object around the map
        /// </summary>
        public virtual void Move()
        {
            Position += Velocity;
        }
    }
}
