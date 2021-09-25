using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryEffect : MoveModifier
{
    [SerializeField] float chance;
    [SerializeField] MoveEffect effect;
    public override void ModifyMove()
    {
        _base.SecondaryEffectList.Add(ChanceEffect);
    }

    IEnumerator ChanceEffect(BattleUnit source, BattleUnit target)
    {
        if (Random.value < chance)
            yield return effect.Run(source, target);
    }
}
