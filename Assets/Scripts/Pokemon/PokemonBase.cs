using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Pokemon", menuName = "New Pokemon ")]
public class PokemonBase : ScriptableObject
{
    [SerializeField] new string name;

    [HorizontalGroup("Sprites")]
    [PreviewField(100)]
    [SerializeField] Sprite frontSprite;
    [HorizontalGroup("Sprites")]
    [PreviewField(100)]
    [SerializeField] Sprite backSprite;

    [SerializeField] int catchRate;

    [BoxGroup("Types")]
    [SerializeField] PokemonType type1;
    [BoxGroup("Types")]
    [SerializeField] PokemonType type2;

    [BoxGroup("Size")]
    [SerializeField] int height;
    [BoxGroup("Size")]
    [SerializeField] int weight;

    [BoxGroup("EXP")]
    [SerializeField] int expYield;
    [BoxGroup("EXP")]
    [SerializeField] GrowthType growthRate;


    [BoxGroup("Base Stat")]
    [SerializeField] int hp;
    [BoxGroup("Base Stat")]
    [SerializeField] int attack;
    [BoxGroup("Base Stat")]
    [SerializeField] int defense;
    [BoxGroup("Base Stat")]
    [SerializeField] int spAttack;
    [BoxGroup("Base Stat")]
    [SerializeField] int spDefense;
    [BoxGroup("Base Stat")]
    [SerializeField] int speed;

    [SerializeField] List<LearnableMove> learnableMoves;

    public string Name => name;
    public Sprite FrontSprite => frontSprite;
    public Sprite BackSprite => backSprite;
    public int CatchRate => catchRate;
    public PokemonType Type1 => type1;
    public PokemonType Type2 => type2;
    public int Height => height;
    public int Weight => weight;
    public int ExpYield => expYield;
    public GrowthType GrowthRate => growthRate;
    public int HP => hp;
    public int Attack => attack;
    public int Defense => defense;
    public int SpAttack => spAttack;
    public int SpDefense => spDefense;
    public int Speed => speed;

    public List<LearnableMove> LearnableMoves => learnableMoves;
}

[System.Serializable]
public class LearnableMove
{
    [SerializeField] MoveBase moveBase;
    [SerializeField] int level;

    public MoveBase Base => moveBase;
    public int Level => level;
}


