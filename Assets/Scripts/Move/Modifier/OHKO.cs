using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OHKO : MoveModifier
{
    [SerializeField] bool isSheerCold;
    public override void ModifyMove()
    {
        _base.Damage = OHKODamage;
        _base.HitChance = OHKOAccuracy;
    }

    int OHKODamage(BattleUnit source, BattleUnit target)
    {
        source.System.MessageQueue.Enqueue($"It's a one-hit KO!");
        return target.MaxHP;
    }

    float OHKOAccuracy(BattleUnit source, BattleUnit target)
    {
        //20% for sheer cold used by non ice type, 30% every other case
        float baseAccuracy = isSheerCold && !source.Types.Contains(_base.Type) ? 0.2f : 0.3f;

        float result = source.Level < target.Level ? 0 : source.Level - target.Level + baseAccuracy;
        return result;
    }
}

