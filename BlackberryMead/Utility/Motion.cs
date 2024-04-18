using Microsoft.Xna.Framework;
using System;

namespace BlackberryMead.Utility
{
    /// <summary>
    /// Utility class for various types of motion.
    /// </summary>
    /// <remarks>
    /// UNSTABLE
    /// </remarks>
    public static class Motion
    {
        /// <summary>
        /// Returns a vector representing the tick movement towards the destination 
        /// from the current position on a straight line.
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="currentPosition"></param>
        /// <param name="movementSpeed"></param>
        /// 
        public static Vector2 LinearMotion(Vector2 destination, Vector2 currentPosition, int movementSpeed)
        {
            // calculate linear distance
            double dis = Vector2.Distance(currentPosition, destination);

            // if the distance is greater than 0, move accordingly
            if (dis > 0)
            {
                // calculate x and y distances
                int xDis = (int)(destination.X - currentPosition.X);
                int yDis = (int)(destination.Y - currentPosition.Y);

                // calculate movement distances
                double moveX = xDis * movementSpeed / dis;
                double moveY = yDis * movementSpeed / dis;

                return new Vector2((int)moveX, (int)moveY);
            }

            // if the distances is 0, return zero vector
            return Vector2.Zero;
        }

        /// <summary>
        /// Returns a vector representing the tick movement in a circle. 
        /// Requires an external increasing integer tick.
        /// </summary>
        /// <param name="center"></param>
        /// <param name="currentPosition"></param>
        /// <param name="radius"></param>
        /// <param name="movementSpeed"></param>
        /// <param name="tick"></param>
        /// <returns></returns>
        public static Vector2 CircularMotion(Vector2 center, int radius, int movementSpeed, int tick)
        {
            int x = (int)(center.X + radius * Math.Cos(0.01 * movementSpeed * tick));
            int y = (int)(center.Y + radius * Math.Sin(0.01 * movementSpeed * tick));
            return new Vector2(x, y);
        }


        /// <summary>
        /// Returns a vector representing the tick movement towards a destination 
        /// in a sinusoidal motion. Requires an external increasing integer tick.
        /// </summary>
        /// <param name="destination"></param>
        /// <param name="currentPosition"></param>
        /// <param name="movementSpeed"></param>
        /// <param name="oscellateSpeed"></param>
        /// <param name="tick"></param>
        /// <returns></returns>
        public static Vector2 SinusoidalMotion(Vector2 destination, Vector2 currentPosition, int movementSpeed,
            double oscellateSpeed, int amplitude, int tick)
        {
            // draw distance line
            // calculate linear distance
            int dis = (int)Vector2.Distance(currentPosition, destination);

            // if the distance is greater than 0, move accordingly
            if (dis > 0)
            {
                // calculate x and y distances
                int xDis = (int)(destination.X - currentPosition.X);
                int yDis = (int)(destination.Y - currentPosition.Y);

                // calculate line angle in radians
                float theta = 0;
                if (xDis == 0)
                {
                    if (yDis > 0)
                    {
                        theta = (float)(3 * Math.PI / 2);
                    }
                    else if (yDis < 0)
                    {
                        theta = (float)(Math.PI / 2);
                    }
                }
                else
                    theta = (float)Math.Atan(yDis / xDis);

                // if the x distance is negative, flip the angle to the other side of the unit circle
                if (xDis < 0)
                {
                    theta += (float)Math.PI;
                }

                // create rotate matrices
                Matrix toLineMatrix = Matrix.CreateRotationZ(theta);
                Matrix toAxisMatrix = Matrix.Invert(toLineMatrix);

                // create rotated position vectors
                Vector2 rotatedPos = Vector2.Transform(currentPosition, toAxisMatrix);
                Vector2 rotatedDestination = Vector2.Transform(destination, toAxisMatrix);

                int xMove = movementSpeed;
                int yMove = (int)(amplitude * Math.Sin(oscellateSpeed * tick));


                // rotate vectors back
                Vector2 moveVector = Vector2.Transform(new Vector2(xMove, yMove), toLineMatrix);
                // convert to int
                moveVector = new Vector2((int)moveVector.X, (int)moveVector.Y);

                return moveVector;
            }
            return Vector2.Zero;
        }


        /// <summary>
        /// Returns a float representing the rotation of the object.
        /// </summary>
        /// <param name="t"></param>
        /// <param name="rotateSpeed"></param>
        /// <param name="type">Possible values: "Exponential", "Quadratic", "Cubic", "Linear"</param>
        /// <returns></returns>
        public static float StaticRotation(double t, double rotateSpeed, string type)
        {
            switch (type)
            {
                case "Exponential":
                    return 0f;

                case "Quadratic":
                    return (float)Math.Pow((t / 100), 2);

                case "Cubic":
                    return 0f;

                case "Linear":
                    return 0f;
            }

            return 0f;
        }


        /// <summary>
        /// Moves a float value between values of an array.
        /// </summary>
        /// <returns></returns>
        public static float AlternatingRotation(float[] pivots)
        {
            return 0f;
        }


        /// <summary>
        /// Returns a vector representing the position of the object 
        /// at tick t along a cyclical parametric function.
        /// </summary>
        /// <param name="origin">Center of the function</param>
        /// <param name="range">Maximum range of the object in pixels +- from the origin</param>
        /// <param name="a">Randomizing variable a. Best results 2.0 > a > 1.0</param>
        /// <param name="b">Randomizing variable b. Best results 2.0 > b > 1.0</param>
        /// <param name="theta">Angle of rotation around origin in units pi</param>
        /// <param name="t">Tick</param>
        /// <returns></returns>
        public static Vector2 Bee(Vector2 origin, Point range, double a, double b, float theta, int movementSpeed, double t)
        {
            // change t to represent movement sped
            t = t * movementSpeed / ((Math.Pow(1.012, t) + 90));

            // create return vector
            Vector2 position = Vector2.Zero;

            position.X = (float)(2 * range.X * (0.9 * (t) * Math.Cos(a * t)) * Math.Sin(t));
            position.Y = (float)(4 * range.Y * Math.Sin(b * t) + 4.3 * Math.Cos(t));

            // rotate around origin
            Matrix matrix = Matrix.CreateRotationZ((float)(theta * Math.PI));
            position = Vector2.Transform(position, matrix);

            return position + origin;
        }
    }
}
