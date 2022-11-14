using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils
{
    public static class WeightedRng
    {
        public static T Roll<T>(T[] candidates, int[] weights)
        {
            var weightSum = weights.Sum();
            var roll = Random.Range(0, weightSum);

            for (var i = 0; i < candidates.Length; i++)
            {
                if (roll < weights[i])
                {
                    return candidates[i];
                }

                roll -= weights[i];
            }

            return default;
        }

        public static T Roll<T>((T candidate, int weight)[] table)
        {
            var weightSum = table.Sum(t => t.weight);
            var roll = Random.Range(0, weightSum);

            foreach (var entry in table)
            {
                if (roll < entry.weight)
                {
                    return entry.candidate;
                }

                roll -= entry.weight;
            }

            return default;
        }
        
        public static T Roll<T>(Dictionary<T, int> table)
        {
            var weightSum = table.Sum(t => t.Value);
            var roll = Random.Range(0, weightSum);

            foreach (var entry in table)
            {
                if (roll < entry.Value)
                {
                    return entry.Key;
                }

                roll -= entry.Value;
            }

            return default;
        }
    }
}