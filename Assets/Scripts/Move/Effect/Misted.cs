using Battle;
using Data.Condition.Sides;
using Data.Volatile;
using Sirenix.Serialization;
using UnityEngine;

namespace Move.Effect
{
    public class Misted : MoveEffect
    {
        public override void Apply(Unit source, Unit target)
        {
            var condition = new Mist();
            target.side.AddCondition(condition);
        }
    }
}
