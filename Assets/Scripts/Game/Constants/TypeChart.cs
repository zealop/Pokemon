using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Game.Constants
{
    [CreateAssetMenu(menuName = "Pokemon/Type chart")]
    public class TypeChart : SerializedScriptableObject
    {
        [SerializeField] private float stabBonus;
        [OdinSerialize] private Dictionary<PokemonType, Dictionary<PokemonType, float>> _chart;
        public float StabBonus => stabBonus;

        public float GetEffectiveness(PokemonType attackType, IEnumerable<PokemonType> defenseTypes)
        {
            try
            {
                return defenseTypes
                    .Select(t => _chart[attackType][t])
                    .Aggregate((a, b) => a * b);
            }
            catch (KeyNotFoundException)
            {
                return 1;
            }
        }
    }
}