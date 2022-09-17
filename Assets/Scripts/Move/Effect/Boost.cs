using System.Collections.Generic;
using Battle;
using Sirenix.Serialization;

namespace Move.Effect
{
    public class Boost : MoveEffect
    {
        [OdinSerialize] private readonly Dictionary<BoostableStat, int> boosts = new Dictionary<BoostableStat, int>();

        public Boost(Dictionary<BoostableStat, int> boosts)
        {
            this.boosts = boosts;
        }

        public Boost(BoostableStat stat, int stage)
        {
            boosts.Add(stat, stage);
        }
        public override void Apply(Unit source, Unit target)
        {
            foreach ((var stat, int boost) in boosts)
            {
                target.ApplyStatBoost(stat, boost, source);
            }
        }
    }
}
