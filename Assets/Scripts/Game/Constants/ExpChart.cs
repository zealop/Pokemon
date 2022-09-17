using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game.Constants
{
    [CreateAssetMenu(fileName = "Pokemon", menuName = "New exp ")]
    public class ExpChart : ScriptableSingleton<ExpChart>
    {
        [SerializeField] private Dictionary<PokemonType, Dictionary<PokemonType, float>> value;
    }
}
