using Battle;

namespace Move.Effect
{
    public class Flinch : MoveEffect
    {
        public override void Apply(Unit source, Unit target)
        {
            target.AddVolatileCondition(new Data.Volatile.Flinch());
        }
    }
}