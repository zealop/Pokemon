using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostEffect : MoveEffect
{
    [OdinSerialize] private Dictionary<BoostableStat, int> boosts = new Dictionary<BoostableStat, int>();
    public override void Apply(BattleUnit source, BattleUnit target)
    {
        target.ApplyStatBoost(boosts);
    }
}
