﻿using System;
using System.Collections.Generic;

namespace BlackberryMead.Collections
{
    /// <summary>
    /// A pair of two <see cref="Dictionary{TKey, TValue}"/>.
    /// </summary>
    public class DoubleDictionary<T, S> where T : notnull
    {
        private Dictionary<T, S> dict1;

        private Dictionary<T, S> dict2;


        /// <summary>
        /// Creates a new empty <see cref="DoubleDictionary{T, S}"/>.
        /// </summary>
        public DoubleDictionary()
        {
            dict1 = new Dictionary<T, S>();
            dict2 = new Dictionary<T, S>();
        }


        /// <summary>
        /// Creates a new <see cref="DoubleDictionary{T, S}"/> from two existing <see cref="Dictionary{TKey, TValue}"/>.
        /// </summary>
        public DoubleDictionary(Dictionary<T, S> dict1, Dictionary<T, S> dict2)
        {
            this.dict1 = dict1;
            this.dict2 = dict2;
        }


        /// <summary>
        /// Gets the tuple of both values if the the given key exists.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public (S, S) this[T key]
        {
            get
            {
                S value1 = default;
                S value2 = default;
                if (dict1.TryGetValue(key, out S s1))
                {
                    value1 = s1;
                }
                if (dict2.TryGetValue(key, out S s2))
                {
                    value2 = s2;
                }
                return (value1, value2);
            }
        }


        /// <summary>
        /// Creates a new <see cref="DoubleDictionary{T, S}"/> from an existing <see cref="DoubleDictionary{T, S}"/>.
        /// </summary>
        public DoubleDictionary(DoubleDictionary<T, S> doubleDict)
        {
            dict1 = new Dictionary<T, S>(doubleDict[0]);
            dict2 = new Dictionary<T, S>(doubleDict[1]);
        }


        /// <summary>
        /// Returns one of the two dictionaries in the <see cref="DoubleDictionary{T, S}"/>.
        /// </summary>
        /// <param name="i">Possible values 0 or 1.</param>
        /// <returns></returns>
        /// <exception cref="IndexOutOfRangeException">Throws an exception if <paramref name="i"/> is not
        /// 0 or 1.</exception>
        public Dictionary<T, S> this[int i]
        {
            get
            {
                if (i == 0)
                    return dict1;
                else if (i == 1)
                    return dict2;
                else
                    throw new IndexOutOfRangeException();
            }

            set
            {
                if (i == 0)
                    dict1 = value;
                else if (i == 1)
                    dict2 = value;
                else
                    throw new IndexOutOfRangeException();
            }
        }


        /// <summary>
        /// Returns a new shallow copy of the <see cref="DoubleDictionary{T, S}"/>.
        /// </summary>
        public DoubleDictionary<T, S> ToNew()
        {
            Dictionary<T, S> newDict1 = new Dictionary<T, S>(dict1);
            Dictionary<T, S> newDict2 = new Dictionary<T, S>(dict2);
            return new DoubleDictionary<T, S>(newDict1, newDict2);
        }
    }
}
