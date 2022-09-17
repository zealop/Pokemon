using Battle;
using Data.Volatile;
using Sirenix.Serialization;
using UnityEngine;

namespace Move.Effect
{
    public class Bind : MoveEffect
    {
        [OdinSerialize] private readonly string message;
        
        public Bind(string message)
        {
            this.message = message;
        }

        public override void Apply(Unit source, Unit target)
        {
            var condition = new Bound(moveBuilder.moveBase, source, message);
            target.AddVolatileCondition(condition);
        }
    }
}
