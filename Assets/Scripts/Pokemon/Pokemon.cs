using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Pokemon
{
    [SerializeField] private PokemonBase _base;
    [SerializeField] private int level;
    [SerializeField] private string nickName;



    public PokemonBase Base => _base;
    public int EXP { get; set; }
    public int Level => level;
    public StatusCondition Status { get; set; }
    public string Name => string.IsNullOrEmpty(nickName) ? Base.Name : nickName; //if no nickname, use specie name instead
    public List<Move> Moves { get; set; }
    public int Attack { get; private set; }
    public int Defense { get; private set; }
    public int SpAttack { get; private set; }
    public int SpDefense { get; private set; }
    public int Speed { get; private set; }
    public int MaxHP { get; private set; }
    public int HP { get; set; }
    public int ExpReward => level * Base.ExpYield;

    public void Init()
    {
        Moves = new List<Move>();
        foreach (var move in Base.LearnableMoves)
        {
            if (move.Level <= Level)
                Moves.Add(new Move(move.Base));

            if (Moves.Count >= 4)
                break;
        }

        EXP = EXPChart.GetEXPAtLevel(Base.GrowthRate, level);

        CalculateStats();

        Status = null;
        HP = MaxHP;
    }
    public Pokemon(PokemonBase _base, int level)
    {
        this._base = _base;
        this.level = level;

        Init();
    }
    public Pokemon(PokemonSaveData data)
    {
        HP = data.hp;
        level = data.level;
        EXP = data.exp;
        Status = data.status;
        _base = Resources.Load<PokemonBase>($"Pokemons/{data.specie}");
        Moves = data.moves.Select(m => new Move(m)).ToList();

        CalculateStats();
    }

    private void CalculateStats()
    {
        Attack = Mathf.FloorToInt((Base.Attack * Level) / 100f) + 5;
        Defense = Mathf.FloorToInt((Base.Defense * Level) / 100f) + 5;
        SpAttack = Mathf.FloorToInt((Base.SpAttack * Level) / 100f) + 5;
        SpDefense = Mathf.FloorToInt((Base.SpDefense * Level) / 100f) + 5;
        Speed = Mathf.FloorToInt((Base.Speed * Level) / 100f) + 5;

        MaxHP = Mathf.FloorToInt((Base.HP * Level) / 100f) + Level + 10;
    }
    public void UpdateHP(int newHP)
    {
        HP = Mathf.Clamp(newHP, 0, MaxHP);
    }
    public bool KnowMove(MoveBase move)
    {
        return Moves.Exists(m => m.Base == move);
    }
    public int IndexofMove(MoveBase move)
    {
        return Moves.FindIndex(m => m.Base == move);
    }
    public void LevelUp()
    {
        level++;
        int temp = MaxHP;
        CalculateStats();
        HP += MaxHP - temp;
    }
    public List<MoveBase> GetMovesToLearnOnLevelUp()
    {
        return Base.LearnableMoves.Where(m => m.Level == level).Select(m => m.Base).ToList();
    }
    public PokemonSaveData GetSaveData()
    {
        var data = new PokemonSaveData()
        {
            hp = HP,
            level = level,
            exp = EXP,
            status = Status,
            specie = _base.name,
            moves = Moves.Select(m => m.GetSaveData()).ToList(),
        };
        return data;
    }


}

public class PokemonSaveData
{
    public string specie;
    public int hp;
    public int level;
    public int exp;
    public StatusCondition status;
    public List<MoveSaveData> moves;
}