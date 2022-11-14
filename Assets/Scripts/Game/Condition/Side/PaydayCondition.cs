namespace Game.Condition.Side
{
    public class PaydayCondition : SideCondition
    {
        public override SideConditionID ID => SideConditionID.Payday;
        public int Count { get; private set; }

        public PaydayCondition(int count)
        {
            Count = count;
        }

        public void Add(int count)
        {
            Count += count;
        }

        
    }
}