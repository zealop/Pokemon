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
        public override void Apply(BattleUnit source, BattleUnit target)
        {
            target.ApplyStatBoost(boosts);
        }
    }
}
