using Battle;
using Data.Volatile;
using Sirenix.Serialization;
using UnityEngine;

namespace Move.Effect
{
    public class Recharge : MoveEffect
    {
        public override void Apply(Unit source, Unit target)
        {
            var condition = new Recharging();
            source.AddVolatileCondition(condition);
        }
    }
}
