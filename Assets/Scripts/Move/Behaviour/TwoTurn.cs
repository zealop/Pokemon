using System;
using Battle;
using Data;
using Data.Volatile;
using Sirenix.Serialization;

namespace Move.Behaviour
{
    public class TwoTurn : Default
    {
        [OdinSerialize] private readonly string message;
        [OdinSerialize] private readonly TwoTurnMove condition;

        public TwoTurn(string message, TwoTurnMove condition)
        {
            this.message = message;
            this.condition = condition;
        }

        public override void Apply(Unit source, Unit target, Action consumePp)
        {
            if (!IsReady(source))
            {
                RegisterMove(source, consumePp);
                
                condition?.BindToUnit(source);
                source.AddVolatileCondition(condition);

                Log(message, source);
            }
            else
            {
                source.RemoveVolatileCondition(VolatileID.TwoTurnMove);
                base.Apply(source, target);
            }
        }
        
        private static bool IsReady(Unit source)
        {
            return source.Volatiles.ContainsKey(VolatileID.TwoTurnMove);
        }
    }
}