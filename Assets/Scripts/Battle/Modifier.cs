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
        private static Queue<IEnumerator> AnimationQueue => BattleManager.I.AnimationQueue;
        private static DialogBox DialogBox => BattleManager.I.DialogBox;

        public Modifier(Unit unit)
        {
            this.unit = unit;
        }

        public readonly List<Action<MoveBase, Unit, int>> OnHitList =
            new List<Action<MoveBase, Unit, int>>();

        public readonly List<Func<MoveBase, Unit, float>> AttackerModList =
            new List<Func<MoveBase, Unit, float>>();

        public readonly List<Func<MoveBase, Unit, float>> DefenderModList =
            new List<Func<MoveBase, Unit, float>>();

        public readonly List<Func<float>> SpeedModList = new List<Func<float>>();

        public readonly PriorityList<Func<MoveBase, Unit, bool>> AccuracyMod =
            new PriorityList<Func<MoveBase, Unit, bool>>();

        public Func<MoveBase, bool> SemiInvulnerable = m => false;
        public Func<MoveBase, bool> Vulnerable = m => false;
        public Action<Unit, int> OnApplyDamage;


        public readonly List<Action> OnBeforeMoveList = new List<Action>();
        public readonly List<Action> OnTurnEndList = new List<Action>();
        public readonly List<Action<Unit>> OnMissList = new List<Action<Unit>>();

        public void OnHit(MoveBase move, Unit source, int damage)
        {
            OnHitList.ForEach(a => a(move, source, damage));

            // lastHitDamage = damage;
        }

        public float AttackerMod(MoveBase move, Unit target)
        {
            return AttackerModList.ToList().Aggregate(1f, (current, mod) => current * mod(move, target));
        }

        public float DefenderMod(MoveBase move, Unit source)
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

        public bool OnAccuracy(MoveBase move, Unit source)
        {
            bool result = false;
            foreach (var e in AccuracyMod.ToList())
            {
                result = e(move, source);
            }

            return result;
        }

        public void OnTurnEnd()
        {
            unit.AttacksThisTurn = 0;
            OnTurnEndList.ForEach(a => a());
        }

        public void OnMiss()
        {
            AnimationQueue.Enqueue(DialogBox.TypeDialog($"{unit.Name}'s missed!"));
            OnMissList.ForEach(a => a(unit));
        }
    }
}