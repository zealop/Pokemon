using Game.Battles;
using Game.Condition;
using Game.Condition.Side;
using Sirenix.Serialization;
using Utils;

namespace Game.Moves.Effect
{
    public class Payday : IMoveEffect
    {
        private const string Message = "Coins scattered on the ground!";
        [OdinSerialize] private readonly int multiplier = 5;
        public void Apply(MoveBuilder move, Unit source, Unit target)
        {
            var sideConditions = source.Side.Conditions;
            var count = source.Level * multiplier;
            if (sideConditions.TryGetValueAs(SideConditionID.Payday, out PaydayCondition payday))
            {
                payday.Add(count);
            }
            else
            {
                sideConditions[SideConditionID.Payday] = new PaydayCondition(count);
            }
        }
    }
}