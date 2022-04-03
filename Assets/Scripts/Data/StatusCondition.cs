using System.Collections;
using System.Collections.Generic;
using Battle;
using Move;
using UnityEngine;

namespace Data
{
    public enum StatusID
    {
        PSN,
        BRN,
        SLP,
        PRZ,
        FRZ,
        TOX
    }

    public abstract class StatusCondition
    {
        public abstract StatusID ID { get; }
        protected BattleUnit Unit { get; private set; }
        protected static Queue<IEnumerator> AnimationQueue => BattleManager.I.AnimationQueue;
        protected static DialogBox DialogBox => BattleManager.I.DialogBox;

        private static T GetChild<T>(BattleUnit unit) where T:StatusCondition, new()
        {
            var condition = new T
            {
                Unit = unit
            };
            return condition;
        }

        public static StatusCondition Create(StatusID id, BattleUnit unit)
        {
            return id switch
            {
                StatusID.PSN => GetChild<StatusPoison>(unit),
                StatusID.BRN => GetChild<StatusBurn>(unit),
                StatusID.SLP => GetChild<StatusSleep>(unit),
                StatusID.PRZ => GetChild<StatusParalyze>(unit),
                StatusID.FRZ => GetChild<StatusFreeze>(unit),
                StatusID.TOX => GetChild<StatusToxic>(unit),
                _ => null,
            };
        }

        public static float CatchBonus(StatusCondition condition)
        {
            if (condition is null) return 1;

            return condition.ID switch
            {
                StatusID.PSN => 1.5f,
                StatusID.BRN => 1.5f,
                StatusID.SLP => 2f,
                StatusID.PRZ => 2f,
                StatusID.FRZ => 1.5f,
                StatusID.TOX => 1.5f,
                _ => 1f,
            };
        }

        public abstract void OnStart();
        public abstract void OnEnd();
    }

    public class StatusPoison : StatusCondition
    {
        public override StatusID ID => StatusID.PSN;
        private const float DamageModifier = 1 / 8f;
    
        public override void OnStart()
        {
            Unit.Modifier.OnTurnEndList.Add(PoisonDamage);

            AnimationQueue.Enqueue(DialogBox.TypeDialog($"{Unit.Name} is poisoned!"));
        }

        public override void OnEnd()
        {
            Unit.Modifier.OnTurnEndList.Remove(PoisonDamage);

            AnimationQueue.Enqueue(DialogBox.TypeDialog($"{Unit.Name} is cured of its poison!"));
        }

        private void PoisonDamage()
        {
            int damage = Mathf.FloorToInt(Unit.MaxHp * DamageModifier);
            Unit.TakeDamage(damage, $"{Unit.Name} is hurt by poison!");
        }
    }

    public class StatusBurn : StatusCondition
    {
        public override StatusID ID => StatusID.BRN;
        private const float DamageModifier = 1 / 16f;
        private const float AttackModifier = 1 / 2f;
    
        public override void OnStart()
        {
            Restart();

            AnimationQueue.Enqueue(DialogBox.TypeDialog($"{Unit.Name} is burned!"));
        }

        public override void OnEnd()
        {
            Unit.Modifier.AttackerModList.Remove(BurnMod);
            Unit.Modifier.OnTurnEndList.Remove(BurnDamage);

            AnimationQueue.Enqueue(DialogBox.TypeDialog($"{Unit.Name} is cured of its burn!"));
        }

        private void BurnDamage()
        {
            int damage = Mathf.FloorToInt(Unit.MaxHp * DamageModifier);
            Unit.TakeDamage(damage, $"{Unit.Name} is hurt by its burn!");
        }

        private static float BurnMod(MoveBase move, BattleUnit target)
        {
            return move.Category == MoveCategory.Physical ? AttackModifier : 1f;
        }

        private void Restart()
        {
            Unit.Modifier.AttackerModList.Add(BurnMod);
            Unit.Modifier.OnTurnEndList.Add(BurnDamage);
        }
    }

    public class StatusSleep : StatusCondition
    {
        public override StatusID ID => StatusID.SLP;
        private int counter;

        public void InitCounter(int turn = 0)
        {
            counter = turn == 0 ? Random.Range(1, 4) : turn;
        }
        public override void OnStart()
        {
            Restart();

            AnimationQueue.Enqueue(DialogBox.TypeDialog($"{Unit.Name} fell asleep!"));
        }

        public override void OnEnd()
        {
            Unit.Modifier.OnBeforeMoveList.Remove(SleepCheck);

            AnimationQueue.Enqueue(DialogBox.TypeDialog($"{Unit.Name} woke up!"));
        }

        private void SleepCheck()
        {
            if (counter > 0)
            {
                counter--;
                Unit.CanMove = false;

                AnimationQueue.Enqueue(DialogBox.TypeDialog($"{Unit.Name} is fast asleep!"));
            }
            else
            {
                Unit.RemoveStatusCondition();
            }
        }

        private void Restart()
        {
            Unit.Modifier.OnBeforeMoveList.Add(SleepCheck);
        }
    }

    public class StatusParalyze : StatusCondition
    {
        public override StatusID ID => StatusID.PRZ;
        private const float SpeedModifier = 0.5f;
        private const float ParalyzeChance = 0.25f;
    
        public override void OnStart()
        {
            Restart();

            AnimationQueue.Enqueue(DialogBox.TypeDialog($"{Unit.Name} is paralyzed!"));
        }

        public override void OnEnd()
        {
            Unit.Modifier.OnBeforeMoveList.Remove(ParalyzeCheck);
            Unit.Modifier.SpeedModList.Remove(ParalyzeSlow);

            AnimationQueue.Enqueue(DialogBox.TypeDialog($"{Unit.Name} is cured of its paralysis!"));
        }

        private void ParalyzeCheck()
        {
            if (Random.value <= ParalyzeChance)
            {
                Unit.CanMove = false;

                AnimationQueue.Enqueue(DialogBox.TypeDialog($"{Unit.Name} is fully paralyzed!"));
            }
        }

        private static float ParalyzeSlow()
        {
            return SpeedModifier;
        }

        private void Restart()
        {
            Unit.Modifier.OnBeforeMoveList.Add(ParalyzeCheck);
            Unit.Modifier.SpeedModList.Add(ParalyzeSlow);
        }
    }

    public class StatusFreeze : StatusCondition
    {
        public override StatusID ID => StatusID.FRZ;
        private const float ThawChance = 0.2f;
    
        public override void OnStart()
        {
            Restart();

            AnimationQueue.Enqueue(DialogBox.TypeDialog($"{Unit.Name} is frozen solid!"));
        }

        public override void OnEnd()
        {
            Unit.Modifier.OnBeforeMoveList.Remove(FreezeCheck);
            Unit.Modifier.OnHitList.Remove(FireHit);

            AnimationQueue.Enqueue(DialogBox.TypeDialog($"{Unit.Name} thawed out!"));
        }

        private void FreezeCheck()
        {
            if (Random.value <= ThawChance)
            {
                Unit.RemoveStatusCondition();
            }
            else
            {
                Unit.CanMove = false;
                AnimationQueue.Enqueue(DialogBox.TypeDialog($"{Unit.Name} is frozen solid!"));
            }
        }

        private void FireHit(MoveBase move, BattleUnit source, int damage)
        {
            if (move.Type == PokemonType.Fire)
            {
                Unit.RemoveStatusCondition();
            }
        }

        private void Restart()
        {
            Unit.Modifier.OnBeforeMoveList.Add(FreezeCheck);
            Unit.Modifier.OnHitList.Add(FireHit);
        }
    }

    public sealed class StatusToxic : StatusCondition
    {
        public override StatusID ID => StatusID.TOX;
        private const float DamageModifier = 1 / 16f;
        private int counter;
    
        public override void OnStart()
        {
            Restart();

            AnimationQueue.Enqueue(DialogBox.TypeDialog($"{Unit.Name} is badly poisoned!"));
        }

        public override void OnEnd()
        {
            Unit.Modifier.OnTurnEndList.Add(ToxicDamage);

            AnimationQueue.Enqueue(DialogBox.TypeDialog($"{Unit.Name} is cured of its poison!"));
        }

        private void ToxicDamage()
        {
            int damage = Mathf.FloorToInt(Unit.MaxHp * DamageModifier * counter);
            Unit.TakeDamage(damage, $"{Unit.Name} is hurt by poison!");
        }

        private void Restart()
        {
            counter = 1;
            Unit.Modifier.OnTurnEndList.Add(ToxicDamage);
        }
    }
}