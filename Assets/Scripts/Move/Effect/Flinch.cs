using Battle;

namespace Move.Effect
{
    public class Flinch : MoveEffect
    {
        public override void Apply(BattleUnit source, BattleUnit target)
        {
            target.AddVolatileCondition(new Data.Volatile.Flinch());
        }
    }
}