using Battle;

namespace Data.Condition
{
    public abstract class SideCondition
    {
        public abstract SideConditionID id { get; }
        protected Side side;

        public void Bind(Side side)
        {
            this.side = side;
        }
        public abstract void Start();
        public abstract void End();
    }
}