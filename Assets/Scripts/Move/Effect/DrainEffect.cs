using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrainEffect : MoveEffect
{
    [SerializeField] float ratio;
    public override IEnumerator Run(BattleUnit source, BattleUnit target)
    {
        yield return source.TakeDamage(-Mathf.FloorToInt(target.LastHitDamage * ratio));
        yield return BattleSystem.Instance.DialogBox.TypeDialog($"{target.Name} had its energy drained!");
    }
}
