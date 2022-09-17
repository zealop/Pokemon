using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Data.Condition;
using Move;
using Pokemons;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Battle
{
    public class Unit
    {
        private static BattleManager battleManager => BattleManager.i;
        public Pokemon Pokemon { get; private set; }
        public string Name => Pokemon.Name;
        public int Level => Pokemon.Level;
        public List<MoveSlot> Moves { get; private set; }
        
        public Side side { get; }
        public Unit(Pokemon pokemon, Side side)
        {
            this.Pokemon = pokemon;
            this.side = side;
        }
        public Status status
        {
            get => Pokemon.Status;
            private set => Pokemon.Status = value;
        }

        public PokemonPartyMono party { get; private set; }

        public Modifier Modifier { get; private set; }

        public bool LockedAction;
        public bool LockedMove;
        public bool EndTurn;

        public Func<bool> CanSwitch = () => true;

        public StatStage StatStage { get; set; }
        public readonly Dictionary<VolatileID, VolatileCondition> Volatiles = new();

        private Dictionary<Unit, MoveBase> MirrorMoves;
        public int Weight => Pokemon.Base.Weight;
        public int Height => Pokemon.Base.Height;

        private int attack;
        private int defense;
        private int spAttack;
        private int spDefense;
        private int speed;
        public int Attack => Mathf.FloorToInt(attack * StatStage[BoostableStat.Attack]);
        public int Defense => Mathf.FloorToInt(defense * StatStage[BoostableStat.Defense]);
        public int SpAttack => Mathf.FloorToInt(spAttack * StatStage[BoostableStat.SpAttack]);
        public int SpDefense => Mathf.FloorToInt(spDefense * StatStage[BoostableStat.SpDefense]);
        public int Speed => Mathf.FloorToInt(speed * StatStage[BoostableStat.Speed] * Modifier.SpeedMod());
        public int Hp => Pokemon.Hp;
        public int MaxHp => Pokemon.MaxHp;
        public float Accuracy => StatStage[BoostableStat.Accuracy];
        public float Evasion => StatStage[BoostableStat.Evasion];

        public List<PokemonType> Types { get; private set; }
        public int CritStage => StatStage.CritStage;
        public bool IsFainted => Hp <= 0;

        public int AttacksThisTurn;
        public bool CanMove;

        public void Setup(Pokemon pokemon)
        {
            Pokemon = pokemon;
            Types = new List<PokemonType> {pokemon.Base.Type1, pokemon.Base.Type2};

            Moves = new List<MoveSlot>(Pokemon.Moves);
            Moves.ForEach(m => m.Bind(this));


            StatStage = new StatStage();

            Volatiles.Clear();
            MirrorMoves = new Dictionary<Unit, MoveBase>();
            Modifier = new Modifier(this);

            attack = pokemon.Attack;
            defense = pokemon.Defense;
            spAttack = pokemon.SpAttack;
            spDefense = pokemon.SpDefense;
            speed = pokemon.Speed;
        }

        public void Transform(Pokemon pokemon)
        {
            attack = pokemon.Attack;
            defense = pokemon.Defense;
            spAttack = pokemon.SpAttack;
            spDefense = pokemon.SpDefense;
            speed = pokemon.Speed;
            // OnTransform?.Invoke()
        }

        public MoveBase LastUsedMove { get; set; }
        public int LastSelectedMoveSlot { get; set; }

        public void UseMove(MoveBuilder move, Unit target)
        {
            move.Execute(this, target);

            if (AttacksThisTurn > 1)
            {
                battleManager.Log($"Hit {AttacksThisTurn} times!");
            }
        }

        #region event to interact with UI elements

        public event Action OnHealthChanged;
        public event Action OnHit;
        public event Action OnFaint;
        public event Action OnStatusChanged;
        public event Action OnAttack;

        #endregion

        public void TakeDamage(DamageDetail damage)
        {
            Pokemon.UpdateHp(Hp - damage.Value);

            OnHit?.Invoke();
            OnHealthChanged?.Invoke();

            battleManager.Log(damage.Messages);

            CheckFaint();
        }

        public void ApplyDamage(Unit target, DamageDetail damage)
        {
            OnAttack?.Invoke();

            target.TakeDamage(damage);
            AttacksThisTurn++;

            Modifier.OnApplyDamage?.Invoke(this, damage.Value);
        }

        public void TakeDamage(int damage, string message)
        {
            TakeDamage(new DamageDetail(damage, message));
        }

        private void CheckFaint()
        {
            if (IsFainted)
            {
                OnFaint?.Invoke();
                battleManager.Log($"{Name} fainted!");
            }
        }

        public void SetStatusCondition(StatusID id)
        {
            if (status is object)
            {
                battleManager.Log($"It doesn't affect {Name}");
            }
            else
            {
                status = Status.Create(id, this);
                status.OnStart();

                OnStatusChanged?.Invoke();
            }
        }

        public void RemoveStatusCondition()
        {
            status.OnEnd();
            status = null;
            OnStatusChanged?.Invoke();
        }

        public void AddVolatileCondition(VolatileCondition condition)
        {
            if (Volatiles.ContainsKey(condition.ID)) return;
            
            Volatiles[condition.ID] = condition;
            condition.Bind(this);
            condition.OnStart();
        }

        public void RemoveVolatileCondition(VolatileID id)
        {
            Volatiles[id].OnEnd();
            Volatiles.Remove(id);
        }

        public void ApplyStatBoost(BoostableStat stat, int boost, Unit source)
        {
            Modifier.OnStatBoost(stat, boost, source);
            
            string message = StatStage.Value[stat] switch
            {
                6 => "can't go any higher",
                -6 => "can't go any lower",
                _ => boost switch
                {
                    -3 => "severely fell",
                    -2 => "harshly fell",
                    -1 => "fell",
                    1 => "rose",
                    2 => "rose sharply",
                    3 => "rose drastically",
                    _ => ""
                }
            };

            StatStage.Value[stat] = Mathf.Clamp(StatStage.Value[stat] + boost, -6, 6);

            battleManager.Log($"{Name}'s {stat} {message}!");
        }

        public MoveSlot GetRandomMove()
        {
            int r = Random.Range(0, Moves.Count);
            return Moves[r];
        }

        public IEnumerator GainEXP(Pokemon defeatedPokemon, bool isTrainerBattle)
        {
            int exp = defeatedPokemon.ExpReward;
            float trainerBonus = isTrainerBattle ? 1.5f : 1f;
            int expGain = Mathf.FloorToInt(exp * trainerBonus / 7);

            Pokemon.Exp += expGain;

            yield return BattleManager.i.DialogBox.TypeDialog($"{Name} gained {expGain} experience points!");
            // yield return Visual.HUD.UpdateExp();
        }

        public IEnumerator LevelUp()
        {
            Pokemon.LevelUp();

            yield return BattleManager.i.DialogBox.TypeDialog($"{Name} grew to level {Level}!");

            yield return CheckForNewMoves();
        }

        private IEnumerator CheckForNewMoves()
        {
            var newMoves = Pokemon.GetMovesToLearnOnLevelUp();

            foreach (var move in newMoves)
            {
                var newMove = new Move.MoveSlot(move);
                Pokemon.Moves.Add(newMove);
                Moves.Add(newMove);
                if (Pokemon.Moves.Count < 4)
                {
                    yield return BattleManager.i.DialogBox.TypeDialog($"{Name} learned {move.Name}!");
                }
                else
                {
                    yield return BattleManager.i.DialogBox.TypeDialog($"{Name} wants to learn {move.Name}!");
                    BattleManager.i.OpenLearnMoveScreen();
                }
            }
        }
    }
}