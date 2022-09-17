using System.Collections.Generic;
using Game.Constants;
using Game.Moves;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Pokemons
{
    [CreateAssetMenu(fileName = "Pokemon", menuName = "New Pokemon")]
    public class PokemonBase : ScriptableObject
    {
        [SerializeField] private string baseName;

        [SerializeField] private PokemonSprite sprite;

        [SerializeField] private int catchRate;

        [BoxGroup("Types")] [SerializeField] private PokemonType type1;
        [BoxGroup("Types")] [SerializeField] private PokemonType type2;

        [BoxGroup("Size")] [SerializeField] private int height;
        [BoxGroup("Size")] [SerializeField] private int weight;

        [BoxGroup("EXP")] [SerializeField] private int expYield;
        [BoxGroup("EXP")] [SerializeField] private GrowthType growthRate;

        [BoxGroup("Base Stat")] [SerializeField]
        private int hp;

        [BoxGroup("Base Stat")] [SerializeField]
        private int attack;

        [BoxGroup("Base Stat")] [SerializeField]
        private int defense;

        [BoxGroup("Base Stat")] [SerializeField]
        private int spAttack;

        [BoxGroup("Base Stat")] [SerializeField]
        private int spDefense;

        [BoxGroup("Base Stat")] [SerializeField]
        private int speed;

        [SerializeField] private List<LearnableMove> learnableMoves;

        public string Name => baseName;
        public PokemonSprite Sprite => sprite;

        public int CatchRate => catchRate;

        public PokemonType Type1 => type1;

        public PokemonType Type2 => type2;

        public int Height => height;

        public int Weight => weight;

        public int ExpYield => expYield;

        public GrowthType GrowthRate => growthRate;

        public int Hp => hp;

        public int Attack => attack;

        public int Defense => defense;

        public int SpAttack => spAttack;

        public int SpDefense => spDefense;

        public int Speed => speed;

        public List<LearnableMove> LearnableMoves => learnableMoves;
    }
}