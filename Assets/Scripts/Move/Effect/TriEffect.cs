using UnityEngine;

public class TriEffect : MoveEffect
{
    private static readonly StatusID[] statuses = new StatusID[] { StatusID.BRN, StatusID.PRZ, StatusID.FRZ };
    public override void Apply(BattleUnit source, BattleUnit target)
    {
        StatusID status = statuses[Random.Range(0, statuses.Length)];
        target.SetStatusCondition(status);
    }
}
