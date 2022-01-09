using UnityEngine;

public class StatusEffect : MoveEffect
{
    [SerializeField] private StatusID condition;
    public override void Apply(BattleUnit source, BattleUnit target)
    {
        target.SetStatusCondition(condition);
    }
}
