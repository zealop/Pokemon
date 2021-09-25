using Sirenix.Serialization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostEffect : MoveEffect
{
    [SerializeField] bool self;
    [OdinSerialize] Dictionary<BoostableStat, int> boosts = new Dictionary<BoostableStat, int>();
    public override IEnumerator Run(BattleUnit source, BattleUnit target)
    {
        var booster = self ? source : target;

        yield return booster.ApplyStatBoost(boosts);
    }
}
