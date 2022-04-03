using Battle;
using Move;

public abstract class MoveAccuracy : MoveComponent
{
    /// <summary>Run accuracy check for move</summary>
    /// <returns>Returns true if pass accuracy check</returns>
    public abstract bool Apply(BattleUnit source, BattleUnit target);
}