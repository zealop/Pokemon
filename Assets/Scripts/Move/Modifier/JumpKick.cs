using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpKick : MoveModifier
{
    public override void ModifyMove()
    {
        _base.OnFail = Crash;
    }

    IEnumerator Crash(BattleUnit source, BattleUnit target)
    {
        yield return source.System.DialogBox.TypeDialog($"{source.Name} kept going and crashed!");
        yield return source.TakeDamage(source.MaxHP / 2);
    }
}