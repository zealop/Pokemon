using Battle;
using Data.Condition;
using Sirenix.Serialization;

namespace Move.Effect
{
    public class Status : MoveEffect
    {
        [OdinSerialize] private readonly StatusID condition;

        public Status(StatusID condition)
        {
            this.condition = condition;
        }

        public override void Apply(Unit source, Unit target)
        {
            target.SetStatusCondition(condition);
        }
    }
}
