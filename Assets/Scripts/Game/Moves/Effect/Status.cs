using Game.Battles;
using Game.Condition;
using Sirenix.Serialization;

namespace Game.Moves.Effect
{
    public class Status : IMoveEffect
    {
        [OdinSerialize] private readonly StatusConditionID status;

        public Status(StatusConditionID status)
        {
            this.status = status;
        }

        public void Apply(MoveBuilder move, Unit source, Unit target)
        {
            target.AddStatusCondition(StatusCondition.Of(status));
        }
    }
}