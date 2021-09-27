using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pokemon
{
    [SerializeField] PokemonBase _base;
    [SerializeField] int level;
    [SerializeField] string nickName;

    public Pokemon(PokemonBase _base, int level)
    {
        this._base = _base;
        this.level = level;

        Init();
    }

    public PokemonBase Base => _base;
    public int Level => level;
    public StatusCondition Status { get; set; }

    //if no nickname, use specie name instead
    public string Name => string.IsNullOrEmpty(nickName) ? Base.Name : nickName;

    public List<Move> Moves { get; set; }

    public int Attack { get; private set; }
    public int Defense { get; private set; }
    public int SpAttack { get; private set; }
    public int SpDefense { get; private set; }
    public int Speed { get; private set; }
    public int MaxHP { get; private set; }
    public int HP { get; set; }


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

        CalculateStats();
        Status = null;
        HP = MaxHP;
    }

    void CalculateStats()
    {
        Attack = Mathf.FloorToInt((Base.Attack * Level) / 100f) + 5;
        Defense = Mathf.FloorToInt((Base.Defense * Level) / 100f) + 5;
        SpAttack = Mathf.FloorToInt((Base.SpAttack * Level) / 100f) + 5;
        SpDefense = Mathf.FloorToInt((Base.SpDefense * Level) / 100f) + 5;
        Speed = Mathf.FloorToInt((Base.Speed * Level) / 100f) + 5;

        MaxHP = Mathf.FloorToInt((Base.HP * Level) / 100f) + Level + 10;
    }

    public void UpdateHP(int damage)
    {
        HP = Mathf.Clamp(HP - damage, 0, MaxHP);
    }

    public bool KnowMove(MoveBase move)
    {
        return Moves.Exists(m => m.Base == move);
    }

    public int IndexofMove(MoveBase move)
    {
        return Moves.FindIndex(m => m.Base == move);
    }
}

