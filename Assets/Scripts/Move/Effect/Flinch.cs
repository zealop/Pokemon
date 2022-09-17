using Battle;
using Data;

namespace Move.Effect
{
    public class Flinch : MoveEffect
    {
        public override void Apply(Unit source, Unit target)
        {
            source.AddVolatileCondition(new Data.Volatile.Flinch());
        }
    }
}