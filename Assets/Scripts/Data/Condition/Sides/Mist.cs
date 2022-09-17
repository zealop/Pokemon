using System.Collections.Generic;
using System.Linq;
using Battle;

namespace Data.Condition.Sides
{
    public class Mist : SideCondition
    {
        public override SideConditionID id => SideConditionID.Mist;

        private List<ContextHolder> contexts;
        private int counter;

        public Mist()
        {
            this.contexts = new List<ContextHolder>();
            this.counter = 5;
        }

        public override void Start()
        {
            contexts = side.Units.Select(u => new ContextHolder(u)).ToList();
            contexts.ForEach(c => c.Start());
            side.onTurnEndList.Add(CountDown);
        }

        public override void End()
        {
            contexts.ForEach(c => c.End());
            side.onTurnEndList.Remove(CountDown);
        }

        private void CountDown()
        {
            counter--;
            if (counter == 0)
            {
                side.RemoveSideCondition(id);
            }
        }

        private class ContextHolder
        {
            private readonly Unit unit;

            public ContextHolder(Unit unit)
            {
                this.unit = unit;
            }

            public void Start()
            {
                unit.Modifier.OnStatBoostList.Add(PreventNegativeStat);
            }

            public void End()
            {
                unit.Modifier.OnStatBoostList.Remove(PreventNegativeStat);
            }

            private bool PreventNegativeStat(BoostableStat stat, int boost, Unit source)
            {
                return boost >= 0 || unit == source;
            }
        }
    }
}