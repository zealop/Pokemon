using Game.Battles;

namespace Game.Condition
{
    public abstract class VolatileCondition
    {
        public abstract VolatileConditionID ID { get; }

        protected Unit Unit { get; private set; }

        public VolatileCondition Bind(Unit unit)
        {
            Unit = unit;
            return this;
        }

        public virtual void OnStart()
        {
            
        }

        public virtual void OnEnd()
        {
            
        }
    }
}