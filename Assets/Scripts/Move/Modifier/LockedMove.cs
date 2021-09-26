using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedMoveEffect : MoveEffect
{
    public override IEnumerator Run(BattleUnit source, BattleUnit target)
    {
        if(!source.Volatiles.ContainsKey(VolatileID.LockedMove))
        {
            yield return source.AddVolatileCondition(new VolatileLockedMove());
        }
    }
}

public class LockedMoveModifier : MoveModifier
{
    public override void ModifyMove()
    {
        _base.OnFail = Stop;
    }

    IEnumerator Stop(BattleUnit source, BattleUnit target)
    {
        yield return source.RemoveVolatileCondition(VolatileID.LockedMove);
    }
}