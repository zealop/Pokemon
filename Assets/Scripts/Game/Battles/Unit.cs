using System;
using System.Collections.Generic;
using Game.Condition;
using Game.Constants;
using Game.Moves;
using Game.Pokemons;
using UnityEngine;
using Utils;

namespace Game.Battles
{
    public class Unit
    {
        public Side Side { get; }
        public Battle Battle => Side.Battle;
        private Pokemon Pokemon { get; }
        public string Name => Pokemon.Name;
        public int Attack { get; set; }
        public int Defense { get; set; }
        public int SpAttack { get; set; }
        public int SpDefense { get; set; }
        public int Speed { get; set; }

        public int Hp
        {
            get => Pokemon.Hp;
            set => Pokemon.Hp = value;
        }

        public float MaxHp => Pokemon.MaxHp;


        public int TotalAttack => Attack;
        public int TotalDefense => Defense;
        public int TotalSpAttack => SpAttack;
        public int TotalSpDefense => SpDefense;
        public int TotalSpeed => Speed;

        public StatusCondition StatusCondition
        {
            get => Pokemon.StatusCondition;
            private set => Pokemon.StatusCondition = value;
        }

        public List<MoveSlot> Moves { get; set; }
        public HashSet<PokemonType> Types { get; set; }
        public int CritStage { get; set; }
        public bool IsFainted => Pokemon.IsFainted;
        public int Level => Pokemon.Level;
        public Modifier Modifier { get; }
        public StatStage StatStage { get; }
        public Memory Memory { get; }
        public Dictionary<VolatileConditionID, VolatileCondition> VolatileConditions { get; } = new();
        public int AttacksThisTurn { get; set; }

        #region Events

        public Action OnAttack;
        public Action OnHit;
        public Action OnHealthChanged;

        #endregion

        public Unit(Side side, Pokemon pokemon)
        {
            Side = side;
            Pokemon = pokemon;
            Moves = pokemon.Moves;
            Types = Sets.Of(Pokemon.Type1, Pokemon.Type2);

            Attack = Pokemon.Attack;
            Defense = Pokemon.Defense;
            SpAttack = Pokemon.SpAttack;
            SpDefense = Pokemon.SpDefense;
            Speed = Pokemon.Speed;

            Modifier = new Modifier(this);
            StatStage = new StatStage(this);
            Memory = new Memory(this);
        }

        public void UseMove(MoveBuilder move, Unit target)
        {
            move.Execute(this, target);
            Memory.LastUsed = move.Base;
        }

        public void ApplyDamage(Unit target, DamageDetail damage)
        {
            OnAttack?.Invoke();

            target.TakeDamage(damage);
            AttacksThisTurn++;

            Modifier.OnApplyDamage(this, damage.Base);
        }


        public void TakeDamage(DamageDetail damage)
        {
            Hp -= damage.Value;

            OnHit?.Invoke();
            OnHealthChanged?.Invoke();

            // battleManager.Log(damage.Messages);

            // CheckFaint();
        }

        public void RemoveStatusCondition()
        {
            StatusCondition.OnEnd();
            StatusCondition = null;
            // OnStatusChanged?.Invoke();
        }

        public void AddStatusCondition(StatusCondition condition)
        {
            if (StatusCondition is not null)
            {
                return;
            }

            StatusCondition = condition;
            condition.Bind(this).OnStart();
        }

        public void RemoveVolatileCondition(VolatileConditionID id)
        {
            if (VolatileConditions.Remove(id, out var condition))
            {
                condition.OnEnd();
            }
            // OnStatusChanged?.Invoke();
        }

        public void AddVolatileCondition(VolatileCondition condition)
        {
            VolatileConditions[condition.ID] = condition;
            condition.Bind(this).OnStart();
        }
    }
}