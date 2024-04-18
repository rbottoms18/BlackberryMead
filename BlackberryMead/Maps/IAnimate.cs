using BlackberryMead.Framework;
using Microsoft.Xna.Framework;

namespace BlackberryMead.Maps
{
    /// <summary>
    /// Defines methods for an object to through about a <see cref="Map2D{T}"/>.
    /// </summary>
    public interface IAnimate : ICollidable, IDrawable<MapDrawContext>, IUpdatable
    {
        /// <summary>
        /// Velocity of the <see cref="IAnimate"/>.
        /// </summary>
        public Vector2 Velocity { get; set; }

        /// <summary>
        /// Hitboxes of the <see cref="IAnimate"/>.
        /// </summary>
        public Rectangle Hitbox { get; }

        /// <summary>
        /// Position of the <see cref="IAnimate"/>.
        /// </summary>
        public Vector2 Position { get; set; }


        /// <summary>
        /// Determines whether the <see cref="IAnimate"/> will collide with the left of a <see cref="Rectangle"/> if a 
        /// given velocity is applied to it. 
        /// </summary>
        /// <param name="velocity">Potential velocity to be applied to the <see cref="IAnimate"/>.</param>
        /// <param name="Rect"><see cref="Rectangle"/> to evaluate collision with.</param>
        /// <returns><see langword="true"/> if the <see cref="IAnimate"/> collides with <paramref name="Rect"/> if
        /// <paramref name="velocity"/> is applied to it.</returns>
        public bool IsTouchingLeft(Vector2 velocity, Rectangle Rect)
        {
            return Hitbox.Right - 1 + velocity.X >= Rect.Left &&
              Hitbox.Right <= Rect.Left &&
              Hitbox.Bottom - 1 > Rect.Top &&
              Hitbox.Top < Rect.Bottom - 1;
        }

        /// <summary>
        /// Determines whether the <see cref="IAnimate"/> will collide with the right of a <see cref="Rectangle"/> if a 
        /// given velocity is applied to it. 
        /// </summary>
        /// <inheritdoc cref="IsTouchingLeft(Vector2, Rectangle)"/>
        public bool IsTouchingRight(Vector2 velocity, Rectangle collisionObjHitbox)
        {
            return Hitbox.Left + velocity.X <= collisionObjHitbox.Right - 1 &&
              Hitbox.Left >= collisionObjHitbox.Right - 1 &&
              Hitbox.Bottom - 1 > collisionObjHitbox.Top &&
              Hitbox.Top < collisionObjHitbox.Bottom - 1;
        }

        /// <summary>
        /// Determines whether the <see cref="IAnimate"/> will collide with the top of a <see cref="Rectangle"/> if a 
        /// given velocity is applied to it. 
        /// </summary>
        /// <inheritdoc cref="IsTouchingLeft(Vector2, Rectangle)"/>
        public bool IsTouchingTop(Vector2 velocity, Rectangle collisionObjHitbox)
        {
            return Hitbox.Bottom - 1 <= collisionObjHitbox.Bottom - 1 && // if Bottom above Top
              Hitbox.Bottom - 1 + velocity.Y >= collisionObjHitbox.Top && // But Bottom + velocity below Top
              Hitbox.Right - 1 > collisionObjHitbox.Left &&
              Hitbox.Left < collisionObjHitbox.Right - 1;
        }

        /// <summary>
        /// Determines whether the <see cref="IAnimate"/> will collide with the bottom of a <see cref="Rectangle"/> if a 
        /// given velocity is applied to it. 
        /// </summary>
        /// <inheritdoc cref="IsTouchingLeft(Vector2, Rectangle)"/>
        public bool IsTouchingBottom(Vector2 velocity, Rectangle collisionObjHitbox)
        {
            return Hitbox.Top + velocity.Y <= collisionObjHitbox.Bottom - 1 &&
              Hitbox.Top >= collisionObjHitbox.Bottom - 1 &&
              Hitbox.Right - 1 > collisionObjHitbox.Left &&
              Hitbox.Left < collisionObjHitbox.Right - 1;
        }


        /// <summary>
        /// Move the <see cref="IAnimate"/> in the <see cref="Map2D{T}"/>.
        /// </summary>
        public virtual void Move()
        {
            Position += Velocity;
        }
    }
}
