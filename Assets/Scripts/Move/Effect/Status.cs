using Battle;
using Data;
using UnityEngine;

namespace Move.Effect
{
    public class Status : MoveEffect
    {
        [SerializeField] private readonly StatusID condition;

        public Status(StatusID condition)
        {
            this.condition = condition;
        }

        public override void Apply(Unit source, Unit target)
        {
            target.SetStatusCondition(condition);
        }
    }
}
