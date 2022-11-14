using System;
using System.Collections.Generic;
using Game.Moves;

namespace Game.Battles
{
    public class Modifier
    {
        public Modifier(Unit unit)
        {
            Unit = unit;
        }

        private Unit Unit { get; }
        public List<Action> OnTurnStartList { get; } = new();
        public List<Action> OnTurnEndList { get; } = new();
        public List<Func<MoveBuilder, Unit, float>> AttackerModList { get; } = new();
        public List<Action> OnBeforeMoveList { get; } = new();
        public List<Action<MoveBase, Unit, int>> OnHitList { get; } = new();
        public List<Func<float>> SpeedModList { get; }= new();

        public void OnTurnStart()
        {
            OnTurnStartList.ForEach(a => a());
        }
        
        public float AttackerMod(object moveBuilder, Unit target)
        {
            return 1;
        }

        public float DefenderMod(MoveBuilder move, Unit source)
        {
            return 1;
        }

        public void OnMiss()
        {
        }

        public void OnAttack()
        {
        }

        public void OnApplyDamage(Unit unit, float damageBase)
        {
        }

        public bool IsSemiInvulnerable(object moveBuilder)
        {
            return false;
        }

        public bool IsVulnerable(object moveBuilder)
        {
            return false;
        }
    }
}