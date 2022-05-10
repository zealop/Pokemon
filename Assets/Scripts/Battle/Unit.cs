using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using Move;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Battle
{
    public class Unit
    {
        [SerializeField] private bool isPlayerUnit;
        public Pokemon Pokemon { get; private set; }
        public string Name => Pokemon.Name;
        public int Level => Pokemon.Level;
        public List<Move.Move> Moves { get; private set; }

        public StatusCondition Status
        {
            get => Pokemon.Status;
            private set => Pokemon.Status = value;
        }

        public PokemonParty Party => isPlayerUnit ? BattleManager.PlayerParty : BattleManager.TrainerParty;
        public Modifier Modifier { get; private set; }
        
        public bool LockedAction;
        public bool LockedMove;
        public bool EndTurn;

        public Func<bool> CanSwitch = () => true;

        public StatStage StatStage { get; set; }
        public Dictionary<VolatileID, VolatileCondition> Volatiles;

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
        public int Hp => Pokemon.HP;
        public int MaxHp => Pokemon.MaxHP;
        public float Accuracy => StatStage[BoostableStat.Accuracy];
        public float Evasion => StatStage[BoostableStat.Evasion];

        public List<PokemonType> Types { get; private set; }
        public int CritStage => StatStage.CritStage;
        public bool IsFainted => Hp <= 0;
        private static BattleManager BattleManager => BattleManager.I;
        
        public int AttacksThisTurn;
        public bool CanMove;

        public void Setup(Pokemon pokemon)
        {
            Pokemon = pokemon;
            Types = new List<PokemonType> {pokemon.Base.Type1, pokemon.Base.Type2};

            Moves = new List<Move.Move>(Pokemon.Moves);

            StatStage = new StatStage();

            Volatiles = new Dictionary<VolatileID, VolatileCondition>();
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

        public void UseMove(Move.Move move, Unit target)
        {
            move.Execute(this, target);

            if (AttacksThisTurn > 1)
            {
                BattleManager.Log($"Hit {AttacksThisTurn} times!");
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
            Pokemon.UpdateHP(Hp - damage.Value);

            OnHit?.Invoke();
            OnHealthChanged?.Invoke();
            
            BattleManager.Log(damage.Messages);

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
                BattleManager.Log($"{Name} fainted!");
            }
        }

        public void SetStatusCondition(StatusID id)
        {
            if (Status is object)
            {
                BattleManager.Log($"It doesn't affect {Name}");
            }
            else
            {
                Status = StatusCondition.Create(id, this);
                Status.OnStart();
                
                OnStatusChanged?.Invoke();
            }
        }

        public void RemoveStatusCondition()
        {
            Status.OnEnd();
            Status = null;
            OnStatusChanged?.Invoke();
        }

        public void AddVolatileCondition(VolatileCondition condition)
        {
            Volatiles[condition.ID] = condition;
            condition.OnStart();
        }

        public void RemoveVolatileCondition(VolatileID id)
        {
            Volatiles[id].OnEnd();
            Volatiles.Remove(id);
        }

        public void ApplyStatBoost(Dictionary<BoostableStat, int> boosts)
        {
            foreach (var kvp in boosts)
            {
                var stat = kvp.Key;
                int boost = kvp.Value;

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

                BattleManager.Log($"{Name}'s {stat} {message}!");
            }
        }
        
        public Move.Move GetRandomMove()
        {
            int r = Random.Range(0, Moves.Count);
            return Moves[r];
        }

        public IEnumerator GainEXP(Pokemon defeatedPokemon, bool isTrainerBattle)
        {
            int exp = defeatedPokemon.ExpReward;
            float trainerBonus = isTrainerBattle ? 1.5f : 1f;
            int expGain = Mathf.FloorToInt(exp * trainerBonus / 7);

            Pokemon.EXP += expGain;

            yield return BattleManager.I.DialogBox.TypeDialog($"{Name} gained {expGain} experience points!");
            // yield return Visual.HUD.UpdateExp();
        }

        public IEnumerator LevelUp()
        {
            Pokemon.LevelUp();

            yield return BattleManager.I.DialogBox.TypeDialog($"{Name} grew to level {Level}!");

            yield return CheckForNewMoves();
        }

        private IEnumerator CheckForNewMoves()
        {
            var newMoves = Pokemon.GetMovesToLearnOnLevelUp();

            foreach (var move in newMoves)
            {
                var newMove = new Move.Move(move);
                Pokemon.Moves.Add(newMove);
                Moves.Add(newMove);
                if (Pokemon.Moves.Count < 4)
                {
                    yield return BattleManager.I.DialogBox.TypeDialog($"{Name} learned {move.Name}!");
                }
                else
                {
                    yield return BattleManager.I.DialogBox.TypeDialog($"{Name} wants to learn {move.Name}!");
                    BattleManager.I.OpenLearnMoveScreen();
                }
            }
        }
    }
}