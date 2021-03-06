using Battle;
using Data;
using UnityEngine;

namespace Move.Effect
{
    public class TriAttack : MoveEffect
    {
        private static readonly StatusID[] Statuses = { StatusID.BRN, StatusID.PRZ, StatusID.FRZ };
        public override void Apply(BattleUnit source, BattleUnit target)
        {
            var status = Statuses[Random.Range(0, Statuses.Length)];
            target.SetStatusCondition(status);
        }
    }
}
