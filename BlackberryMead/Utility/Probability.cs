using System;

namespace BlackberryMead.Utility
{
    public static class Probability
    {
        public static readonly Random random = new Random();

        /// <summary>
        /// Returns the value of the Normal Distribution at x
        /// </summary>
        /// <param name="std">Standard Deviation</param>
        /// <param name="mean">Mean or expected value</param>
        /// <param name="c">Multiplicative constant</param>
        /// <param name="x">Value to evaluate the function at</param>
        /// <returns></returns>
        public static double NormalDistributionValue(float std, float mean, float c, float x)
        {
            return c / (std * Math.Sqrt(2 * Math.PI)) * Math.Exp(-0.5 * Math.Pow((x - mean) / std, 2));
        }


        /// <summary>
        /// Returns a random integer between an upper and lower bound
        /// </summary>
        /// <param name="lowerBound">Inclusive</param>
        /// <param name="upperBound">Exclusive</param>
        /// <returns></returns>
        public static int RandomInteger(int lowerBound, int upperBound)
        {
            return random.Next(lowerBound, upperBound);
        }


        /// <summary>
        /// Returns a random value weighted by an array of values
        /// </summary>
        /// <param name="values">List of weighted values</param>
        /// <returns></returns>
        public static int WeightedRandom(double[] values)
        {
            double sum = 0;
            for (int i = 0; i < values.Length; i++)
            {
                sum += values[i];
            }

            // randomize value
            double rndValue = random.Next(0, (int)Math.Floor(sum));

            for (int i = 0; i < values.Length; i++)
            {
                if (rndValue < values[i])
                {
                    return i;
                }
                rndValue -= values[i];
            }

            return values.Length - 1;
        }
    }
}
