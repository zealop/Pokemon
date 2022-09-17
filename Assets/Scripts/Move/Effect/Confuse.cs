using Battle;
using Data.Volatile;

namespace Move.Effect
{
    public class Confuse : MoveEffect
    {
        public override void Apply(Unit source, Unit target)
        {
            target.AddVolatileCondition(new Confused());
        }
    }
}
