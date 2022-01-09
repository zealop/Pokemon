using UnityEngine;

public abstract class MoveAccuracy : MoveComponent
{
    /// <summary>Run accuracy check for move</summary>
    /// <returns>Returns true if pass accuracy check</returns>
    public abstract bool Apply(BattleUnit source, BattleUnit target);
}
public class DefaultAccuracy : MoveAccuracy
{
    public override bool Apply(BattleUnit source, BattleUnit target)
    {
        return Random.value <= HitChance(source, target) || move.Accuracy == 0;
    }

    private float HitChance(BattleUnit source, BattleUnit target)
    {
        return move.Accuracy * source.Accuracy * target.Evasion / 100f;
    }
}
