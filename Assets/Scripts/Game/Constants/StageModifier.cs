using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Constants
{
    [CreateAssetMenu(menuName = "Pokemon/Stage modifier")]
    public class StageModifier : ScriptableObject
    {
        [SerializeField] private float[] normalModifier;
        [SerializeField] private float[] precisionModifier;
        [SerializeField] private float[] critModifier;
        [SerializeField] private float critBonus;

        public float[] NormalModifier => normalModifier;
        public float[] PrecisionModifier => precisionModifier;
        public float[] CritModifier => critModifier;
        public float CritBonus => critBonus;

        public bool IsCrit(int critStage)
        {
            return Random.value <= critModifier[Math.Clamp(critStage, 0, critModifier.Length)];
        }
    }
}