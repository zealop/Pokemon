using System;
using Game.Battles;
using Game.Condition.Status;

namespace Game.Condition
{
    public abstract class StatusCondition
    {
        public abstract StatusConditionID ID { get; }
        public Unit Unit { get; private set; }

        public static StatusCondition Of(StatusConditionID id) => id switch
        {
            StatusConditionID.Poison => new PoisonCondition(),
            StatusConditionID.Burn => new BurnCondition(),
            StatusConditionID.Paralyze => new ParalyzeCondition(),
            StatusConditionID.Freeze => new FreezeCondition(),
            StatusConditionID.Sleep => new SleepCondition(),
            StatusConditionID.Toxic => new ToxicCondition(),
            _ => throw new ArgumentOutOfRangeException(nameof(id), id, null)
        };

        public StatusCondition Bind(Unit unit)
        {
            Unit = unit;
            return this;
        }

        public abstract void OnStart();
        public abstract void OnEnd();

        protected void Log(string message)
        {
        }
    }
}