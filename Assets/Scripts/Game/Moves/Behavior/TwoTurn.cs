using Game.Battles;
using Game.Condition;
using Game.Condition.Volatile;
using Sirenix.Serialization;

namespace Game.Moves.Behavior
{
    public class TwoTurn : Default
    {
        private const VolatileConditionID ID = VolatileConditionID.TwoTurnMove;
        [OdinSerialize] private readonly IMoveEffect effect;
        
        public override void Apply(MoveBuilder move, Unit source, Unit target)
        {
            if (source.VolatileConditions.ContainsKey(ID))
            {
                source.RemoveVolatileCondition(ID);
                base.Apply(move, source, target);
            }
            else
            {
                source.Modifier.OnTurnStartList.Add(() => LockAction(source, target));
                source.AddVolatileCondition(new TwoTurnMoveCondition());
            }
        }

        private void LockAction(Unit source, Unit target)
        {
            //TODO: replace with source.createMove
            var move = new MoveBuilder(source.Memory.LastUsed, null);
            source.Battle.Enqueue(move, source, target);
        }
    }
}