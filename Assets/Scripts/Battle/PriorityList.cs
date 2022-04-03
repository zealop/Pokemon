using System.Collections.Generic;

namespace Battle
{
    public class PriorityList<T>
    {
        private readonly SortedDictionary<int, List<T>> dictionary = new SortedDictionary<int, List<T>>();

        public void Add(T element, int priority = 0)
        {
            if (!dictionary.TryGetValue(priority, out var list))
            {
                list = new List<T>();
                dictionary.Add(priority, list);
            }
            list.Add(element);
        }

        public void Remove(T element, int priority = 0)
        {
            if (dictionary.TryGetValue(priority, out var list))
            {
                list.Remove(element);
            }
        }

        public List<T> ToList()
        {
            var result = new List<T>();
            foreach (var list in dictionary.Values)
            {
                result.AddRange(list);
            }

            return result;
        }
    }
}