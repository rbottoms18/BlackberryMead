#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackberryMead.Utility
{
    /// <summary>
    /// Static helper class that contains useful methods for manipulating arrays and other <see cref="IEnumerable{T}"/>.
    /// </summary>
    public static class ArrayHelper
    {
        /// <summary>
        /// Fill an array with a value.
        /// </summary>
        /// <typeparam name="T">Value type of the array.</typeparam>
        /// <param name="array">The array to be filled.</param>
        /// <param name="value">The value to fill the array with.</param>
        /// <returns><paramref name="array"/> with every entry replaced with <paramref name="value"/>.</returns>
        public static T[] FillArray<T, S>(T[] array, S value) where S : T
        {
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = value;
            }
            return array;
        }


        /// <inheritdoc cref="FillArray{T}(T[], T)"/>
        public static T[,] FillArray<T>(T[,] array, T value)
        {
            for (int i = 0; i <= array.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= array.GetUpperBound(1); j++)
                {
                    array[i, j] = value;
                }
            }
            return array;
        }


        /// <inheritdoc cref="FillArray{T}(T[], T)"/>
        public static T[][] FillArray<T>(T[][] array, T value)
        {
            for (int i = 0; i < array.Length; i++)
            {
                for (int j = 0; j < array[i].Length; j++)
                {
                    array[i][j] = value;
                }
            }
            return array;
        }


        /// <summary>
        /// Flattens a 2d array into a list.
        /// </summary>
        /// <typeparam name="T">Type of objects in <paramref name="array"/>.</typeparam>
        /// <param name="array">2 dimensional array to flatten.</param>
        /// <returns>A list of all elements in <paramref name="array"/>, ordered by rows.</returns>
        public static List<T> Flatten<T>(T[,] array)
        {
            List<T> _ = new List<T>();
            for (int i = 0; i < array.GetUpperBound(0); i++)
            {
                for (int j = 0; j < array.GetUpperBound(1); j++)
                {
                    _.Add(array[i, j]);
                }
            }
            return _;
        }


        /// <summary>
        /// Flatten a jagged list into a list.
        /// </summary>
        /// <typeparam name="T">Type of objects in <paramref name="list"/>.</typeparam>
        /// <param name="list">Jagged list to flatten.</param>
        /// <returns>A list of all elements in <paramref name="list"/>.</returns>
        public static List<T> Flatten<T>(List<List<T>> list)
        {
            List<T> _ = new List<T>();
            foreach (List<T> l in list)
            {
                _ = _.Concat(l).ToList();
            }
            return _;
        }


        /// <summary>
        /// Unflattens a List of objects into a two dimensional array.
        /// </summary>
        /// <typeparam name="T">Type of objects to populate with.</typeparam>
        /// <param name="values">List of values to unflatten.</param>
        /// <param name="rows">Number of rows in the 2d array.</param>
        /// <param name="columns">Number of columns in the 2d array.</param>
        /// <returns>A two dimensional array of size [<paramref name="rows"/>, <paramref name="columns"/>]
        /// populated with objects from values.<br/>
        /// Objects from <paramref name="values"/> will only be added up to the capacity of the array. If there are fewer
        /// objects in <paramref name="values"/> than elements in the array, any remaining spaces will be filled with null.</returns>
        public static T[,] Unflatten<T>(List<T> values, int rows, int columns)
        {
            T[,] _ = new T[rows, columns];
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    _[i, j] = values[i * columns + j];
                }
            }
            return _;
        }


        /// <summary>
        /// Replaces all instances of <paramref name="targetValue"/> in <paramref name="values"/> with <paramref name="replacement"/>.
        /// </summary>
        /// <typeparam name="T">Type of objects in <paramref name="values"/>.</typeparam>
        /// <param name="values">2d array of values to operate on.</param>
        /// <param name="targetValue">Value to be replaced with <paramref name="replacement"/>.</param>
        /// <param name="replacement">Value to be assigned to any instance of <paramref name="targetValue"/>.</param>
        /// <returns>A 2d array identical to <paramref name="values"/> except any instance of <paramref name="targetValue"/>
        /// is replaced with <paramref name="replacement"/>.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static T[,] Replace<T>(T[,] values, T? targetValue, T? replacement)
        {
            T[,] _ = new T[values.GetUpperBound(0), values.GetUpperBound(1)];

            if (values == null)
                throw new ArgumentNullException(nameof(values));

            for (int i = 0; i < values.GetUpperBound(0); i++)
            {
                for (int j = 0; j < values.GetUpperBound(1); j++)
                {
                    _[i, j] = values[i, j]!.Equals(targetValue) ? values[i, j] : replacement!;
                }
            }
            return _;
        }


        /// <summary>
        /// Creates a new array filled with <paramref name="value"/>.
        /// </summary>
        /// <typeparam name="T">Type of array to be created.</typeparam>
        /// <param name="rows">Number of rows in the array.</param>
        /// <param name="columns">Number of columns in the array.</param>
        /// <param name="value">Value to fill the array with.</param>
        /// <returns>A new array of dimensions [<paramref name="rows"/>, <paramref name="columns"/>]
        /// filled with <paramref name="value"/>.</returns>
        public static T[,] NewFilledArray<T>(int rows, int columns, T value)
        {
            T[,] _ = new T[rows, columns];
            FillArray(_, value);
            return _;
        }


        /// <summary>
        /// Creates a jagged new array filled with <paramref name="value"/>.
        /// </summary>
        ///<inheritdoc cref="NewFilledArray{T}(int, int, T)"/>
        public static T[][] NewFilledJaggedArray<T>(int rows, int columns, T value)
        {
            T[][] _ = new T[rows][];
            for (int i = 0; i < rows; i++)
                _[i] = new T[columns];
            FillArray(_, value);
            return _;
        }


        /// <summary>
        /// Removes the given index from the array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">Array to remove index from.</param>
        /// <param name="index">Index to remove.</param>
        /// <returns></returns>
        public static T[] Remove<T>(T[] array, int index)
        {
            List<T> list = array.ToList();
            list.RemoveAt(index);
            return list.ToArray();
        }
    }
}
