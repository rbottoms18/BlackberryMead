using System;
using System.Collections.Generic;

namespace BlackberryMead.Utility
{
    /// <summary>
    /// Represents a bag of items with weighted probabilities.
    /// Has methods to return a random element from the bag.
    /// </summary>
    public class WeightedRandomBag<T>
    {
        private Random rnd = new Random();

        /// <summary>
        /// An entry in the bag.
        /// </summary>
        private struct Entry
        {
            public int weight;
            public T item;
        }

        /// <summary>
        /// List of all the possible entries in the bag.
        /// </summary>
        private List<Entry> entries;

        /// <summary>
        /// Total weight of all items in the bag.
        /// </summary>
        private int totalWeight;


        /// <summary>
        /// Creates a new empty <see cref="WeightedRandomBag{T}"/>.
        /// </summary>
        public WeightedRandomBag()
        {
            entries = new List<Entry>();
        }


        /// <summary>
        /// Creates a <see cref="WeightedRandomBag{T}"/> from a list of items and weights.
        /// Items and weights must index match.
        /// Generates the probability table.
        /// </summary>
        /// <param name="items"></param>
        /// <param name="weights"></param>
        public WeightedRandomBag(List<T> items, List<int> weights)
        {
            entries = new List<Entry>();
            // Sort items by weight
            for (int i = 0; i < items.Count; i++)
            {
                AddEntry(items[i], weights[i]);
            }
        }


        /// <summary>
        /// Add an entry to the <see cref="WeightedRandomBag{T}"/>.
        /// Recomputes the order of entries in the <see cref="WeightedRandomBag{T}"/> to optimize pulling.
        /// </summary>
        public void AddEntry(T item, int weight)
        {
            totalWeight += weight;
            // Find where it should be put in the sorted list
            entries.Add(new Entry { item = item, weight = totalWeight });
        }


        /// <summary>
        /// Gets a random item from the <see cref="WeightedRandomBag{T}"/>.
        /// </summary>
        /// <returns></returns>
        public T GetRandom()
        {
            // Generate a value between 0 and totalWeight
            int r = rnd.Next(totalWeight);

            // Loop through all entries
            foreach (Entry e in entries)
            {
                if (e.weight >= r)
                    return e.item;
            }
            // Should only return this when entries is empty
            return default;
        }


        /// <summary>
        /// Returns an unweighted random <typeparamref name="T"/> from the list of
        /// all possible <typeparamref name="T"/> objects in the <see cref="WeightedRandomBag{T}"/>.
        /// </summary>
        public T PickOne()
        {
            return entries[rnd.Next(0, entries.Count)].item;
        }
    }
}
