using Battle;
using Data.Volatile;
using UnityEngine;

namespace Move.Effect
{
    public class Bind : MoveEffect
    {
        [SerializeField] private readonly string message;
        
        public Bind(string message)
        {
            this.message = message;
        }

        public override void Apply(Unit source, Unit target)
        {
            var condition = new Bound(move, source);
            target.AddVolatileCondition(condition);
            QueueMessage(source, target);
        }

        private void QueueMessage(Unit source, Unit target)
        {
            Log(message, source, target);
        }
    }
}
