using Battle;

namespace Move.Effect
{
    public class Frenzy : MoveEffect
    {
        public override void Apply(Unit source, Unit target)
        {
            source.AddVolatileCondition(new Data.Volatile.Frenzy());
        }
    }
}
