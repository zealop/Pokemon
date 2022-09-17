using Battle;
using Data.Volatile;

namespace Move.Effect
{
    public class Disable : MoveEffect
    {
        public override void Apply(Unit source, Unit target)
        {
            if (target.LastUsedMove is null)
            {
                OnFail();
                return;
            }
            target.AddVolatileCondition(new Disabled(target.LastUsedMove));
        }
    }
}
