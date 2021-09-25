using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect : MoveEffect
{
    [SerializeField] StatusID condition;

    public override IEnumerator Run(BattleUnit source, BattleUnit target)
    {
        yield return target.SetStatusCondition(condition);
    }
}

public class TriEffect : MoveEffect
{
    List<StatusID> conditions = new List<StatusID>{ StatusID.BRN, StatusID.FRZ, StatusID.PRZ};
    public override IEnumerator Run(BattleUnit source, BattleUnit target)
    {
        StatusID condition = conditions[Random.Range(0, conditions.Count)];
        yield return target.SetStatusCondition(condition);
    }
}