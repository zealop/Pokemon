using UnityEngine;

public abstract class MoveBehaviour : MoveComponent
{
    /// <summary>Run accuracy check for move</summary>
    /// <returns>Returns true if pass accuracy check</returns>
    public abstract bool Apply(BattleUnit source, BattleUnit target);
}
