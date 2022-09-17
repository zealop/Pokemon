using System;
using System.Collections.Generic;
using System.Linq;
using Data.Condition;
using Move;
using UnityEngine;
using UnityEngine.Serialization;

namespace Pokemons
{
    [Serializable]
    public class Pokemon
    {
        [FormerlySerializedAs("_base")] [SerializeField] private PokemonBase @base;
        [SerializeField] private int level;
        [SerializeField] private string nickName;
    
        public PokemonBase Base => @base;
        public int Exp { get; set; }
        public int Level => level;
        public Status Status { get; set; }
        public string Name => string.IsNullOrEmpty(nickName) ? Base.SpecieName : nickName; //if no nickname, use specie name instead
        public List<MoveSlot> Moves { get; set; }
        public int Attack { get; private set; }
        public int Defense { get; private set; }
        public int SpAttack { get; private set; }
        public int SpDefense { get; private set; }
        public int Speed { get; private set; }
        public int MaxHp { get; private set; }
        public int Hp { get; set; }
        public int ExpReward => level * Base.ExpYield;

        public void Init()
        {
            Moves = new List<MoveSlot>();
            foreach (var move in Base.LearnableMoves)
            {
                if (move.Level <= Level)
                    Moves.Add(new MoveSlot(move.Base));

                if (Moves.Count >= 4)
                    break;
            }

            Exp = EXPChart.GetEXPAtLevel(Base.GrowthRate, level);

            CalculateStats();

            Status = null;
            Hp = MaxHp;
        }
        public Pokemon(PokemonBase _base, int level)
        {
            this.@base = _base;
            this.level = level;

            Init();
        }
        public Pokemon(PokemonSaveData data)
        {
            Hp = data.hp;
            level = data.level;
            Exp = data.exp;
            Status = data.status;
            @base = Resources.Load<PokemonBase>($"Pokemons/{data.specie}");
            Moves = data.moves.Select(m => new MoveSlot(m)).ToList();

            CalculateStats();
        }

        private void CalculateStats()
        {
            Attack = Mathf.FloorToInt((Base.Attack * Level) / 100f) + 5;
            Defense = Mathf.FloorToInt((Base.Defense * Level) / 100f) + 5;
            SpAttack = Mathf.FloorToInt((Base.SpAttack * Level) / 100f) + 5;
            SpDefense = Mathf.FloorToInt((Base.SpDefense * Level) / 100f) + 5;
            Speed = Mathf.FloorToInt((Base.Speed * Level) / 100f) + 5;

            MaxHp = Mathf.FloorToInt((Base.Hp * Level) / 100f) + Level + 10;
        }
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
            level++;
            int temp = MaxHp;
            CalculateStats();
            Hp += MaxHp - temp;
        }
        public List<MoveBase> GetMovesToLearnOnLevelUp()
        {
            return Base.LearnableMoves.Where(m => m.Level == level).Select(m => m.Base).ToList();
        }
        public PokemonSaveData GetSaveData()
        {
            var data = new PokemonSaveData()
            {
                hp = Hp,
                level = level,
                exp = Exp,
                status = Status,
                specie = @base.name,
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
        public Status status;
        public List<MoveSaveData> moves;
    }
}