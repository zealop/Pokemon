using System.Collections;
public abstract class MoveEffect : MoveComponent
{
    public abstract void Apply(BattleUnit source, BattleUnit target);
}
