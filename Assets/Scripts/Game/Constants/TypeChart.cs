using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;

namespace Game.Constants
{
    [CreateAssetMenu(fileName = "Pokemon", menuName = "New type chart")]
    public class TypeChart : SerializedScriptableObject
    {
        [OdinSerialize] private Dictionary<PokemonType, Dictionary<PokemonType, float>> chart;

        public float GetEffectiveNess(PokemonType attackType, PokemonType defenseType)
        {
            return chart[attackType][defenseType];
        }
    }
}