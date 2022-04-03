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

        public override void Apply(BattleUnit source, BattleUnit target)
        {
            var condition = new Bound(move, source);
            target.AddVolatileCondition(condition);
            QueueMessage(source, target);
        }

        private void QueueMessage(BattleUnit source, BattleUnit target)
        {
            Log(message, source, target);
        }
    }
}
