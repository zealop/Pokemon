using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Move;

namespace Battle
{
    public class Modifier
    {
        private readonly Unit unit;
        private static Queue<IEnumerator> AnimationQueue => BattleManager.i.AnimationQueue;
        private static DialogBox DialogBox => BattleManager.i.DialogBox;

        public Modifier(Unit unit)
        {
            this.unit = unit;
        }

        public readonly List<Action<MoveBase, Unit, int>> OnHitList = new();

        public readonly List<Func<MoveBuilder, Unit, float>> AttackerModList = new();

        public readonly List<Func<MoveBuilder, Unit, float>> DefenderModList = new();

        public readonly List<Func<float>> SpeedModList = new();

        public readonly PriorityList<Func<MoveBuilder, Unit, bool>> AccuracyMod = new();

        public Func<MoveBuilder, bool> SemiInvulnerable = _ => false;
        public Func<MoveBuilder, bool> Vulnerable = _ => false;
        public Action<Unit, int> OnApplyDamage;


        public readonly List<Action> OnBeforeMoveList = new();
        public readonly List<Action> OnTurnEndList = new();
        public readonly List<Action<Unit>> OnMissList = new();
        public readonly List<Func<BoostableStat, int, Unit, bool>> OnStatBoostList = new();
        public void OnHit(MoveBase move, Unit source, int damage)
        {
            OnHitList.ForEach(a => a(move, source, damage));

            // lastHitDamage = damage;
        }

        public float AttackerMod(MoveBuilder move, Unit target)
        {
            return AttackerModList.ToList().Aggregate(1f, (current, mod) => current * mod(move, target));
        }

        public float DefenderMod(MoveBuilder move, Unit source)
        {
            return DefenderModList.ToList().Aggregate(1f, (current, mod) => current * mod(move, source));
        }

        public float SpeedMod()
        {
            return SpeedModList.ToList().Aggregate(1f, (current, mod) => current * mod());
        }

        public void OnBeforeMove()
        {
            OnBeforeMoveList.ForEach(a => a());
        }

        public bool OnAccuracy(MoveBuilder move, Unit source)
        {
            var result = false;
            foreach (var e in AccuracyMod.ToList())
            {
                result = e(move, source);
            }

            return result;
        }

        public void OnTurnEnd()
        {
            unit.AttacksThisTurn = 0;
            foreach (var e in OnTurnEndList.ToList())
            {
                e();
            }
        }

        public void OnMiss()
        {
            AnimationQueue.Enqueue(DialogBox.TypeDialog($"{unit.Name}'s missed!"));
            OnMissList.ForEach(a => a(unit));
        }
        
        
        
        public void OnStatBoost(BoostableStat stat, int boost, Unit source)
        {
            var result = true;
            OnStatBoostList.ForEach(a => result = a(stat, boost, source));
        }
    }
}