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

        public override void Apply(BattleUnit source, BattleUnit target)
        {
            target.SetStatusCondition(condition);
        }
    }
}
