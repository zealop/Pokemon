using System;
using System.Collections.Generic;
using System.Linq;
using Game.Condition;
using Game.Constants;
using Game.Moves;
using Game.Statuses;
using UnityEngine;

namespace Game.Pokemons
{
    [Serializable]
    public class Pokemon
    {
        [SerializeField] private PokemonBase @base;
        [SerializeField] private int level;
        [SerializeField] private string nickName;
        [SerializeField] private List<MoveSlot> moves;
        
        private PokemonBase Base => @base;
        public int Exp { get; set; }
        public int Level => level;
        public StatusCondition StatusCondition { get; set; }
        public string Name => string.IsNullOrEmpty(nickName) ? Base.Name : nickName;
        public List<MoveSlot> Moves => moves;
        public int Attack => Mathf.FloorToInt((Base.Attack * Level) / 100f) + 5;
        public int Defense => Mathf.FloorToInt((Base.Defense * Level) / 100f) + 5;
        public int SpAttack => Mathf.FloorToInt((Base.SpAttack * Level) / 100f) + 5;
        public int SpDefense => Mathf.FloorToInt((Base.SpDefense * Level) / 100f) + 5;
        public int Speed => Mathf.FloorToInt((Base.Speed * Level) / 100f) + 5;
        public PokemonType Type1 => Base.Type1;
        public PokemonType Type2 => Base.Type2;
        public int MaxHp { get; set; }

        public int Hp { get; set; }
        public int ExpReward => level * Base.ExpYield;
        public bool IsFainted => Hp <= 0;

        public void Init()
        {
            moves = new List<MoveSlot>();
            foreach (var move in Base.LearnableMoves)
            {
                if (move.Level <= Level)
                    Moves.Add(new MoveSlot(move.Base));

                if (Moves.Count >= 4)
                    break;
            }

            // Exp = EXPChart.GetEXPAtLevel(Base.GrowthRate, level);

            StatusCondition = null;
            MaxHp = Mathf.FloorToInt((Base.Hp * Level) / 100f) + Level + 10;
            Hp = MaxHp;
        }

        public Pokemon(PokemonBase @base, int level)
        {
            this.@base = @base;
            this.level = level;

            Init();
        }

        // public Pokemon(PokemonSaveData data)
        // {
        //     Hp = data.hp;
        //     level = data.level;
        //     Exp = data.exp;
        //     Status = data.status;
        //     @base = Resources.Load<PokemonBase>($"Pokemons/{data.specie}");
        //     Moves = data.moves.Select(m => new MoveSlot(m)).ToList();
        //
        //     CalculateStats();
        // }


        public void UpdateHp(int newHp)
        {
            Hp = Mathf.Clamp(newHp, 0, MaxHp);
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
            var oldMaxHp = MaxHp;
            level++;
            Hp += MaxHp - oldMaxHp;
        }

        public List<MoveBase> GetMovesToLearnOnLevelUp()
        {
            return Base.LearnableMoves.Where(m => m.Level == level).Select(m => m.Base).ToList();
        }

        // public PokemonSaveData GetSaveData()
        // {
        //     var data = new PokemonSaveData()
        //     {
        //         hp = Hp,
        //         level = level,
        //         exp = Exp,
        //         status = Status,
        //         specie = @base.name,
        //         moves = Moves.Select(m => m.GetSaveData()).ToList(),
        //     };
        //     return data;
        // }
    }

    // public class PokemonSaveData
    // {
    //     public string specie;
    //     public int hp;
    //     public int level;
    //     public int exp;
    //     public Status status;
    //     public List<MoveSaveData> moves;
    // }
}